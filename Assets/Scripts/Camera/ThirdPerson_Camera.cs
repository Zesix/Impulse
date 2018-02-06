using UnityEngine;

public class ThirdPerson_Camera : MonoBehaviour
{
	public ThirdPerson_Camera Instance;                     // Reference to the Main Camera.
	public Transform TargetLookTransform;                   // Transform of the camera will be looking at.
	public float Distance = 5f;                             // Start distance of camera.
	public float DistanceMin = 3f;                          // Minimum distance of camera zoom.
	public float DistanceMax = 10f;                         // Maximum distance of camera zoom.
	public float DistanceSmooth = 0.05f;                    // Camera zooming smooth factor.
	public float DistanceCameraResumeSmooth = 1f;   		// Distance at which point smoothing is resumed after occlusion handling is no longer occuring.
	public float XSmooth = 0.05f;                           // Smoothness factor for x position calculations.
	public float YSmooth = 0.1f;                            // Smoothness factor for y position calculations.
	public float YMinLimit = -40f;
	public float YMaxLimit = 80f;
	public float OcclusionDistanceStep = 0.5f;
	public int MaxOcclusionChecks = 10;                     // Max number of times to check for occlusion.

	[System.NonSerialized]
	public float MouseX;
	[System.NonSerialized]
	public float MouseY;
	[System.NonSerialized]
	public float DesiredDistance;
	[System.NonSerialized]
	public float DistanceCameraSmooth;              // Camera smoothing distance (after occlusion is no longer happening).
	[System.NonSerialized]
	public float PreOccludedDistance;

	private float _velocityX;
	private float _velocityY;
	private float _velocityZ;
	private float _velocityDistance;
	private float _startDistance;
	private Vector3 _position = new Vector3 (768f, 3.5f, 903f);
	private Vector3 _desiredPosition = new Vector3 (768f, 3.5f, 903f);

	private void Start ()
	{
		if (Instance == null)
			Instance = this;

		// If main camera is null, set as main camera
		if (Camera.main == null)
			tag = "MainCamera";

		// Ensure our distance is between min and max (valid)
		Distance = Mathf.Clamp (Distance, DistanceMin, DistanceMax);
		_startDistance = Distance;
		Reset ();
	}
	
	private void LateUpdate ()
	{
		if (TargetLookTransform == null) {
			return;
		}
		
		var count = 0;
		do {
			CalculateDesiredPosition ();
			count++;
		} while (CheckIfOccluded(count));
		
		UpdatePosition ();
	}
	
	// Smoothing.
	private void CalculateDesiredPosition ()
	{
		// Evaluate distance.
		ResetDesiredDistance ();
		Distance = Mathf.SmoothDamp (Distance, DesiredDistance, ref _velocityDistance, DistanceCameraSmooth);
		
		// Calculate desired position.
		_desiredPosition = CalculatePosition (MouseY, MouseX, Distance);
	}
	
	private bool CheckIfOccluded (int count)
	{
		var isOccluded = false;
		var nearestDistance = CheckCameraPoints (TargetLookTransform.position, _desiredPosition);
		
		if (nearestDistance != -1) {
			if (count < MaxOcclusionChecks) {
				isOccluded = true;
				Distance -= OcclusionDistanceStep;
				
				// 0.25 is a good default value.
				if (Distance < 0.25f) {
					Distance = 0.25f;
				}
			} else {
				Distance = nearestDistance - GetComponent<Camera> ().nearClipPlane;
			}
			DesiredDistance = Distance;
			DistanceCameraSmooth = DistanceCameraResumeSmooth;
		}

		return isOccluded;
	}
	
	private Vector3 CalculatePosition (float rotX, float rotY, float rotDist)
	{
		var direction = new Vector3 (0, 0, -rotDist);                      // -distance because we want it to point behind our character.
		var rotation = Quaternion.Euler (rotX, rotY, 0);
		return TargetLookTransform.position + (rotation * direction);
	}

