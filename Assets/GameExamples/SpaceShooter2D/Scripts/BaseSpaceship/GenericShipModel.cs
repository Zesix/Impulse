/*****************************************
 * This file is part of Impulse Framework.

    Impulse Framework is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    any later version.

    Impulse Framework is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Impulse Framework.  If not, see <http://www.gnu.org/licenses/>.
*****************************************/

using UnityEngine;
using System.Collections;
using UnityEngine.Sprites;

namespace SpaceShooter2D
{
    public class GenericShipModel : MonoBehaviour
    {
        #region Properties
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
        protected float rotation = 70;
        [SerializeField]
        protected float drift = 1.0f;
        [SerializeField]
        protected float deceleration= 5.0f;
        [SerializeField]
        protected float bulletSpeed = 10f;
        
        // Used by the AI for moving along axes.
        public float movementMagnitude = 0;
        public float horzAIAxis;
        public float vertAIAxis;
        public bool inverseMovement = false;

        protected Rigidbody myRigidbody;

        // Getters and setters
        public float Acceleration
        {
            get { return acceleration; }
            private set { acceleration = value; }
        }
        public float Drift
        {
            get { return drift; }
            private set { drift = value; }
        }
        public float MaxAcceleration
        {
            get { return maxAcceleration; }
            private set { maxAcceleration = value; }
        }
        public float Rotation
        {
            get { return rotation; }
            private set { rotation = value; }
        }
        public float BulletSpeed
        {
            get { return bulletSpeed; }
            private set { bulletSpeed = value; }
        }

        public float Shields
        {
            get { return shields; }
        }
        public float Health
        {
            get { return health; }
        }

        // Control parameters
        protected Vector3 currentLookDirection = Vector3.zero;
        protected Vector3 currentMovementDirection = Vector3.zero;
        protected Vector3 currentVelocity = Vector3.zero;
        [Range(0, 1)]
        public float WeaponMomentum = 0.2f;

        bool AIControlled = false;
        Vector3 AILookDirection = Vector3.zero;

        // Player or AI Inputs
        protected Vector3 destinationInput = Vector3.zero;
        protected Vector3 keyboardDestinationInput = Vector3.zero;
        protected Vector3 rotationInput = Vector3.zero;
        protected bool useKeyboardMovement = false;
        protected bool strafeToDestination = false;
        protected bool fireInput = false;
        protected bool secondaryInput = false;
        protected Vector3 offsetFromTarget = Vector3.zero;

        // Components.
        [SerializeField]
        protected FireProjectile myShooter;
        [SerializeField]
        protected FireProjectile mySecondaryShooter;
        protected Faction myFaction;
        protected SphereDetector myDetector;
#endregion

        virtual protected void Start()
        {
            Init();
        }

        /// <summary>
        /// Execute the fixed Update
        /// </summary>
        virtual protected void FixedUpdate()
        {
            if (!AIControlled)
                ExecuteMovement();
            if (AIControlled)
                ExecuteAIMovement();
        }

        virtual protected void Update()
        {
            ExecuteWeapons();
            ExecuteMovement();
        }

        /// <summary>
        /// Sets up the player ship data.
        /// </summary>
        virtual protected void Init()
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
            destinationInput = transform.position;
        }

        /// <summary>
        /// Deals damage to the player
        /// </summary>
        /// <param name="Damage"></param>
        virtual public void DealDamage(float Damage)
        {
            // Get current shields, pre-damage
            float currentShields = shields;

            // Deal damage to the shields
            shields = Mathf.Max(shields - Damage, 0);

            // Reduce remaining damage by the pre-damage shields
            Damage = Mathf.Max(Damage - currentShields, 0);

            // Check if there is enough damage to do health damage
            if (Damage > 0)
            {
                // Deal remaining health damage
                health = Mathf.Max(health - Damage, 0);

                // To display an animation upon taking damage, call the appropriate function here.
            }

            // Debug
            Debug.Log(gameObject + " received: " + Damage + " Points of damage. Current Shields: " + shields);
        }

        /// <summary>
        /// Executes the movement.
        /// </summary>
        virtual protected void ExecuteMovement()
        {
            // Set desired destination
            Vector3 destination = destinationInput;

            // Get movement direction
            Vector3 targetLookDirection = destination - new Vector3(transform.position.x,
                                                                transform.position.y,
                                                                transform.position.z);

            // Execute look momentum
            currentLookDirection = Vector3.Lerp(currentLookDirection, targetLookDirection, drift * Time.fixedDeltaTime);

            // Mouse based movement
            if(!this.useKeyboardMovement)
            {
                // Get movement direction
                currentMovementDirection = currentLookDirection;
            }
            // Keyboard based movement
            else
            {
                // Get relative direction
                Vector3 targetKeyboardDirection = this.transform.TransformDirection(keyboardDestinationInput);

                // Get movement direction
                currentMovementDirection = Vector3.Lerp(currentMovementDirection, targetKeyboardDirection, drift * Time.fixedDeltaTime);
            }

            // Execute movement (acceleration)
            if ((this.useKeyboardMovement && keyboardDestinationInput.magnitude > 0 )||
                !this.useKeyboardMovement)
            {
                currentVelocity =
                    Vector3.ClampMagnitude(currentVelocity + currentMovementDirection*acceleration*Time.fixedDeltaTime,
                        MaxAcceleration);
            }
            // Execute movement (deceleration) (only for keyboardmovement)
            else
            {
                currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, deceleration*Time.fixedDeltaTime);

            }
            transform.Translate(currentVelocity * Time.fixedDeltaTime, Space.World);


