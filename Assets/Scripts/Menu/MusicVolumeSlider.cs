using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeSlider : MonoBehaviour {
	// Set slider value equal to music manager value.
	private void Awake () {
		GetComponent<Slider>().value = MusicManager.Instance.Volume;
	}

	// method to update things on the scene and store the new value
	public void ChangeValue (float value) {
		MusicManager.Instance.Volume = value;
	}

}