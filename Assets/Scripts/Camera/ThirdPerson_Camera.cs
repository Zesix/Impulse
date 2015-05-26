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

public class ThirdPerson_Camera : MonoBehaviour
{
	public ThirdPerson_Camera instance;                     // Reference to the Main Camera.
	public Transform targetLookTransform;                   // Transform of the camera will be looking at.
	public float distance = 5f;                             // Start distance of camera.
	public float distanceMin = 3f;                          // Minimum distance of camera zoom.
	public float distanceMax = 10f;                         // Maximum distance of camera zoom.
	public float distanceSmooth = 0.05f;                    // Camera zooming smooth factor.
	public float distanceCameraResumeSmooth = 1f;   		// Distance at which point smoothing is resumed after occlusion handling is no longer occuring.
	public float xSmooth = 0.05f;                           // Smoothness factor for x position calculations.
	public float ySmooth = 0.1f;                            // Smoothness factor for y position calculations.
	public float yMinLimit = -40f;
	public float yMaxLimit = 80f;
	public float occlusionDistanceStep = 0.5f;
	public int maxOcclusionChecks = 10;                     // Max number of times to check for occlusion.

	[System.NonSerialized]
	public float mouseX = 0f;
	[System.NonSerialized]
	public float mouseY = 0f;
	[System.NonSerialized]
	public float desiredDistance = 0f;
	[System.NonSerialized]
	public float distanceCameraSmooth = 0f;              // Camera smoothing distance (after occlusion is no longer happening).
	[System.NonSerialized]
	public float preOccludedDistance = 0f;

	private float velocityX = 0f;
	private float velocityY = 0f;
	private float velocityZ = 0f;
	private float velocityDistance = 0f;
	private float startDistance = 0f;
	private Vector3 position = new Vector3 (768f, 3.5f, 903f);
	private Vector3 desiredPosition = new Vector3 (768f, 3.5f, 903f);
	
	void Start ()
	{
		if (instance == null)
			instance = this;

		// If main camera is null, set as main camera
		if (Camera.main == null)
			this.tag = "MainCamera";

		// Ensure our distance is between min and max (valid)
		distance = Mathf.Clamp (distance, distanceMin, distanceMax);
		startDistance = distance;
		Reset ();
	}
	
	private void LateUpdate ()
	{
		if (targetLookTransform == null) {
			return;
		}
		
		int count = 0;
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
		distance = Mathf.SmoothDamp (distance, desiredDistance, ref velocityDistance, distanceCameraSmooth);
		
		// Calculate desired position.
		desiredPosition = CalculatePosition (mouseY, mouseX, distance);
	}
	
	private bool CheckIfOccluded (int count)
	{
		bool isOccluded = false;
		float nearestDistance = CheckCameraPoints (targetLookTransform.position, desiredPosition);
		
		if (nearestDistance != -1) {
			if (count < maxOcclusionChecks) {
				isOccluded = true;
				distance -= occlusionDistanceStep;
				
				// 0.25 is a good default value.
				if (distance < 0.25f) {
					distance = 0.25f;
				}
			} else {
				distance = nearestDistance - GetComponent<Camera> ().nearClipPlane;
			}
			desiredDistance = distance;
			distanceCameraSmooth = distanceCameraResumeSmooth;
		}

		return isOccluded;
	}
	
	private Vector3 CalculatePosition (float rotX, float rotY, float rotDist)
	{
		Vector3 direction = new Vector3 (0, 0, -rotDist);                      // -distance because we want it to point behind our character.
		Quaternion rotation = Quaternion.Euler (rotX, rotY, 0);
		return targetLookTransform.position + (rotation * direction);
	}

