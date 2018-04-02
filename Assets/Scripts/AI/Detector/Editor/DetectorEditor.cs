namespace Impulse.Detector
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(SphereDetector))]
	public class DetectorEditor : Editor
	{
		private void OnSceneGUI()
		{
			var detector = (SphereDetector) target;
			Handles.color = Color.white;

			var viewAngleA = detector.DirectionFromAngle(-detector.ViewAngle / 2, false);
			var viewAngleB = detector.DirectionFromAngle(detector.ViewAngle / 2, false);
			Handles.DrawLine(detector.transform.position, detector.transform.position + viewAngleA * detector.DetectionRange);
			Handles.DrawLine(detector.transform.position, detector.transform.position + viewAngleB * detector.DetectionRange);
		}
	}
}
