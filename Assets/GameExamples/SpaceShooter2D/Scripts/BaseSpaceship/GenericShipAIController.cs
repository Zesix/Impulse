using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
        #region Variables
        // Our ship model.
        protected GenericShipModel model;

        // Our ship view.
        protected GenericShipView view;

        // Collider
        SphereCollider collider;

        // Our detector.
        [SerializeField]
        SphereDetector myDetector;

        // Our AI behavior update rate. Increase this if you want the AI to stick to a behavior longer.
        [SerializeField]
        float behaviorChangeRate = 1.0f;

        // Our waypoint manager. If this is not assigned, then the ship will automatically patrol in a square formation.
        [SerializeField]
        WaypointPathManager waypointManager;

        // Forward distance threshold. Used when generating a random point in front of the ship during square formation patrolling.
        [SerializeField]
        [Range(1,100)]
        float forwardPatrolDistance = 6f;

        // Random forward point. This is the travel destination that is randomly generated during square formation patrolling.
        Vector3 forwardSquarePatrolPoint;

        // Chase parameters
        [SerializeField]
        float chaseLockDuration = 3.0f;
        [SerializeField]
        bool strafeAroundTarget = true;
        [SerializeField]
        [Range(1, 100)]
        float strafeDistanceFromTarget = 6f;
        [SerializeField]
        [Range(1, 100)]
        float distanceFromTargetVariance = 2.0f;
        [SerializeField]
        [Range(1, 100)]
        float varianceInterval = 2.0f;
        float finalDistanceFromTarget = 0;
        float currentStrafeAngle = 0;
        float currentStrafeSign = 1;
        bool lockedInChase = false;

        // States.
        [SerializeField]
        GenericShipAIState startState;

        [SerializeField]
        GenericShipAIState currentState;

        enum GenericShipAIState
        {
            Idle,
            WaypointPatrolling,
            SquarePatrolling,
            Chasing
        }
        #endregion

        // Use this for initialization
        protected virtual void Start()
        {
            model = GetComponent<GenericShipModel>();
            view = GetComponent<GenericShipView>();
            myDetector = GetComponent<SphereDetector>();

            if (myDetector == null)
            {
                Debug.LogError("No Detector attached to " + gameObject + "!");
            }

            model.SetAIControlled(true);

            // Check if initial state is specified. If not, and there is no waypoint manager, then begin in square patrol mode.
            // Otherwise, begin in waypoint patrol mode.
            if (startState == GenericShipAIState.Idle && waypointManager == null)
                currentState = GenericShipAIState.SquarePatrolling;
            if (startState == GenericShipAIState.Idle && waypointManager != null)
                currentState = GenericShipAIState.WaypointPatrolling;

            // If we are patrolling and have no waypoints, then begin patrolling in square formation.
            if (currentState == GenericShipAIState.SquarePatrolling)
                StartCoroutine(SquareFormationPatrol());

            // Begin custom update loop for AI.
            InvokeRepeating("UpdateAIController", 0.0f, behaviorChangeRate);
        }

        // Updates the AI controller.
        private void UpdateAIController()
        {
            if (currentState == GenericShipAIState.SquarePatrolling)
            {
                // If we are in square formation patrol and have reached our current forward point, then turn left and generate a new forward point.
                if (transform.position == forwardSquarePatrolPoint)
                {
                    // Turn some number of degrees left.
                    transform.Rotate(new Vector3(0,0,Random.Range(0, 90)));

                    StartCoroutine(SquareFormationPatrol());
                }
            }

            // Check if there is an enemy within range.
            GameObject closestEnemy = myDetector.ClosestEnemy();

            // If we have an enemy, then chase and attack it. Otherwise, resume patrolling.
            // Don't execute this if we're in lock chase 
            if (closestEnemy != null && !lockedInChase)
            {
                currentState = GenericShipAIState.Chasing;

                StartCoroutine(LockChase());
                StartCoroutine(ChaseAttackEnemyVariance());
                StartCoroutine(ChaseAttackEnemy(closestEnemy));
            }
            else if(closestEnemy == null)
            {
                if (waypointManager != null)
                    currentState = GenericShipAIState.WaypointPatrolling;
                currentState = GenericShipAIState.SquarePatrolling;
            }

        }

        void AttackTarget(GameObject enemy)
        {
            // While facing the enemy, shoot.
            if (isFacingTarget(enemy))
            {
                model.SetFireInput(true);
            }
            if (!isFacingTarget(enemy))
            {
                model.SetFireInput(false);
            }
        }

        /// <summary>
        /// Returns if the current gameobject is facing the target gameobject
        /// </summary>
        /// <returns></returns>
        bool isFacingTarget(GameObject target)
        {
            float AngleThreshold = 30.0f;
            // Note: this is for 2D only (XY plane). If you're going to use 3D, replace transform.up with transform.forward
            return AngleThreshold >= Vector3.Angle((target.transform.position - transform.position).normalized, transform.up);
        }

        Vector3 GetRandomForwardPosition()
        {
            Vector3 randomForwardPosition = transform.position + (Vector3.up * Random.Range(1, forwardPatrolDistance));
            return randomForwardPosition;
        }
        
        IEnumerator SquareFormationPatrol()
        {
            if (currentState == GenericShipAIState.SquarePatrolling)
            {
                // Get random forward position and set it as destination.
                forwardSquarePatrolPoint = GetRandomForwardPosition();
                model.SetDestinationInput(forwardSquarePatrolPoint);
                while (transform.position != forwardSquarePatrolPoint)
                {
                    yield return new WaitForSeconds(behaviorChangeRate);
                }
            }
        }

        IEnumerator ChaseAttackEnemy(GameObject enemy)
        {
            while (currentState == GenericShipAIState.Chasing)
            {
                // Strafe toward enemy.
                if (strafeAroundTarget)
                {
                    // Get current target angle
                    currentStrafeAngle = (currentStrafeAngle + model.MaxAcceleration*currentStrafeSign*Time.fixedDeltaTime)%360.0f;

                    // Get position around target
                    Vector3 targetPosition = enemy.transform.position +
                                             Quaternion.Euler(0, 0, currentStrafeAngle)*Vector3.up*
                                             finalDistanceFromTarget;

                    // Execute movement
                    model.SetDestinationInput(targetPosition);
                    model.SetAILookDirection((enemy.transform.position - transform.position).normalized);
                }
                // Fly toward enemy.
                else
                {
                    model.SetDestinationInput(enemy.transform.position);
                    model.SetAILookDirection((enemy.transform.position - transform.position).normalized);
                }

                // Shoot enemy.
                AttackTarget(enemy);

                yield return new WaitForFixedUpdate();
            }

            // Remove strafe behaviour
            model.SetAILookDirection(Vector3.zero,false);
        }

        IEnumerator ChaseAttackEnemyVariance()
        {
            while (currentState == GenericShipAIState.Chasing)
            {
                // Get current distance from target
                finalDistanceFromTarget = Random.Range(strafeDistanceFromTarget - distanceFromTargetVariance,
                    strafeDistanceFromTarget + distanceFromTargetVariance);

                // Get current rotation sign
                currentStrafeSign = Random.value > 0.5f ? 1 : -1;

                yield return new WaitForSeconds(varianceInterval);
            }
        }

        IEnumerator LockChase()
        {
            // Start chase lock
            lockedInChase = true;

            yield return new WaitForSeconds(chaseLockDuration);

            // Stop chase lock
            lockedInChase = false;

        }

    }
}