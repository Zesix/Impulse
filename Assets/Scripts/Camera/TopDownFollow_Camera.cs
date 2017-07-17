using UnityEngine;

public class TopDownFollow_Camera : MonoBehaviour {

	public Transform FollowTarget;
	public Vector3 TargetOffset;
	public float MoveSpeed = 2f;
	
	private Transform _myTransform;

	private void Start () {
		// Cache camera transform
		_myTransform = transform;	
	}
	
	public void SetTarget ( Transform aTransform ) {
		FollowTarget = aTransform;	
	}

	private void LateUpdate () {
		if(FollowTarget != null)
			_myTransform.position = Vector3.Lerp( _myTransform.position, FollowTarget.position + TargetOffset, MoveSpeed * Time.deltaTime );
	}

}
