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

public class ExtendedMonoBehaviour : MonoBehaviour {
	// This class is used to add some common variables to MonoBehaviour, rather than
	// constantly repeating the same declarations in every class.
	
	public Transform myTransform;
	public GameObject myGameObject;
	
	public bool didInit;
	
	public int id;
	
	[System.NonSerialized]
	public Vector3 tempVector3;
	
	[System.NonSerialized]
	public Transform tempTransform;

	// Extensible function for setting ID at runtime.
	public virtual void SetID (int anID) {
		id = anID;
	}

}