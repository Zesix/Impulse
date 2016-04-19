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
        protected GenericShipModel myModel;
        protected Animator myAnimator;
        protected AudioSource myAudio;

        // Animation parameters
        [Range(0, 1)]
        public float MaxVolume = 0.5f;
        [Range(0, 180)]
        public float forwardAngleThreshold = 10.0f;

        virtual protected void Start()
        {
            myModel = GetComponent<GenericShipModel>();
            myAnimator = GetComponent<Animator>();
            if (myAnimator == null)
            {
                Debug.LogError("No animator attached to PlayerShip!");
            }
            myAudio = GetComponent<AudioSource>();
        }

        /// <summary>
        /// Executes the main Update
        /// </summary>
        virtual protected void Update()
        {
            ExecuteAnimations();
            ExecuteThrustersFX();
        }

        virtual protected void ExecuteThrustersFX()
        {
            if (myAudio == null) return;
            float currentVolume = Mathf.Lerp(0, MaxVolume, myModel.GetCurrentDirection().magnitude);
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
            Vector3 up2D = new Vector3(myModel.transform.up.x, myModel.transform.up.y, 0);
            float relativeAngle = Vector3.Angle(up2D, myModel.GetCurrentDirection().normalized);

            // Get relative sign
            Vector3 relativeCross = Vector3.Cross(myModel.transform.up, myModel.GetCurrentDirection().normalized);
            if (relativeCross.z < 0) relativeAngle = -relativeAngle;


            // Set movement animations
            SetForward(relativeAngle > -forwardAngleThreshold && relativeAngle < forwardAngleThreshold);
            SetTurnLeft(relativeAngle >= forwardAngleThreshold);
            SetTurnRight(relativeAngle <= -forwardAngleThreshold);

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

        virtual public bool isAlive()
        {
            return myModel.Health > 0;
        }

        virtual public void setForcedPosition(Vector3 input)
        {
            transform.position = input;
        }
        #endregion
    }
}