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
using UnityStandardAssets.CrossPlatformInput;

public class MouseInputController : BaseInputController
{
    // Reference to the model. Replace with your own specific model during implementation.
    BaseObjectModel myModel;

    // 2D mouse position on screen.
    [SerializeField]
    Vector2 mouseInput;

    // Mouse position in world space, from screen to world point.
    Vector3 mouse3DInput;

    [SerializeField]
    float cameraDistance = 10.0f;

    // Where the mouse has clicked, in 3D coordinates.
    [SerializeField]
    Vector3 clickPosition;

    void Awake()
    {
        myModel = GetComponent<BaseObjectModel>();

        if (myModel == null)
            Debug.LogError(gameObject.name + " is missing a View!");
        
        // By default, set our click position to our current position.
        clickPosition = transform.position;
    }

    public override void CheckInput()
    {
        // Get mouse position
        mouse3DInput = Camera.main.ScreenToWorldPoint(new Vector3(CrossPlatformInputManager.mousePosition.x, CrossPlatformInputManager.mousePosition.y,
                                                                        cameraDistance));
        mouseInput = new Vector2(mouse3DInput.x, mouse3DInput.y);

        // get fire / action buttons
        Fire1 = CrossPlatformInputManager.GetButton("Fire1");

        // If we have clicked, get the world space position and send it to our view.
        // If you want to use something other than the fire button for movement, replace 'Fire1'.
        if (Fire1)
        {
            SetClickPosition();
            SendInput();
        }
    }

    void Update()
    {
        CheckInput();
    }

    /// <summary>
    /// Sets the click position.
    /// </summary>
    void SetClickPosition()
    {
        Plane clickPlane = new Plane(Vector3.up, transform.position);
        Ray clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float rayPoint = 0f;

        if (clickPlane.Raycast(clickRay, out rayPoint))
            clickPosition = clickRay.GetPoint(rayPoint);
    }

    protected virtual void SendInput()
    {
        myModel.setDestinationInput(clickPosition);
        myModel.setFireInput(Fire1);
    }

}