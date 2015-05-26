using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MusicVolumeSlider : MonoBehaviour {
	// Set slider value equal to music manager value.
	void Awake () {
		GetComponent<Slider>().value = MusicManager.Instance.Volume;
	}

	// method to update things on the scene and store the new value
	public void ChangeValue (float value) {
		MusicManager.Instance.Volume = value;
	}

}