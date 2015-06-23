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

namespace SpaceShooter2D
{

    public class GenericShipView : MonoBehaviour
    {
        // Player Components
        [SerializeField]
        protected FireProjectile myShooter;
        [SerializeField]
        protected FireProjectile mySecondaryShooter;
        protected GenericShipModel myModel;
        protected Animator myAnimator;
        protected Faction myFaction;
        protected Detector2D myDetector;
        protected AudioSource myAudio;

        // Animation parameters
        [Range(0, 1)]
        public float MaxVolume = 0.5f;
        [Range(0, 180)]
        public float forwardAngleThreshold = 10.0f;

        // Player or AI Inputs
        protected Vector2 destinationInput = Vector2.zero;
        protected bool fireInput = false;
        protected bool secondaryInput = false;
        public bool forcedPosition = false;
        protected Vector2 offsetFromTarget = Vector2.zero;

        // Control parameters
        [Range(0, 10)]
        public float forcedPositionTime = 3.0f;
        protected Vector2 currentDirection = Vector2.zero;
        protected Vector2 currentVelocity = Vector2.zero;
        [Range(0, 1)]
        public float WeaponMomentum = 0.2f;

        virtual protected void Start()
        {
            myDetector = GetComponent<Detector2D>();
            myModel = GetComponent<GenericShipModel>();
            myAnimator = GetComponent<Animator>();
            if (myAnimator == null)
            {
                Debug.LogError("No animator attached to PlayerShip!");
            }
            myFaction = GetComponent<Faction>();
            if (myFaction != null)
            {
                if (myShooter != null)
                    myShooter.SetWeaponFaction(myFaction.FactionName);
                if (mySecondaryShooter != null)
                    mySecondaryShooter.SetWeaponFaction(myFaction.FactionName);
            }
            myAudio = GetComponent<AudioSource>();

            // Set the current position as the desired destination
            destinationInput = transform.position;
        }

        /// <summary>
        /// Executes the main Update
        /// </summary>
        virtual protected void Update()
        {
            ExecuteWeapons();
            ExecuteAnimations();
            ExecuteThrustersFX();
        }

        /// <summary>
        /// Execute the fixed Update
        /// </summary>
        virtual protected void FixedUpdate()
        {
            ExecuteMovement();
        }

        virtual protected void ExecuteThrustersFX()
        {
            if (myAudio == null) return;
            float currentVolume = Mathf.Lerp(0, MaxVolume, currentDirection.magnitude);
            if (!myAudio.isPlaying)
            {
                myAudio.loop = true;
                myAudio.Play();
            }
            myAudio.volume = currentVolume;
        }

        /// <summary>
        /// Set the animations
        /// </summary>
        virtual protected void ExecuteAnimations()
        {
            // Get relative angle
            Vector2 up2D = new Vector2(myModel.transform.up.x, myModel.transform.up.y);
            float relativeAngle = Vector2.Angle(up2D, currentDirection.normalized);

            // Get relative sign
            Vector3 relativeCross = Vector3.Cross(myModel.transform.up, currentDirection.normalized);
            if (relativeCross.z < 0) relativeAngle = -relativeAngle;


            // Set movement animations
            SetForward(relativeAngle > -forwardAngleThreshold && relativeAngle < forwardAngleThreshold);
            SetTurnLeft(relativeAngle >= forwardAngleThreshold);
            SetTurnRight(relativeAngle <= -forwardAngleThreshold);

        }

