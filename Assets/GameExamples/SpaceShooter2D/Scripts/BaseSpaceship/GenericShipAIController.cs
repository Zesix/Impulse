using UnityEngine;
using System.Collections;

namespace SpaceShooter2D
{

    /// <summary>
    /// A generic AI controller that will make a ship patrol and chase members of an enemy faction
    /// that are within detection range.
    /// </summary>
    [RequireComponent(typeof(GenericShipModel))]
    [RequireComponent(typeof(SphereDetector))]
    [RequireComponent(typeof(SphereCollider))]
    public class GenericShipAIController : MonoBehaviour
    {
        // Our ship model.
        protected GenericShipModel model;

        // Our ship view.
        protected GenericShipView view;

        // Collider
        private SphereCollider _sphereCollider;

        // Our detector.
        [SerializeField] private SphereDetector _myDetector;

        // Our AI behavior update rate. Increase this if you want the AI to stick to a behavior longer.
        [SerializeField] private float _behaviorChangeRate = 1.0f;

        // Our waypoint manager. If this is not assigned, then the ship will automatically patrol in a square formation.
        [SerializeField] private WaypointPathManager _waypointManager;

        // Forward distance threshold. Used when generating a random point in front of the ship during square formation patrolling.
        [SerializeField]
        [Range(1, 100)] private float _forwardPatrolDistance = 6f;

        // Random forward point. This is the travel destination that is randomly generated during square formation patrolling.
        private Vector3 _forwardSquarePatrolPoint;

        // Chase parameters
        [SerializeField] private float _chaseLockDuration = 3.0f;
        [SerializeField] private bool _strafeAroundTarget = true;
        [SerializeField]
        [Range(1, 100)] private float _strafeDistanceFromTarget = 6f;
        [SerializeField]
        [Range(1, 100)] private float _distanceFromTargetVariance = 2.0f;
        [SerializeField]
        [Range(1, 100)] private float _varianceInterval = 2.0f;

        private float _finalDistanceFromTarget;
        private float _currentStrafeAngle;
        private float _currentStrafeSign = 1;
        private bool _lockedInChase;

        // States.
        [SerializeField] private GenericShipAiState _startState;

        [SerializeField] private GenericShipAiState _currentState;

        private enum GenericShipAiState
        {
            Idle,
            WaypointPatrolling,
            SquarePatrolling,
            Chasing
        }

        // Use this for initialization
        protected virtual void Start()
        {
            model = GetComponent<GenericShipModel>();
            view = GetComponent<GenericShipView>();
            _myDetector = GetComponent<SphereDetector>();

            if (_myDetector == null)
            {
                Debug.LogError("No Detector attached to " + gameObject + "!");
            }

            // Check if initial state is specified. If not, and there is no waypoint manager, then begin in square patrol mode.
            // Otherwise, begin in waypoint patrol mode.
            if (_startState == GenericShipAiState.Idle && _waypointManager == null)
                _currentState = GenericShipAiState.SquarePatrolling;
            if (_startState == GenericShipAiState.Idle && _waypointManager != null)
                _currentState = GenericShipAiState.WaypointPatrolling;

            // If we are patrolling and have no waypoints, then begin patrolling in square formation.
            if (_currentState == GenericShipAiState.SquarePatrolling)
                StartCoroutine(SquareFormationPatrol());

            // Begin custom update loop for AI.
            InvokeRepeating(nameof(UpdateAiController), 0.0f, _behaviorChangeRate);
        }

