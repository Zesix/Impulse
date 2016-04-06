using UnityEngine;
using System.Collections;

namespace IsometricShooter3D
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyModel : CharacterModel
    {

        #region Properties
        // Our material.
        Material skinMaterial;

        // Colors.
        [SerializeField]
        Color normalColor;
        [SerializeField]
        Color attackingColor = Color.red;

        // The target we are chasing.
        Transform target;
        CharacterModel targetModel;

        // Collision radii of target and our own mesh.
        // Used in pathfinding to prevent 'going inside' the target.
        float myCollisionRadius;
        float targetCollisionRadius;

        // Used for pathfinding.
        NavMeshAgent pathfinder;

        // How often the enemy updates the player's position during pathfinding.
        [SerializeField]
        float refreshRate = 0.25f;
        public float RefreshRate
        {
            get { return refreshRate; }
            set { refreshRate = value; }
        }

        // The distance in which this character will attack the target.
        [SerializeField]
        float attackDistanceThreshold = 0.5f;

        // Attack damage.
        [SerializeField]
        float attackDamage = 1f;
        public float AttackDamage
        {
            get { return attackDamage; }
        }

        // Attack speed.
        [SerializeField]
        float attackSpeed = 3f;
        public float AttackSpeed
        {
            get { return attackSpeed; }
        }

        // Time between attacks.
        [SerializeField]
        float timeBetweenAttacks = 1f;

        // Time until next attack (if within distance).
        [SerializeField]
        float nextAttackTime;

        // States this object can be in. Used to prevent an error in which the object tries to attack in the middle of pathfinding.
        enum EnemyState { Idle, Chasing, Attacking };

        // Current state.
        [SerializeField]
        EnemyState currentState;
        #endregion

        void OnEnable()
        {
            this.AddObserver(OnGameOver, GameplayState.GameOverNotification);
        }

        void OnDisable()
        {
            this.RemoveObserver(OnGameOver, GameplayState.GameOverNotification);
        }

        protected override void Start()
        {
            base.Start();
            pathfinder = GetComponent<NavMeshAgent>();
            skinMaterial = GetComponent<Renderer>().material;
            normalColor = skinMaterial.color;

            myCollisionRadius = GetComponent<CapsuleCollider>().radius;
            targetCollisionRadius = GetComponent<CapsuleCollider>().radius;

            // If we have a target, set target and begin chasing.
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                currentState = EnemyState.Chasing;
                target = GameObject.FindGameObjectWithTag("Player").transform;
                targetModel = target.GetComponent<CharacterModel>();
            }

            // Begin pathfinding.
            StartCoroutine(UpdatePath());
        }

        void Update()
        {
            // Check if in range of attack and sufficient time has passed since last attack. If so, then attack.
            if (Time.time > nextAttackTime && target != null)
            {
                // Compare square distance to target with attack distance for more performant calculation.
                float sqrDistToTarget = (target.position - transform.position).sqrMagnitude;
                // We add the radii of our mesh and the target mesh so distance is compared from the edges of each mesh instead of their centers.
                if (sqrDistToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2))
                {
                    nextAttackTime = Time.time + timeBetweenAttacks;
                    StartCoroutine(Attack());
                }
            }
        }

        void OnGameOver(object sender, object args)
        {
            currentState = EnemyState.Idle;
        }

        IEnumerator UpdatePath()
        {
            while (target != null && target != null)
            {
                if (currentState == EnemyState.Chasing)
                {
                    Vector3 directionToTarget = (target.position - transform.position).normalized;
                    // Set pathfinding position so that we don't go 'inside' the target. We also add half the attackDistance to prevent 'crowding' the target.
                    Vector3 targetPosition = target.position - directionToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold / 2);

                    if (!dead)
                        pathfinder.SetDestination(targetPosition);
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }

        IEnumerator Attack()
        {
            // Have we dealt damage yet?
            bool hasAppliedDamage = false;

            // Set current state to attacking.
            currentState = EnemyState.Attacking;

            // Disable moving toward target while attacking.
            pathfinder.enabled = false;

            Vector3 originalPosition = transform.position;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            // Set attack position so that we don't go too far 'inside' the target.
            Vector3 attackPosition = target.position - directionToTarget * (myCollisionRadius);
            float attackCompletePercent = 0f;

            // Update skin color.
            skinMaterial.color = attackingColor;

            while (attackCompletePercent <= 1)
            {
                // Deal damage when the enemy hits the target, not at start of attack animation.
                if (attackCompletePercent >= 0.5f && !hasAppliedDamage)
                {
                    hasAppliedDamage = true;
                    targetModel.TakeDamage(attackDamage);
                }

                attackCompletePercent += Time.deltaTime * attackSpeed;

                // Since we need to interpolate from 0 to 1 and then back to zero,
                // we use the parabola equation y = 4(-x^2 + x).
                float interpolation = (-Mathf.Pow(attackCompletePercent, 2) + attackCompletePercent) * 4;

                transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);

                yield return null;
            }

            // Update skin color.
            skinMaterial.color = normalColor;
            
            // Reenable pathfinder and resume chasing.
            currentState = EnemyState.Chasing;
            pathfinder.enabled = true;
        }
    }
}