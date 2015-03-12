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

public class ArtificialFriction : MonoBehaviour {
	private Rigidbody myBody;
	private Transform myTransform;
	private float myMass;
	private float slideSpeed;
	private Vector3 velo;
	private Vector3 flatVelo;
	private Vector3 myRight;
	private Vector3 TEMPvec3;
	
	public float artificialGrip = 100f;
	
	void Start () {
		// cache references to our rigidbody, mass and transform
		myBody = GetComponent<Rigidbody>();
		myMass = myBody.mass;
		myTransform = transform;
	}
	
	void FixedUpdate () {
		// grab the values we need to calculate friction
		myRight = myTransform.right;
		
		// calculate flat velocity
		velo = myBody.velocity;
		flatVelo.x = velo.x;
		flatVelo.y = 0;
		flatVelo.z = velo.z;
		
		// calculate how much we are sliding
		slideSpeed = Vector3.Dot (myRight, flatVelo);
		
		// build a new vector to compensate for the sliding
		TEMPvec3 = myRight * (-slideSpeed * myMass * artificialGrip);
		
		// apply the correctional force to the rigidbody
		myBody.AddForce (TEMPvec3 * Time.deltaTime);
	}

}
