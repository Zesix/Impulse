using UnityEngine;

public class ExtendedMonoBehaviour : MonoBehaviour {
	// This class is used to add some common variables to MonoBehaviour, rather than
	// constantly repeating the same declarations in every class.
	
	public Transform MyTransform;
	public GameObject MyGameObject;
	
	public bool DidInit;
	
	public int Id;
	
	[System.NonSerialized]
	public Vector3 TempVector3;
	
	[System.NonSerialized]
	public Transform TempTransform;

	// Extensible function for setting ID at runtime.
	public virtual void SetId (int anId) {
		Id = anId;
	}

}