        // Updates the AI controller.
        private void UpdateAiController()
        {
            if (_currentState == GenericShipAiState.SquarePatrolling)
            {
                // If we are in square formation patrol and have reached our current forward point, then turn left and generate a new forward point.
                if (transform.position == _forwardSquarePatrolPoint)
                {
                    // Turn some number of degrees left.
                    transform.Rotate(new Vector3(0, 0, Random.Range(0, 90)));

                    StartCoroutine(SquareFormationPatrol());
                }
            }

            // Check if there is an enemy within range.
            var closestEnemy = _myDetector.ClosestEnemy();

            // If we have an enemy, then chase and attack it. Otherwise, resume patrolling.
            // Don't execute this if we're in lock chase 
            if (closestEnemy != null && !_lockedInChase)
            {
                _currentState = GenericShipAiState.Chasing;

                StartCoroutine(LockChase());
                StartCoroutine(ChaseAttackEnemyVariance());
                StartCoroutine(ChaseAttackEnemy(closestEnemy));
            }
            else if (closestEnemy == null)
            {
                if (_waypointManager != null)
                    _currentState = GenericShipAiState.WaypointPatrolling;
                _currentState = GenericShipAiState.SquarePatrolling;
            }

        }

        private void AttackTarget(GameObject enemy)
        {
            // While facing the enemy, shoot.
            if (IsFacingTarget(enemy))
            {
                model.SetFireInput(true);
            }
            if (!IsFacingTarget(enemy))
            {
                model.SetFireInput(false);
            }
        }

        /// <summary>
        /// Returns if the current gameobject is facing the target gameobject
        /// </summary>
        /// <returns></returns>
        private bool IsFacingTarget(GameObject target)
        {
            var AngleThreshold = 30.0f;
            // Note: this is for 2D only (XY plane). If you're going to use 3D, replace transform.up with transform.forward
            return AngleThreshold >= Vector3.Angle((target.transform.position - transform.position).normalized, transform.up);
        }

        private Vector3 GetRandomForwardPosition()
        {
            var randomForwardPosition = transform.position + (Vector3.up * Random.Range(1, _forwardPatrolDistance));
            return randomForwardPosition;
        }

        private IEnumerator SquareFormationPatrol()
        {
            if (_currentState == GenericShipAiState.SquarePatrolling)
            {
                // Get random forward position and set it as destination.
                _forwardSquarePatrolPoint = GetRandomForwardPosition();
                model.SetDestinationInput(_forwardSquarePatrolPoint);
                while (transform.position != _forwardSquarePatrolPoint)
                {
                    yield return new WaitForSeconds(_behaviorChangeRate);
                }
            }
        }

        private IEnumerator ChaseAttackEnemy(GameObject enemy)
        {
            while (_currentState == GenericShipAiState.Chasing)
            {
                // Strafe toward enemy.
                if (_strafeAroundTarget)
                {
                    // Get current target angle
                    _currentStrafeAngle = (_currentStrafeAngle + model.MaxAcceleration * _currentStrafeSign * Time.fixedDeltaTime) % 360.0f;

                    // Get position around target
                    var targetPosition = enemy.transform.position +
                                             Quaternion.Euler(0, 0, _currentStrafeAngle) * Vector3.up *
                                             _finalDistanceFromTarget;

                    // Execute movement
                    model.SetDestinationInput(targetPosition);
                    model.SetAiLookDirection((enemy.transform.position - transform.position).normalized);
                }
                // Fly toward enemy.
                else
                {
                    model.SetDestinationInput(enemy.transform.position);
                    model.SetAiLookDirection((enemy.transform.position - transform.position).normalized);
                }

                // Shoot enemy.
                AttackTarget(enemy);

                yield return new WaitForFixedUpdate();
            }

            // Remove strafe behaviour
            model.SetAiLookDirection(Vector3.zero, false);
        }

        private IEnumerator ChaseAttackEnemyVariance()
        {
            while (_currentState == GenericShipAiState.Chasing)
            {
                // Get current distance from target
                _finalDistanceFromTarget = Random.Range(_strafeDistanceFromTarget - _distanceFromTargetVariance,
                    _strafeDistanceFromTarget + _distanceFromTargetVariance);

                // Get current rotation sign
                _currentStrafeSign = Random.value > 0.5f ? 1 : -1;

                yield return new WaitForSeconds(_varianceInterval);
            }
        }

        private IEnumerator LockChase()
        {
            // Start chase lock
            _lockedInChase = true;

            yield return new WaitForSeconds(_chaseLockDuration);

            // Stop chase lock
            _lockedInChase = false;

        }

    }
}