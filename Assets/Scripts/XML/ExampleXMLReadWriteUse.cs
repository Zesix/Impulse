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

/// <summary>
/// This saves an XML file with example properties.
/// </summary>

using UnityEngine;
using System.Collections;
using System.Xml.Serialization;

[System.Serializable]
public class XMLExample //The class we use to serialize can be called anything we want
{
	[XmlElement]
	public int castCount{ get; set; }
	
	[XmlElement]
	public string damage{ get; set; }

	[XmlElement]
	public int damageAmount{ get; set; }

	[XmlElement]
	public float castingTime{ get; set; }

	public void Initialize (int CastCount = 2, string Damage = "Fire", int DamageAmount = 39, float CastingTime = 3.14159f)
	{
		castCount = CastCount;
		damage = Damage;
		damageAmount = DamageAmount;
		castingTime = CastingTime;

	}
}

public class ExampleXMLReadWriteUse : MonoBehaviour
{

	void Update ()
	{

		if (Input.GetKeyDown (KeyCode.S)) {
			Debug.Log ("Saving data!");

			XMLExample[] ourList = new XMLExample[3];
			ourList [0] = new XMLExample ();
			ourList [1] = new XMLExample ();
			ourList [2] = new XMLExample ();

			ourList [0].Initialize (1, "Lightning", 499, 5.0f);
			ourList [1].Initialize (4, "Earth", 550, 7.0f);
			ourList [2].Initialize (20, "Water", 600, 8.0f);

			XMLData.SaveObjects ("Potato", "Mashed", ourList);

			Debug.Log ("Data saving complete! Saved to: " + "Potato/Mashed.xml");
		}

		if (Input.GetKeyDown (KeyCode.D)) {
			Debug.Log ("Starting load!");

			XMLExample[] ourExampleResults = XMLData.LoadObjects ("Potato", "Mashed.xml");

			foreach (XMLExample e in ourExampleResults) {
				Debug.Log ("CastCount: " + e.castCount + "\nDamage: " + e.damage + "\nDamageAmount: " + e.damageAmount + "\nCastingTime: " + e.castingTime);

			}
		}
	}
}
