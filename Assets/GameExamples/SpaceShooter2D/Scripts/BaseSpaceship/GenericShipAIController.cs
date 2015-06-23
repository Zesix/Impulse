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
        Detector2D ourDetector;

        // Use this for initialization
        protected virtual void Start()
        {
            myShip = GetComponent<GenericShipView>();
            if (ourDetector == null)
            {
                ourDetector = GetComponent<Detector2D>();
            }

            // Begin custom update loop for AI.
            InvokeRepeating("HandleClosestEnemy", 0.0f, ourDetector.DetectionRate);
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
                Debug.Log("Attacking player.");

                // Shoot once we are within attack range.
                if (distance <= ourDetector.AttackRange)
                {
                    myShip.setFireInput(true);
                }
            }

            // Otherwise, fly away from it.
            if (distance <= ourDetector.AvoidRange)
            {
                Debug.Log("Avoiding player.");
                // Get away position
                Vector2 awayPosition = ourPosition +
                               (ourPosition - enemyPosition).normalized;

                myShip.setDestinationInput(awayPosition);
            }
        }
    }
}