        /// <summary>
        /// Executes the movement.
        /// </summary>
        virtual protected void ExecuteMovement()
        {
            // Set desired destination
            Vector2 destination = destinationInput;

            // Set desired acceleration
            float MaxAcceleration = myModel.MaxAcceleration;

            // Get movement direction
            Vector2 targetDirection = destination - new Vector2(myModel.transform.position.x,
                                                                myModel.transform.position.y);

            // Execute movement momentum
            currentDirection = Vector2.Lerp(currentDirection, targetDirection, myModel.Drift * Time.fixedDeltaTime);

            // Execute movement
            currentVelocity = Vector2.ClampMagnitude(currentVelocity + currentDirection * myModel.Acceleration * Time.fixedDeltaTime,
                                                     MaxAcceleration);
            if (!forcedPosition)
                myModel.transform.Translate(currentVelocity * Time.fixedDeltaTime, Space.World);

            // Get absolute angle
            float absoluteAngle = Vector2.Angle(Vector2.up, currentDirection.normalized);

            // Get absolute angle sign
            Vector3 absoluteCross = Vector3.Cross(Vector2.up, currentDirection.normalized);
            if (absoluteCross.z < 0) absoluteAngle = -absoluteAngle;

            // Rotate towards proper orientation
            Quaternion destinationAngle = Quaternion.AngleAxis(absoluteAngle, Vector3.forward);
            myModel.transform.rotation = Quaternion.Lerp(myModel.transform.rotation, destinationAngle, myModel.Rotation * Time.fixedDeltaTime);

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
                shooter.Fire(myModel.BulletSpeed, currentVelocity * WeaponMomentum);
            }
            // Continuous shoot mode
            else if (shooter.fireMode == FireProjectile.FiringMode.Continuous)
            {
                // Shoot as long as burst beam duration allows it
                shooter.Fire(myModel.BulletSpeed, currentVelocity * WeaponMomentum);
            }
        }

        /// <summary>
        /// Collsion Detection
        /// </summary>
        virtual protected void OnTriggerEnter2D(Collider2D coll)
        {
            Projectile incomingProjectile = coll.gameObject.GetComponent<Projectile>();
            if (incomingProjectile != null && myFaction.FactionName != incomingProjectile.Faction)
            {
                myModel.DealDamage(incomingProjectile.Damage);
                incomingProjectile.Deactivate();
            }
        }

        #region Setters and Getters
        /// <summary>
        /// Set animator values for playing animations.
        /// </summary>
        /// <param name="value">True = play animation</param>
        virtual protected void SetForward(bool value)
        {
            myAnimator.SetBool("Forward", value);
        }

        virtual protected void SetTurnLeft(bool value)
        {
            myAnimator.SetBool("TurnLeft", value);
        }

        virtual protected void SetTurnRight(bool value)
        {
            myAnimator.SetBool("TurnRight", value);
        }

        virtual protected void SetReverse(bool value)
        {
            myAnimator.SetBool("Reverse", value);
        }

        virtual protected void TriggerReverse()
        {
            myAnimator.SetTrigger("ReverseTrigger");
        }

        virtual public Detector2D getDetector()
        {
            return myDetector;
        }

        virtual public bool isAlive()
        {
            return myModel.Health > 0;
        }

        virtual public Vector2 GetCurrentDirection()
        {
            return currentDirection;
        }

        virtual public void setDestinationInput(Vector2 input)
        {
            destinationInput = input;
        }

        virtual public void setTemporaryForcedPosition()
        {
            forcedPosition = true;
            StopCoroutine("removeForcedPositionDelayed");
            StartCoroutine("removeForcedPositionDelayed");
        }

        virtual public void removeTemporaryForcedPosition()
        {
            forcedPosition = false;
            StopCoroutine("removeForcedPositionDelayed");
        }

        IEnumerator removeForcedPositionDelayed()
        {
            yield return new WaitForSeconds(forcedPositionTime * Random.value);
            forcedPosition = false;
        }

        virtual public void setForcedPosition(Vector2 input)
        {
            transform.position = input;
        }

        virtual public void removeForcedPosition()
        {
            forcedPosition = false;
            StopCoroutine("removeForcedPositionDelayed");
        }

        virtual public void setFireInput(bool input)
        {
            fireInput = input;
        }

        virtual public void setSecondaryInput(bool input)
        {
            secondaryInput = input;
        }

        virtual public void setOffsetFromTarget(Vector2 offset)
        {
            offsetFromTarget = offset;
        }

        virtual public Vector2 getOffsetFromTarget()
        {
            return offsetFromTarget;
        }
        #endregion
    }
}