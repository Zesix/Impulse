#pragma warning disable 1587
/// <summary>
/// This saves an XML file with example properties.
/// </summary>
#pragma warning restore 1587

using UnityEngine;
using System.Xml.Serialization;

[System.Serializable]
public class XMLExample //The class we use to serialize can be called anything we want
{
    [XmlElement]
    public int CastCount{ get; set; }
	
    [XmlElement]
    public string Damage{ get; set; }

    [XmlElement]
    public int DamageAmount{ get; set; }

    [XmlElement]
    public float CastingTime{ get; set; }

    public void Initialize (int castCount = 2, string damage = "Fire", int damageAmount = 39, float castingTime = 3.14159f)
    {
        CastCount = castCount;
        Damage = damage;
        DamageAmount = damageAmount;
        CastingTime = castingTime;

    }
}

public class ExampleXmlReadWriteUse : MonoBehaviour
{
    private void Update ()
    {

        if (Input.GetKeyDown (KeyCode.S)) {
            Debug.Log ("Saving data!");

            var ourList = new XMLExample[3];
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

            var ourExampleResults = XMLData.LoadObjects ("Potato", "Mashed.xml");

            foreach (var e in ourExampleResults) {
                Debug.Log ("CastCount: " + e.CastCount + "\nDamage: " + e.Damage + "\nDamageAmount: " + e.DamageAmount + "\nCastingTime: " + e.CastingTime);

            }
        }
    }
}