            // Execute Rotation Towards Mouse Input (keyboard movement)
            float absoluteAngle;
            if (useKeyboardMovement)
            {
                absoluteAngle = Vector3.Angle(Vector2.up, targetLookDirection.normalized);

                // Get absolute angle sign
                Vector3 absoluteCross = Vector3.Cross(Vector2.up, targetLookDirection.normalized);
                if (absoluteCross.z < 0) absoluteAngle = -absoluteAngle;
            }
            // Execute Rotation Towards Movement Direction
            else
            {
                // Get absolute angle
                absoluteAngle = Vector3.Angle(Vector2.up, currentLookDirection.normalized);

                // Get absolute angle sign
                Vector3 absoluteCross = Vector3.Cross(Vector2.up, currentLookDirection.normalized);
                if (absoluteCross.z < 0) absoluteAngle = -absoluteAngle;
            }

            // If we are strafing, we do not rotate toward the target.
            if (!strafeToDestination)
            {
                // Rotate towards proper orientation
                Quaternion destinationAngle = Quaternion.AngleAxis(absoluteAngle, Vector3.forward);
                transform.rotation = Quaternion.Lerp(transform.rotation, destinationAngle,
                    rotation * Time.fixedDeltaTime);
            }

        }

        /// <summary>
        /// Moves the ship according to AI.
        /// </summary>
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

        /// <summary>
        /// Executes firing of weapons.
        /// </summary>
        virtual protected void ExecuteWeapons()
        {
            if (fireInput)
                ExecuteSpecificWeapon(myShooter);
            else if (secondaryInput)
                ExecuteSpecificWeapon(mySecondaryShooter);
        }

        virtual protected void ExecuteSpecificWeapon(FireProjectile shooter)
        {
            // Check if the shooter exist
            if (shooter == null || shooter.firing) return;

            // Single shoot mode
            if (shooter.fireMode == FireProjectile.FiringMode.Single)
            {
                // Fire the main weapon and send the current ship's velocity as momentum
                shooter.Fire(bulletSpeed, currentVelocity * WeaponMomentum);
            }
            // Continuous shoot mode
            else if (shooter.fireMode == FireProjectile.FiringMode.Continuous)
            {
                // Shoot as long as burst beam duration allows it
                shooter.Fire(bulletSpeed, currentVelocity * WeaponMomentum);
            }
        }

        /// <summary>
        /// Collsion Detection
        /// </summary>
        virtual protected void OnTriggerEnter(Collider coll)
        {
            Projectile incomingProjectile = coll.gameObject.GetComponent<Projectile>();
            if (incomingProjectile != null && myFaction.FactionName != incomingProjectile.Faction)
            {
                DealDamage(incomingProjectile.Damage);
                incomingProjectile.Deactivate();
            }
        }

        #region Setters and Getters
        virtual public SphereDetector GetDetector()
        {
            return myDetector;
        }

        virtual public void SetDestinationInput(Vector3 input)
        {
            destinationInput = input;
        }

        virtual public void SetKeyboardDestinationInput(Vector3 input)
        {
            keyboardDestinationInput = input;
        }

        virtual public void SetRotationInput (Vector3 input)
        {
            rotationInput = input;
        }

        virtual public void SetFireInput(bool input)
        {
            fireInput = input;
        }

        virtual public void SetSecondaryInput(bool input)
        {
            secondaryInput = input;
        }

        virtual public void SetOffsetFromTarget(Vector3 offset)
        {
            offsetFromTarget = offset;
        }

        virtual public Vector3 GetOffsetFromTarget()
        {
            return offsetFromTarget;
        }

        virtual public void SetAIControlled(bool value)
        {
            AIControlled = value;
        }

        virtual public void SetKeyboardMovement(bool value)
        {
            useKeyboardMovement = value;
        }

        virtual public void SetAILookDirection(Vector3 direction)
        {
            AILookDirection = direction;
        }

        virtual public Vector3 GetCurrentLookDirection()
        {
            return currentLookDirection;
        }

        virtual public Vector3 GetCurrentDirection()
        {
            return currentMovementDirection;
        }
        #endregion
    }
}
