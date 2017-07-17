using UnityEngine;

public class ViewconeDetection : MonoBehaviour {

	public GameObject AlertIcon;
	private const string ObjectTag = "Player"; // Change if you are checking for another tag.
	public Transform Character;			// Transform of the character.

	void Start () 
	{
		AlertIcon.SetActive (false);
		if (Character == null)
		{
			Debug.LogError (ObjectTag + " viewcone character property is not set!");
		}
	}

	public void ObjectSpotted (Collider col) 
	{
		if(col.CompareTag(ObjectTag))
		{
			RaycastHit newHit;
			Debug.DrawRay(transform.position, col.transform.position - transform.position);

			if(Physics.Raycast (new Ray(transform.position, col.transform.position - transform.position), out newHit))
			{
				if(newHit.collider.CompareTag(ObjectTag))
				{
					Debug.LogWarning (ObjectTag + " spotted by " + Character.name + ".");

					AlertIcon.SetActive (true);

				}
				else
				{
					Debug.Log (ObjectTag + " within viewcone of " + Character.name + ", but is obstructed.");
					AlertIcon.SetActive (false);

				}
			}
		}
	}

	public void ObjectLeft (Collider col)
	{
		if(col.CompareTag(ObjectTag))
		{
			AlertIcon.SetActive (false);
		}

	}
}
