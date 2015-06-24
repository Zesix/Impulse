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
using System.Collections.Generic;
using System.Linq;

namespace SpaceShooter2D
{

    /// <summary>
    /// A generic AI controller that will make a ship seek out and attack members of an enemy faction
    /// that are within detection range.
    /// </summary>
    [RequireComponent(typeof(GenericShipView))]
    public class GenericShipAIController : MonoBehaviour
    {
        // Our ship.
        protected GenericShipView myShip;

        // Our detector.
        [SerializeField]
        Detector ourDetector;

        // Our AI behavior update rate. Increase this if you want the AI to stick to a behavior longer.
        [SerializeField]
        float behaviorChangeRate = 1.0f;

        // Use this for initialization
        protected virtual void Start()
        {
            myShip = GetComponent<GenericShipView>();
            ourDetector = GetComponent<Detector>();
            if (ourDetector == null)
            {
                Debug.LogError("No Detector attached to " + gameObject + "!");
            }

            // Begin custom update loop for AI.
            InvokeRepeating("HandleClosestEnemy", 0.0f, behaviorChangeRate);
        }

        // Check if enemies are detected and handle accordingly.
        private void HandleClosestEnemy()
        {
            GameObject closestEnemy = ourDetector.ClosestEnemy();

            if (closestEnemy != null)
            {
                AttackPlayer(closestEnemy);
            }
        }

        void AttackPlayer(GameObject enemy)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            Vector2 ourPosition = transform.position;
            Vector2 enemyPosition = enemy.transform.position;

            // If the closest enemy is outside avoid range, fly toward it and attack.
            if (distance > ourDetector.AvoidRange)
            {
                myShip.setDestinationInput(enemyPosition);
            }

            // Otherwise, fly away from it.
            if (distance <= ourDetector.AvoidRange)
            {

                // Get away position.

                // This one turns the enemy away 180 degrees.
                // Vector2 awayPosition = ourPosition + (ourPosition - enemyPosition).normalized;

                // This one turns the enemy a bit to the side, chosen randomly from specified angles.
                float[] angleOffsets = { 45, 60 };
                int myRandomIndex = Random.Range(0, angleOffsets.Length);
                Vector2 awayPosition = new Vector2(ourPosition.x + angleOffsets[myRandomIndex], ourPosition.y + angleOffsets[myRandomIndex]);

                myShip.setDestinationInput(awayPosition);
            }

            // While facing the enemy, shoot.
            if (isFacingTarget(enemy))
            {
                myShip.setFireInput(true);
            }
            if (!isFacingTarget(enemy))
            {
                myShip.setFireInput(false);
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
    }
}