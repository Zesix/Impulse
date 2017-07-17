using UnityEngine;

public class MusicPlaylist : MonoBehaviour {
	public bool ActivateOnAwake = true;
	public AudioClip[] MusicList;

	private void Awake() {
		if (ActivateOnAwake && MusicManager.Instance)
			MusicManager.Instance.ChangePlaylist (this);
	}

	private void Start () {
		// Have playlist persist across scenes.
		DontDestroyOnLoad (gameObject); // Don't destroy this object

		// When a new scene is loaded, destroy the other playlists.
		foreach (var playlist in FindObjectsOfType<MusicPlaylist>()) {
			if (playlist.name != name) {
				Destroy (playlist.gameObject);
			}
		}
	}

}