	private float CheckCameraPoints (Vector3 from, Vector3 to)
	{
		float nearestDistance = -1f;
		
		RaycastHit hitInfo;
		
		ThirdPerson_Helper.ClipPlanePoints clipPlanePoints =
			ThirdPerson_Helper.ClipPlaneAtNear (to);

		
		// Draw the raycasts going through the near clip plane vertexes.
		Debug.DrawLine (from, to + transform.forward * -GetComponent<Camera> ().nearClipPlane, Color.red);
		Debug.DrawLine (from, clipPlanePoints.upperLeft, Color.red);
		Debug.DrawLine (from, clipPlanePoints.upperRight, Color.red);
		Debug.DrawLine (from, clipPlanePoints.lowerLeft, Color.red);
		Debug.DrawLine (from, clipPlanePoints.lowerRight, Color.red);
		Debug.DrawLine (clipPlanePoints.upperLeft, clipPlanePoints.upperRight, Color.red);
		Debug.DrawLine (clipPlanePoints.upperRight, clipPlanePoints.lowerRight, Color.red);
		Debug.DrawLine (clipPlanePoints.lowerRight, clipPlanePoints.lowerLeft, Color.red);
		Debug.DrawLine (clipPlanePoints.lowerLeft, clipPlanePoints.upperLeft, Color.red);
        

		if (Physics.Linecast (from, clipPlanePoints.upperLeft, out hitInfo) && hitInfo.collider.tag != "Player")
			nearestDistance = hitInfo.distance;
		if (Physics.Linecast (from, clipPlanePoints.lowerLeft, out hitInfo) && hitInfo.collider.tag != "Player")
		if (hitInfo.distance < nearestDistance || nearestDistance == -1)
			nearestDistance = hitInfo.distance;
		if (Physics.Linecast (from, clipPlanePoints.upperRight, out hitInfo) && hitInfo.collider.tag != "Player")
		if (hitInfo.distance < nearestDistance || nearestDistance == -1)
			nearestDistance = hitInfo.distance;
		if (Physics.Linecast (from, to + transform.forward * -GetComponent<Camera> ().nearClipPlane, out hitInfo) && hitInfo.collider.tag != "Player")
		if (hitInfo.distance < nearestDistance || nearestDistance == -1)
			nearestDistance = hitInfo.distance;
		
		return nearestDistance;
	}
	
	private void ResetDesiredDistance ()
	{
		if (desiredDistance < preOccludedDistance) {
			Vector3 pos = CalculatePosition (mouseY, mouseX, preOccludedDistance);
			float nearestDistance = CheckCameraPoints (targetLookTransform.position, pos);

			if (CheckBehindCam (desiredPosition) && (nearestDistance == -1 || nearestDistance > preOccludedDistance)) {
				desiredDistance = preOccludedDistance;
			}
		}
	}

	private bool CheckBehindCam (Vector3 to) // Checks the area behind the camera to make sure the camera can back up to its desired spot
	{
		RaycastHit hitInfo;
        
		Vector3 pos = CalculatePosition (mouseY, mouseX, preOccludedDistance);
        
		ThirdPerson_Helper.ClipPlanePoints clipPlanePoints =
             ThirdPerson_Helper.ClipPlaneAtNear (to);
		/* Debug.DrawLine(this.transform.position, pos, Color.blue);
        Debug.DrawLine(clipPlanePoints.upperLeft, pos, Color.blue);
        Debug.DrawLine(clipPlanePoints.upperRight, pos, Color.blue);
        Debug.DrawLine(clipPlanePoints.lowerLeft, pos, Color.blue);
        Debug.DrawLine(clipPlanePoints.lowerRight, pos, Color.blue);
        */
		if (Physics.Linecast (clipPlanePoints.upperLeft, pos, out hitInfo)) {
			return false;
		}
		if (Physics.Linecast (clipPlanePoints.upperLeft, pos, out hitInfo)) {
			return false;
		}
		if (Physics.Linecast (clipPlanePoints.upperRight, pos, out hitInfo)) {
			return false;
		}
		if (Physics.Linecast (clipPlanePoints.lowerLeft, pos, out hitInfo)) {
			return false;
		}
		if (Physics.Linecast (clipPlanePoints.lowerRight, pos, out hitInfo)) {
			return false;
		}
		return true;
	}
	
	private void UpdatePosition ()
	{
		float posX = Mathf.SmoothDamp (position.x, desiredPosition.x, ref velocityX, xSmooth * Time.deltaTime);
		float posY = Mathf.SmoothDamp (position.y, desiredPosition.y, ref velocityY, ySmooth * Time.deltaTime);
		float posZ = Mathf.SmoothDamp (position.z, desiredPosition.z, ref velocityZ, xSmooth * Time.deltaTime);
		
		position = new Vector3 (posX, posY, posZ);
		
		transform.position = position;
		
		transform.LookAt (targetLookTransform);
	}

	public void Reset ()
	{
		mouseX = 0f;
		mouseY = 10f;
		distance = startDistance;
		desiredDistance = distance;
		preOccludedDistance = distance;
	}
}
