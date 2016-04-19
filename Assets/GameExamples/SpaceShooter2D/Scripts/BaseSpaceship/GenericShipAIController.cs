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
    [RequireComponent(typeof(GenericShipModel))]
    [RequireComponent(typeof(GenericShipView))]
    [RequireComponent(typeof(BaseAIController))]
    [RequireComponent(typeof(Detector))]
    public class GenericShipAIController : MonoBehaviour
    {
        // Our ship model.
        protected GenericShipModel myShipModel;

        // Our ship view.
        protected GenericShipView myShipView;

        // Our AI controller.
        protected BaseAIController myController;

        // Our detector.
        [SerializeField]
        Detector myDetector;

        // Our AI behavior update rate. Increase this if you want the AI to stick to a behavior longer.
        [SerializeField]
        float behaviorChangeRate = 1.0f;

        // Use this for initialization
        protected virtual void Start()
        {
            myShipModel = GetComponent<GenericShipModel>();
            myShipView = GetComponent<GenericShipView>();
            myDetector = GetComponent<Detector>();
            myController = GetComponent<BaseAIController>();

            if (myDetector == null)
            {
                Debug.LogError("No Detector attached to " + gameObject + "!");
            }

            if (myController == null)
            {
                Debug.LogError("No AI Controller attached to " + gameObject + "!");
            }

            myController.SetAIControl(true);
            myShipModel.SetAIControlled(true);

            // Begin custom update loop for AI.
            InvokeRepeating("UpdateAIController", 0.0f, behaviorChangeRate);
        }

        // Updates the AI controller.
        private void UpdateAIController()
        {
            // Set AI controller parameters based on model parameters.
            myController.patrolSpeed = myShipModel.Acceleration;
            myController.patrolTurnSpeed = myShipModel.Rotation;
            myController.moveSpeed = myShipModel.Acceleration;
            myController.wallAvoidDistance = myDetector.AvoidRange;
            myController.waypointDistance = myDetector.AvoidRange;
            myController.minChaseDistance = myDetector.AvoidRange;
            myController.maxChaseDistance = myDetector.AttackRange;

            GameObject closestEnemy = myDetector.ClosestEnemy();

            // If an enemy is close, set it as our chase target.
            if (closestEnemy != null)
            {
                myController.SetChaseTarget(closestEnemy.transform);
                AttackTarget(closestEnemy);
                myShipModel.SetAILookDirection(closestEnemy.transform.position);
            }

            myShipModel.horzAIAxis = myController.GetHorizontal();
            myShipModel.vertAIAxis = myController.GetVertical();
            myShipModel.inverseMovement = myController.GetInverseMovement();
            myShipModel.movementMagnitude = myController.GetMagnitude();

        }

        void AttackTarget(GameObject enemy)
        {
            // While facing the enemy, shoot.
            if (isFacingTarget(enemy))
            {
                myShipModel.SetFireInput(true);
            }
            if (!isFacingTarget(enemy))
            {
                myShipModel.SetFireInput(false);
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