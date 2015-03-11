using UnityEngine;
using System.Collections;

public class SaveMusicValue : MonoBehaviour 
{
	// Name of the prefs
	public string Name;

	// The defult value of the prefs
	[Range(0,1)]
	public float DefaultValue;

	// method to update things on the scene and store the new value
	public void ChangeValue(float value)
	{
        PlayerPrefs.SetFloat(Name, value);
		MusicManager.Instance.Volume = value;
	}

	void Awake() {
		MusicManager.Instance.Volume = PlayerPrefs.GetFloat (Name, DefaultValue);
	}
}
