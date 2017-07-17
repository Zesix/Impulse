#pragma warning disable 1587
/// <summary>
/// XML read write.
/// IMPORTANT: FOR XML LOADING TO BE DONE
/// THIS SCRIPT NEEDS TO BE ON A GAMEOBJECT
/// </summary>
#pragma warning restore 1587

using UnityEngine;

public class XMLReadWrite : MonoBehaviour
{
	
	public static XMLReadWrite Instance { get; private set; }
	
	public void Start ()
	{
		Instance = this;
		
		DontDestroyOnLoad (this);
		
	}

	public XMLExample[] DataLoad (string folderPath, string fileName)
	{
		var newObjects = XMLData.LoadObjects (folderPath, fileName + ".xml");
		
		return newObjects;
	}
	
	public void SaveData (string folderPath, string fileName, XMLExample[] objToSave)
	{
		
		XMLData.SaveObjects (folderPath, fileName, objToSave);
		
	}
}