	private float CheckCameraPoints (Vector3 from, Vector3 to)
	{
		var nearestDistance = -1f;
		
		RaycastHit hitInfo;
		
		var clipPlanePoints = ThirdPerson_Helper.ClipPlaneAtNear (to);

		
		// Draw the raycasts going through the near clip plane vertexes.
		Debug.DrawLine (from, to + transform.forward * -GetComponent<Camera> ().nearClipPlane, Color.red);
		Debug.DrawLine (from, clipPlanePoints.UpperLeft, Color.red);
		Debug.DrawLine (from, clipPlanePoints.UpperRight, Color.red);
		Debug.DrawLine (from, clipPlanePoints.LowerLeft, Color.red);
		Debug.DrawLine (from, clipPlanePoints.LowerRight, Color.red);
		Debug.DrawLine (clipPlanePoints.UpperLeft, clipPlanePoints.UpperRight, Color.red);
		Debug.DrawLine (clipPlanePoints.UpperRight, clipPlanePoints.LowerRight, Color.red);
		Debug.DrawLine (clipPlanePoints.LowerRight, clipPlanePoints.LowerLeft, Color.red);
		Debug.DrawLine (clipPlanePoints.LowerLeft, clipPlanePoints.UpperLeft, Color.red);
        

		if (Physics.Linecast (from, clipPlanePoints.UpperLeft, out hitInfo) && !hitInfo.collider.CompareTag("Player"))
			nearestDistance = hitInfo.distance;
		if (Physics.Linecast (from, clipPlanePoints.LowerLeft, out hitInfo) && !hitInfo.collider.CompareTag("Player"))
		if (hitInfo.distance < nearestDistance || nearestDistance == -1)
			nearestDistance = hitInfo.distance;
		if (Physics.Linecast (from, clipPlanePoints.UpperRight, out hitInfo) && !hitInfo.collider.CompareTag("Player"))
		if (hitInfo.distance < nearestDistance || nearestDistance == -1)
			nearestDistance = hitInfo.distance;
		if (Physics.Linecast (from, to + transform.forward * -GetComponent<Camera> ().nearClipPlane, out hitInfo) && !hitInfo.collider.CompareTag("Player"))
		if (hitInfo.distance < nearestDistance || nearestDistance == -1)
			nearestDistance = hitInfo.distance;
		
		return nearestDistance;
	}
	
	private void ResetDesiredDistance ()
	{
		if (DesiredDistance < PreOccludedDistance) {
			var pos = CalculatePosition (MouseY, MouseX, PreOccludedDistance);
			var nearestDistance = CheckCameraPoints (TargetLookTransform.position, pos);

			if (CheckBehindCam (_desiredPosition) && (nearestDistance == -1 || nearestDistance > PreOccludedDistance)) {
				DesiredDistance = PreOccludedDistance;
			}
		}
	}

	private bool CheckBehindCam (Vector3 to) // Checks the area behind the camera to make sure the camera can back up to its desired spot
	{
		RaycastHit hitInfo;
        
		var pos = CalculatePosition (MouseY, MouseX, PreOccludedDistance);
        
		var clipPlanePoints = ThirdPerson_Helper.ClipPlaneAtNear (to);
		/* Debug.DrawLine(this.transform.position, pos, Color.blue);
        Debug.DrawLine(clipPlanePoints.upperLeft, pos, Color.blue);
        Debug.DrawLine(clipPlanePoints.upperRight, pos, Color.blue);
        Debug.DrawLine(clipPlanePoints.lowerLeft, pos, Color.blue);
        Debug.DrawLine(clipPlanePoints.lowerRight, pos, Color.blue);
        */
		if (Physics.Linecast (clipPlanePoints.UpperLeft, pos, out hitInfo)) {
			return false;
		}
		if (Physics.Linecast (clipPlanePoints.UpperLeft, pos, out hitInfo)) {
			return false;
		}
		if (Physics.Linecast (clipPlanePoints.UpperRight, pos, out hitInfo)) {
			return false;
		}
		if (Physics.Linecast (clipPlanePoints.LowerLeft, pos, out hitInfo)) {
			return false;
		}
		if (Physics.Linecast (clipPlanePoints.LowerRight, pos, out hitInfo)) {
			return false;
		}
		return true;
	}
	
	private void UpdatePosition ()
	{
		var posX = Mathf.SmoothDamp (_position.x, _desiredPosition.x, ref _velocityX, XSmooth * Time.deltaTime);
		var posY = Mathf.SmoothDamp (_position.y, _desiredPosition.y, ref _velocityY, YSmooth * Time.deltaTime);
		var posZ = Mathf.SmoothDamp (_position.z, _desiredPosition.z, ref _velocityZ, XSmooth * Time.deltaTime);
		
		_position = new Vector3 (posX, posY, posZ);
		
		transform.position = _position;
		
		transform.LookAt (TargetLookTransform);
	}

	public void Reset ()
	{
		MouseX = 0f;
		MouseY = 10f;
		Distance = _startDistance;
		DesiredDistance = Distance;
		PreOccludedDistance = Distance;
	}
}
