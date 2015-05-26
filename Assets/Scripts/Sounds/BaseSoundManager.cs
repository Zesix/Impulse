using UnityEngine;
using System.Collections;

public class SoundObject {
	public AudioSource source;
	public GameObject sourceGO;
	public Transform sourceTR;
	
	public AudioClip clip;
	public string name;
	
	public SoundObject (AudioClip aClip, string aName, float aVolume) {
		// in this (the constructor) we create a new audio source and store the details of the sound itself
		sourceGO = new GameObject("AudioSource_"+aName);
		sourceTR = sourceGO.transform;
		source = sourceGO.AddComponent<AudioSource>();
		source.name = "AudioSource_"+aName;
		source.playOnAwake = false;
		source.clip = aClip;
		source.volume = aVolume;
		clip = aClip;
		name = aName;
	}
	
	public void PlaySound (Vector3 atPosition) {
		sourceTR.position = atPosition;
		source.PlayOneShot(clip);
	}
}

public class BaseSoundManager : MonoBehaviour {
	public static BaseSoundManager Instance;
	
	public AudioClip[] GameSounds;
	
	private int totalSounds;
	private ArrayList soundObjectList;
	private SoundObject tempSoundObj;
	
	public float volume = 1;
	public string gamePrefsName = "DefaultGame"; // DO NOT FORGET TO SET THIS IN THE EDITOR!!
	
	public void Awake() {
		Instance = this;
	}
	
	void Start () {
		// we will grab the volume from PlayerPrefs when this script first starts
        if (gamePrefsName != "") {
            volume = PlayerPrefs.GetFloat(gamePrefsName+"_SFXVol");
        }
		Debug.Log ("BaseSoundController gets volume from prefs "+gamePrefsName+"_SFXVol at "+volume);
		soundObjectList = new ArrayList();
		
		// make sound objects for all of the sounds in GameSounds array
		foreach (AudioClip theSound in GameSounds) {
			tempSoundObj = new SoundObject(theSound, theSound.name, volume);
			soundObjectList.Add(tempSoundObj);
			totalSounds++;
		}
	}
	
	public void PlaySoundByIndex(int anIndexNumber, Vector3 aPosition) {
		// make sure we're not trying to play a sound indexed higher than exists in the array
		if (anIndexNumber>soundObjectList.Count) {
			Debug.LogWarning ("BaseSoundController>Trying to do PlaySoundByIndex with invalid index number. Playing last sound in array, instead.");
			anIndexNumber = soundObjectList.Count-1;
		}
		
		tempSoundObj = (SoundObject)soundObjectList[anIndexNumber];
		tempSoundObj.PlaySound(aPosition);  
	}
	
}
