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

public class ViewconeDetection : MonoBehaviour {

	public GameObject ourAlertIcon;
	private string objectTag = "Player";	// Change if you are checking for another tag.
	public Transform character;			// Transform of the character.

	void Start () 
	{
		ourAlertIcon.SetActive (false);
		if (character == null)
		{
			Debug.LogError (objectTag + " viewcone character property is not set!");
		}
	}

	public void ObjectSpotted (Collider col) 
	{
		if(col.tag == objectTag)
		{
			RaycastHit newHit;
			Debug.DrawRay(transform.position, col.transform.position - transform.position);

			if(Physics.Raycast (new Ray(transform.position, col.transform.position - transform.position), out newHit))
			{
				if(newHit.collider.tag == objectTag)
				{
					Debug.LogWarning (objectTag + " spotted by " + character.name + ".");

					ourAlertIcon.SetActive (true);

				}
				else
				{
					Debug.Log (objectTag + " within viewcone of " + character.name + ", but is obstructed.");
					ourAlertIcon.SetActive (false);

				}
			}
		}
	}

	public void ObjectLeft (Collider col)
	{
		if(col.tag == objectTag)
		{
			ourAlertIcon.SetActive (false);

		}

	}
}
