using UnityEngine;

public class ThirdPersonCameraMouseInputController : MonoBehaviour
{
	private ThirdPerson_Camera _thirdPersonCamera;
	public float MouseXSensitivity = 5f;                    // Mouse X sensitivity.
	public float MouseYSensitivity = 5f;                    // Mouse Y sensitivity.
	public float MouseWheelSensitivity = 5f;                // Mouse wheel/scroll sensitivity.

	private void Start ()
	{
		if (!_thirdPersonCamera)
			_thirdPersonCamera = GetComponent<ThirdPerson_Camera> ();
	}

	private void LateUpdate ()
	{
		HandlePlayerInput ();
	}

	private void HandlePlayerInput ()
	{
		var deadZone = 0.01f;
		
		// If right mouse button is down, get mouse axis input.
		if (Input.GetMouseButton (1)) {
			_thirdPersonCamera.MouseX += Input.GetAxis ("Mouse X") * MouseXSensitivity;
			_thirdPersonCamera.MouseY -= Input.GetAxis ("Mouse Y") * MouseYSensitivity;
		}
		
		// Clamp (limit) mouse Y rotation. Uses thirdPersonCameraHelper.cs.
		_thirdPersonCamera.MouseY = ThirdPerson_Helper.clampingAngle (_thirdPersonCamera.MouseY, 
		                                                             _thirdPersonCamera.YMinLimit, 
		                                                             _thirdPersonCamera.YMaxLimit
		);
		
		// Clamp (limit) mouse scroll wheel.
		if (Input.GetAxis ("Mouse ScrollWheel") > deadZone || Input.GetAxis ("Mouse ScrollWheel") < -deadZone) {
			_thirdPersonCamera.DesiredDistance = Mathf.Clamp (_thirdPersonCamera.Distance - 
				Input.GetAxis ("Mouse ScrollWheel") * 
				MouseWheelSensitivity,
			                                                 _thirdPersonCamera.DistanceMin,
			                                                 _thirdPersonCamera.DistanceMax
			);
			_thirdPersonCamera.PreOccludedDistance = _thirdPersonCamera.DesiredDistance;
			_thirdPersonCamera.DistanceCameraSmooth = _thirdPersonCamera.DistanceSmooth;
		}
	}
}
