using System;
using UnityEngine;

namespace SpaceShooter2D
{
    public class GenericShipModel : MonoBehaviour
    {
        // Ship properties
        protected float health;
        [SerializeField]
        protected float maxHealth = 100;
        protected float shields;
        [SerializeField]
        protected float maxShields = 100;

        [SerializeField]
        protected float acceleration = 15;
        [SerializeField]
        protected float maxAcceleration = 20;
        [SerializeField]
        protected float strafeMaxAcceleration = 2.8f;
        [SerializeField]
        protected float rotation = 70;
        [SerializeField]
        protected float chaseRotation = 140;
        [SerializeField]
        protected float drift = 1.0f;
        [SerializeField]
        protected float deceleration = 5.0f;
        [SerializeField]
        protected float bulletSpeed = 10f;

        // Used by the AI for moving along axes.
        public float MovementMagnitude;
        public float HorzAiAxis;
        public float VertAiAxis;
        public bool InverseMovement;

        protected Rigidbody myRigidbody;

        // Getters and setters
        public float Acceleration => acceleration;

        public float Drift => drift;

        public float MaxAcceleration => maxAcceleration;

        public float Rotation => rotation;

        public float BulletSpeed => bulletSpeed;

        public float Shields => shields;

        public float Health => health;

        // Control parameters
        protected Vector3 currentLookDirection = Vector3.zero;
        protected Vector3 currentMovementDirection = Vector3.zero;
        protected Vector3 currentVelocity = Vector3.zero;
        [Range(0, 1)]
        public float WeaponMomentum = 0.2f;

        private Vector3 _aiLookDirection = Vector3.zero;
        private bool _forceAiLookDirection; // used during chases

        // Player or AI Inputs
        protected Vector3 destinationInput = Vector3.zero;
        protected Vector3 keyboardDestinationInput = Vector3.zero;
        protected Vector3 rotationInput = Vector3.zero;
        protected bool useKeyboardMovement;
        protected bool strafeToDestination = false;
        protected bool fireInput;
        protected bool secondaryInput;
        protected Vector3 offsetFromTarget = Vector3.zero;

        // Components.
        [SerializeField]
        protected FireProjectile myShooter;
        [SerializeField]
        protected FireProjectile mySecondaryShooter;
        protected Faction myFaction;
        protected SphereDetector myDetector;

        protected virtual void Start()
        {
            Init();
        }

        /// <summary>
        /// Execute the fixed Update
        /// </summary>
        protected virtual void FixedUpdate()
        {
            ExecuteMovement();
        }

        protected virtual void Update()
        {
            ExecuteWeapons();
        }

        /// <summary>
        /// Sets up the player ship data.
        /// </summary>
        protected virtual void Init()
        {
            health = maxHealth;
            shields = maxShields;

            myDetector = GetComponent<SphereDetector>();
            myFaction = GetComponent<Faction>();
            if (myFaction != null)
            {
                if (myShooter != null)
                    myShooter.SetWeaponFaction(myFaction.FactionName);
                if (mySecondaryShooter != null)
                    mySecondaryShooter.SetWeaponFaction(myFaction.FactionName);
            }

            // Set the current position as the desired destination
            //SetDestinationInput(transform.position);
        }

        /// <summary>
        /// Deals damage to the player
        /// </summary>
        /// <param name="damage"></param>
        public virtual void DealDamage(float damage)
        {
            // Get current shields, pre-damage
            var currentShields = shields;

            // Deal damage to the shields
            shields = Mathf.Max(shields - damage, 0);

            // Reduce remaining damage by the pre-damage shields
            damage = Mathf.Max(damage - currentShields, 0);

            // Check if there is enough damage to do health damage
            if (damage > 0)
            {
                // Deal remaining health damage
                health = Mathf.Max(health - damage, 0);

                // To display an animation upon taking damage, call the appropriate function here.
            }

            // Debug
            Debug.Log(gameObject + " received: " + damage + " Points of damage. Current Shields: " + shields);
        }

        /// <summary>
        /// Executes the movement.
        /// </summary>
        protected virtual void ExecuteMovement()
        {
            // Set desired destination
            var destination = destinationInput;
            // Get movement direction
            var targetLookDirection = destination - new Vector3(transform.position.x,
                                                                transform.position.y,
                                                                transform.position.z);

            // Execute look momentum
            currentLookDirection = Vector3.Lerp(currentLookDirection, targetLookDirection, drift * Time.fixedDeltaTime);

            // Mouse based movement
            if (!useKeyboardMovement)
            {
                // Get movement direction
                currentMovementDirection = currentLookDirection;
            }
            // Keyboard based movement
            else
            {
                // Get relative direction
                var targetKeyboardDirection = transform.TransformDirection(keyboardDestinationInput);

                // Get movement direction
                currentMovementDirection = Vector3.Lerp(currentMovementDirection, targetKeyboardDirection, drift * Time.fixedDeltaTime);
            }

            // Execute movement (acceleration)
            if (useKeyboardMovement && keyboardDestinationInput.magnitude > 0 ||
                !useKeyboardMovement)
            {
                currentVelocity =
                    Vector3.ClampMagnitude(currentVelocity + currentMovementDirection * acceleration * Time.fixedDeltaTime,
                        !_forceAiLookDirection ? MaxAcceleration : strafeMaxAcceleration);
            }
            // Execute movement (deceleration) (only for keyboardmovement)
            else
            {
                currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, deceleration * Time.fixedDeltaTime);

            }
            transform.Translate(currentVelocity * Time.fixedDeltaTime, Space.World);


