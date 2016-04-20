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

    [RequireComponent(typeof(GenericShipModel))]
    public class GenericShipPlayerController : BaseInputController
    {
        // Options
        public bool UseKeyboard = true;

        // Our ship
        protected GenericShipModel myShip;

        // Our input
        [SerializeField]
        Vector2 mouseInput;
        [SerializeField]
        Vector2 keyboardInput;
        [SerializeField]
        Vector3 mouse3DInput;
        [SerializeField]
        float CameraDistance = 10.0f;

        protected virtual void Start()
        {
            myShip = GetComponent<GenericShipModel>();
        }

        public override void CheckInput()
        {
            // Get mouse position
            mouse3DInput = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                                                                            CameraDistance));
            mouseInput = new Vector2(mouse3DInput.x, mouse3DInput.y);

            // Get keyboard input
            keyboardInput = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));

            // get fire / action buttons
            Fire1 = Input.GetButton("Fire1");
            Fire2 = Input.GetButton("Fire2");
        }

        protected virtual void SendInput()
        {
            // Set keyboard mode
            myShip.SetKeyboardMovement(UseKeyboard);

            // Enable if you want the ship to fly toward the mouse cursor position
            myShip.SetDestinationInput(mouseInput);
            myShip.SetKeyboardDestinationInput(keyboardInput);

            myShip.SetRotationInput(mouseInput);
            myShip.SetFireInput(Fire1);
            myShip.SetSecondaryInput(Fire2);
        }

        private void Update()
        {
            CheckInput();
            SendInput();
        }

    }
}