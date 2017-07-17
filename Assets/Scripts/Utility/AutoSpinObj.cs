using UnityEngine;

public class AutoSpinObj : MonoBehaviour {	
	public Vector3 SpinVector = new Vector3 (1,0,0);
	private Transform _myTransform;

	private void Start () {
		_myTransform = transform;	
	}

	private void Update () {
		_myTransform.Rotate (SpinVector * Time.deltaTime);	
	}

}