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
        // Model properties
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

        virtual protected void Start()
        {
            Init();
        }

        /// <summary>
        /// Sets up the player ship data.
        /// </summary>
        virtual protected void Init()
        {
            health = maxHealth;
            shields = maxShields;
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

    }
}
