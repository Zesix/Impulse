namespace Impulse.Detector
{
	using UnityEngine;
	using System.Collections;

	public class DetectorDemoCharacterController : MonoBehaviour
	{

		public float moveSpeed = 6;

		private Rigidbody myRigidbody;
		private Camera viewCamera;
		private Vector3 velocity;

		private void Start()
		{
			myRigidbody = GetComponent<Rigidbody>();
			viewCamera = Camera.main;
		}

		private void Update()
		{
			Vector3 mousePos = viewCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
				viewCamera.transform.position.y));
			transform.LookAt(mousePos + Vector3.up * transform.position.y);
			velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * moveSpeed;
		}

		private void FixedUpdate()
		{
			myRigidbody.MovePosition(myRigidbody.position + velocity * Time.fixedDeltaTime);
		}
	}
}
