using UnityEngine;
using System.Collections;

public class SoundObject {
	public AudioSource Source;
	public GameObject SourceGameObject;
	public Transform SourceTransform;
	
	public AudioClip Clip;
	public string Name;
	
	public SoundObject (AudioClip aClip, string aName, float aVolume) {
		// in this (the constructor) we create a new audio source and store the details of the sound itself
		SourceGameObject = new GameObject("AudioSource_"+aName);
		SourceTransform = SourceGameObject.transform;
		Source = SourceGameObject.AddComponent<AudioSource>();
		Source.name = "AudioSource_"+aName;
		Source.playOnAwake = false;
		Source.clip = aClip;
		Source.volume = aVolume;
		Clip = aClip;
		Name = aName;
	}
	
	public void PlaySound (Vector3 atPosition) {
		SourceTransform.position = atPosition;
		Source.PlayOneShot(Clip);
	}
}

public class BaseSoundManager : MonoBehaviour {
	public static BaseSoundManager Instance;
	
	public AudioClip[] GameSounds;

	private ArrayList _soundObjectList;
	private SoundObject _tempSoundObj;
	
	public float Volume = 1;
	public string GamePrefsName = "DefaultGame"; // DO NOT FORGET TO SET THIS IN THE EDITOR!!
	
	public void Awake() {
		Instance = this;
	}

	private void Start () {
		// we will grab the volume from PlayerPrefs when this script first starts
        if (GamePrefsName != "") {
            Volume = PlayerPrefs.GetFloat(GamePrefsName+"_SFXVol");
        }
		Debug.Log ("BaseSoundController gets volume from prefs "+GamePrefsName+"_SFXVol at "+Volume);
		_soundObjectList = new ArrayList();
		
		// make sound objects for all of the sounds in GameSounds array
		foreach (AudioClip theSound in GameSounds) {
			_tempSoundObj = new SoundObject(theSound, theSound.name, Volume);
			_soundObjectList.Add(_tempSoundObj);
		}
	}
	
	public void PlaySoundByIndex(int anIndexNumber, Vector3 aPosition) {
		// make sure we're not trying to play a sound indexed higher than exists in the array
		if (anIndexNumber>_soundObjectList.Count) {
			Debug.LogWarning ("BaseSoundController>Trying to do PlaySoundByIndex with invalid index number. Playing last sound in array, instead.");
			anIndexNumber = _soundObjectList.Count-1;
		}
		
		_tempSoundObj = (SoundObject)_soundObjectList[anIndexNumber];
		_tempSoundObj.PlaySound(aPosition);  
	}
	
}
