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

        Vector3 ourPosition;

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
            InvokeRepeating("HandleClosestObject", 0.0f, behaviorChangeRate);
        }

        // Check if enemies are detected and handle accordingly.
        private void HandleClosestObject()
        {
            ourPosition = transform.position;
            GameObject closestEnemy = ourDetector.ClosestEnemy();
            GameObject closestAlly = ourDetector.ClosestAlly();

            // Handle case if there is a close ally and a close enemy.
            if (closestAlly != null && closestEnemy != null)
            {
                float closestAllyDistance = Vector3.Distance(transform.position, closestAlly.transform.position);
                float closestEnemyDistance = Vector3.Distance(transform.position, closestEnemy.transform.position);

                if (closestEnemyDistance <= closestAllyDistance)
                {
                    if (closestEnemyDistance > ourDetector.AvoidRange)
                    {
                        myShip.setStrafeToDestination(false);
                        myShip.setDestinationInput(closestEnemy.transform.position);
                        AttackPlayer(closestEnemy);
                    }

                    if (closestEnemyDistance < ourDetector.AvoidRange)
                    {
                        myShip.setDestinationInput(transform.position * -1);
                    }
                }

                if (closestEnemyDistance > closestAllyDistance)
                {
                    float distanceToClosestAlly = Vector3.Distance(transform.position, closestAlly.transform.position);

                    if (distanceToClosestAlly < ourDetector.AvoidRange)
                    {
                        AvoidNearbyAllies();
                    }
                }
            }

            // Handle case if there is a close enemy but no close ally.
            if (closestAlly == null && closestEnemy != null)
            {
                float closestEnemyDistance = Vector3.Distance(transform.position, closestEnemy.transform.position);


                if (closestEnemyDistance > ourDetector.AvoidRange)
                {
                    myShip.setStrafeToDestination(false);
                    myShip.setDestinationInput(closestEnemy.transform.position);
                    AttackPlayer(closestEnemy);
                }

                if (closestEnemyDistance < ourDetector.AvoidRange)
                {
                    myShip.setDestinationInput(transform.position * -1);
                }
            }

            // Handle case if there is a close ally but no close enemy.
            if (closestAlly != null && closestEnemy == null)
            {
                float distanceToClosestAlly = Vector3.Distance(transform.position, closestAlly.transform.position);

                if (distanceToClosestAlly < ourDetector.AvoidRange)
                {
                    AvoidNearbyAllies();
                }
            }
        }

        void AttackPlayer(GameObject enemy)
        {
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

        void AvoidNearbyAllies()
        {
            Vector3 ourPosition = transform.position;

            // Get away position.

            // This one sets the destination 180 degrees.
            // Vector3 awayPosition = ourPosition * -1;

            // This one turns the destination at specified angled offsets, chosen randomly.
            // float[] angleOffsets = { -90, 180, 90 };
            // int myRandomIndex = Random.Range(0, angleOffsets.Length);
            // Vector3 awayPosition = new Vector3(ourPosition.x + angleOffsets[myRandomIndex], ourPosition.y + angleOffsets[myRandomIndex], 0);

            // This one sets a random angle.
            Vector3 awayPosition = new Vector3(ourPosition.x + Random.Range(-45, 45), ourPosition.y + Random.Range(-45, 45), 0);

            myShip.setDestinationInput(awayPosition);
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