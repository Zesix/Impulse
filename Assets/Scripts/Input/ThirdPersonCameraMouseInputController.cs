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

public class ThirdPersonCameraMouseInputController : MonoBehaviour
{
	private ThirdPerson_Camera thirdPersonCamera;
	public float mouseXSensitivity = 5f;                    // Mouse X sensitivity.
	public float mouseYSensitivity = 5f;                    // Mouse Y sensitivity.
	public float mouseWheelSensitivity = 5f;                // Mouse wheel/scroll sensitivity.
    
	void Start ()
	{
		if (!thirdPersonCamera)
			thirdPersonCamera = GetComponent<ThirdPerson_Camera> ();
	}

	void LateUpdate ()
	{
		HandlePlayerInput ();
	}

	private void HandlePlayerInput ()
	{
		float deadZone = 0.01f;
		
		// If right mouse button is down, get mouse axis input.
		if (Input.GetMouseButton (1)) {
			thirdPersonCamera.mouseX += Input.GetAxis ("Mouse X") * mouseXSensitivity;
			thirdPersonCamera.mouseY -= Input.GetAxis ("Mouse Y") * mouseYSensitivity;
		}
		
		// Clamp (limit) mouse Y rotation. Uses thirdPersonCameraHelper.cs.
		thirdPersonCamera.mouseY = ThirdPerson_Helper.clampingAngle (thirdPersonCamera.mouseY, 
		                                                             thirdPersonCamera.yMinLimit, 
		                                                             thirdPersonCamera.yMaxLimit
		);
		
		// Clamp (limit) mouse scroll wheel.
		if (Input.GetAxis ("Mouse ScrollWheel") > deadZone || Input.GetAxis ("Mouse ScrollWheel") < -deadZone) {
			thirdPersonCamera.desiredDistance = Mathf.Clamp (thirdPersonCamera.distance - 
				Input.GetAxis ("Mouse ScrollWheel") * 
				mouseWheelSensitivity,
			                                                 thirdPersonCamera.distanceMin,
			                                                 thirdPersonCamera.distanceMax
			);
			thirdPersonCamera.preOccludedDistance = thirdPersonCamera.desiredDistance;
			thirdPersonCamera.distanceCameraSmooth = thirdPersonCamera.distanceSmooth;
		}
	}
}