            // Execute Rotation Towards Mouse Input (keyboard movement)
            float absoluteAngle;
            if (useKeyboardMovement)
            {
                absoluteAngle = Vector3.Angle(Vector2.up, targetLookDirection.normalized);

                // Get absolute angle sign
                var absoluteCross = Vector3.Cross(Vector2.up, targetLookDirection.normalized);
                if (absoluteCross.z < 0) absoluteAngle = -absoluteAngle;

                // Rotate towards proper orientation
                transform.eulerAngles = new Vector3(0, 0, Mathf.LerpAngle(transform.eulerAngles.z, absoluteAngle, rotation * Time.fixedDeltaTime));
            }
            // Execute Rotation Towards Movement Direction
            else if (!_forceAiLookDirection)
            {
                // Get absolute angle
                absoluteAngle = Vector3.Angle(Vector2.up, currentLookDirection.normalized);

                // Get absolute angle sign
                var absoluteCross = Vector3.Cross(Vector2.up, currentLookDirection.normalized);
                if (absoluteCross.z < 0) absoluteAngle = -absoluteAngle;

                // Rotate towards proper orientation
                transform.eulerAngles = new Vector3(0, 0, Mathf.LerpAngle(transform.eulerAngles.z, absoluteAngle, rotation * Time.fixedDeltaTime));
            }
            // Execute Rotation Towards Target
            else if (_forceAiLookDirection)
            {
                // Get absolute angle
                absoluteAngle = Vector3.Angle(Vector2.up, _aiLookDirection.normalized);

                // Get absolute angle sign
                var absoluteCross = Vector3.Cross(Vector2.up, _aiLookDirection.normalized);
                if (absoluteCross.z < 0) absoluteAngle = -absoluteAngle;

                // Rotate towards proper orientation
                transform.eulerAngles = new Vector3(0, 0, Mathf.LerpAngle(transform.eulerAngles.z, absoluteAngle, chaseRotation * Time.fixedDeltaTime));
            }

        }

        /// <summary>
        /// Moves the ship according to AI.
        /// </summary>
        /*
        virtual protected void ExecuteAIMovement()
        {
            Vector3 MoveDirection = transform.up * movementMagnitude;

            // Move the ship
            transform.position += MoveDirection * acceleration * Time.fixedDeltaTime;

            // Get movement angle
            Vector3 WorldRotateDirection = new Vector3(horzAIAxis, vertAIAxis, 0);


            // Get absolute angle sign
            float absoluteAngle = Vector3.Angle(Vector2.up, WorldRotateDirection.normalized);
            Vector3 absoluteCross = Vector3.Cross(Vector2.up, WorldRotateDirection.normalized);
            if (absoluteCross.z < 0) absoluteAngle = -absoluteAngle;

            // Get absolute angle
            Quaternion destinationAngle = Quaternion.AngleAxis(inverseMovement ? absoluteAngle + 180.0f : absoluteAngle, Vector3.forward);

            // Execute rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, destinationAngle, rotation * Time.fixedDeltaTime);
        }
        */

        /// <summary>
        /// Executes firing of weapons.
        /// </summary>
        protected virtual void ExecuteWeapons()
        {
            if (fireInput)
                ExecuteSpecificWeapon(myShooter);
            else if (secondaryInput)
                ExecuteSpecificWeapon(mySecondaryShooter);
        }

        protected virtual void ExecuteSpecificWeapon(FireProjectile shooter)
        {
            // Check if the shooter exist
            if (shooter == null || shooter.firing) return;

            // Single shoot mode
            switch (shooter.fireMode)
            {
                case FireProjectile.FiringMode.Single:
                    // Fire the main weapon and send the current ship's velocity as momentum
                    shooter.Fire(bulletSpeed, currentVelocity * WeaponMomentum);
                    break;
                case FireProjectile.FiringMode.Continuous:
                    // Shoot as long as burst beam duration allows it
                    shooter.Fire(bulletSpeed, currentVelocity * WeaponMomentum);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Collsion Detection
        /// </summary>
        protected virtual void OnTriggerEnter(Collider coll)
        {
            var incomingProjectile = coll.gameObject.GetComponent<Projectile>();
            if (incomingProjectile != null && myFaction.FactionName != incomingProjectile.Faction)
            {
                DealDamage(incomingProjectile.Damage);
                incomingProjectile.Deactivate();
            }
        }
        public virtual SphereDetector GetDetector()
        {
            return myDetector;
        }

        public virtual void SetDestinationInput(Vector3 input)
        {
            destinationInput = input;
        }

        public virtual void SetKeyboardDestinationInput(Vector3 input)
        {
            keyboardDestinationInput = input;
        }

        public virtual void SetRotationInput(Vector3 input)
        {
            rotationInput = input;
        }

        public virtual void SetFireInput(bool input)
        {
            fireInput = input;
        }

        public virtual void SetSecondaryInput(bool input)
        {
            secondaryInput = input;
        }

        public virtual void SetOffsetFromTarget(Vector3 offset)
        {
            offsetFromTarget = offset;
        }

        public virtual Vector3 GetOffsetFromTarget()
        {
            return offsetFromTarget;
        }

        public virtual void SetKeyboardMovement(bool value)
        {
            useKeyboardMovement = value;
        }

        public virtual void SetAiLookDirection(Vector3 direction, bool enable = true)
        {
            _forceAiLookDirection = enable;
            _aiLookDirection = direction;
        }

        public virtual Vector3 GetCurrentLookDirection()
        {
            return currentLookDirection;
        }

        public virtual Vector3 GetCurrentDirection()
        {
            return currentMovementDirection;
        }
    }
}
