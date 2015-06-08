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

public class Mouse_Input : BaseInputController
{
    // Player input
    [SerializeField]
    Vector2 mouseInput;
    [SerializeField]
    float CameraDistance = 10.0f;

    public override void CheckInput()
    {
        // Get mouse position
        Vector3 mouse3DInput = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                                                                        CameraDistance));
        mouseInput = new Vector2(mouse3DInput.x, mouse3DInput.y);

        // get fire / action buttons
        Fire1 = Input.GetButton("Fire1");
    }

    private void Update()
    {
        CheckInput();
    }

}