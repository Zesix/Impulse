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

public static class ThirdPerson_Helper
{
	public struct ClipPlanePoints
	{
		public Vector3 upperLeft;
		public Vector3 upperRight;
		public Vector3 lowerLeft;
		public Vector3 lowerRight;
	}
	
	// Clamp angle to between -360, 360.
	public static float clampingAngle (float angle, float min, float max)
	{
		do {
			if (angle < -360)
				angle += 360;
			if (angle > 360)
				angle -= 360;
		} while (angle < -360 || angle > 360);

		return Mathf.Clamp (angle, min, max);
	}
	
	public static ClipPlanePoints ClipPlaneAtNear (Vector3 pos)
	{
		ClipPlanePoints clipPlanePoints = new ClipPlanePoints ();
		
		// Ensure there is a main camera for us to check.
		if (!Camera.main)
			return clipPlanePoints;
		
		// Properties to determine the main camera's near clip plane.
		Transform mainCameraTransform = Camera.main.transform;
		float halfFieldOfView = (Camera.main.fieldOfView / 2) * Mathf.Deg2Rad;
		float aspect = Camera.main.aspect;
		float distance = Camera.main.nearClipPlane;
		float height = distance * Mathf.Tan (halfFieldOfView);
		float width = height * aspect;
		
		clipPlanePoints.lowerRight = pos + mainCameraTransform.right * width;
		clipPlanePoints.lowerRight -= mainCameraTransform.up * height;
		clipPlanePoints.lowerRight += mainCameraTransform.forward * distance;
		
		clipPlanePoints.lowerLeft = pos - mainCameraTransform.right * width;
		clipPlanePoints.lowerLeft -= mainCameraTransform.up * height;
		clipPlanePoints.lowerLeft += mainCameraTransform.forward * distance;
		
		clipPlanePoints.upperRight = pos + mainCameraTransform.right * width;
		clipPlanePoints.upperRight += mainCameraTransform.up * height;
		clipPlanePoints.upperRight += mainCameraTransform.forward * distance;
		
		clipPlanePoints.upperLeft = pos - mainCameraTransform.right * width;
		clipPlanePoints.upperLeft += mainCameraTransform.up * height;
		clipPlanePoints.upperLeft += mainCameraTransform.forward * distance;
		
		return clipPlanePoints;
	}
}