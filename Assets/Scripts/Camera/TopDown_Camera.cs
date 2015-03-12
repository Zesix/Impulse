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

public class TopDown_Camera : MonoBehaviour {

	public Transform followTarget;
	public Vector3 targetOffset;
	public float moveSpeed = 2f;
	
	private Transform myTransform;
	
	void Start () {
		// Cache camera transform
		myTransform = transform;	
	}
	
	public void SetTarget ( Transform aTransform ) {
		followTarget = aTransform;	
	}
	
	void LateUpdate () {
		if(followTarget != null)
			myTransform.position = Vector3.Lerp( myTransform.position, followTarget.position + targetOffset, moveSpeed * Time.deltaTime );
	}

}
