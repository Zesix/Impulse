using UnityEngine;

public class ArtificialFriction : MonoBehaviour {
	private Rigidbody _myBody;
	private Transform _myTransform;
	private float _myMass;
	private float _slideSpeed;
	private Vector3 _velocity;
	private Vector3 _flatVelocity;
	private Vector3 _myRight;
	private Vector3 _tempVector3;
	
	public float ArtificialGrip = 100f;

	private void Start () {
		// cache references to our rigidbody, mass and transform
		_myBody = GetComponent<Rigidbody>();
		_myMass = _myBody.mass;
		_myTransform = transform;
	}

	private void FixedUpdate () {
		// grab the values we need to calculate friction
		_myRight = _myTransform.right;
		
		// calculate flat velocity
		_velocity = _myBody.velocity;
		_flatVelocity.x = _velocity.x;
		_flatVelocity.y = 0;
		_flatVelocity.z = _velocity.z;
		
		// calculate how much we are sliding
		_slideSpeed = Vector3.Dot (_myRight, _flatVelocity);
		
		// build a new vector to compensate for the sliding
		_tempVector3 = _myRight * (-_slideSpeed * _myMass * ArtificialGrip);
		
		// apply the correctional force to the rigidbody
		_myBody.AddForce (_tempVector3 * Time.deltaTime);
	}

}
