/*****************************************
 * This file is part of Impulse Framework.

    Impulse Framework is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    any later version.

    Impulse Framework is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Impulse Framework.  If not, see <http://www.gnu.org/licenses/>.
*****************************************/

using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
	public static MusicManager Instance;	        // Singleton
	[SerializeField]
	private float
		volume;

	public float Volume {
		get {
			return source.volume;
		}
		set {
			volume = value;
			source.volume = value;
		}
	}
	
	public MusicPlaylist Playlist;
	public bool Shuffle;
	public RepeatMode Repeat;
	public float FadeDuration;
	public bool PlayOnAwake;
	private AudioSource source;
	
	void Start ()
	{
		// If there is no instance of this class, set it.
		if (Instance == null) {
			DontDestroyOnLoad (gameObject); // Don't destroy this object
			Instance = this;
		} else {
			Debug.LogError ("There is already a Music Manager in the scene.");
			GameObject.Destroy (this);
		}
		
		// grab audio source
		source = GetComponent<AudioSource> ();
		source.playOnAwake = false;
		if (FadeDuration > 0)
			source.volume = 0f;
		else
			Volume = volume;
		if (Playlist == null)
			return;
		if (Playlist.MusicList.Length > 0)
			source.clip = Playlist.MusicList [0];
		else 
			Debug.LogError ("There are no music in the list");
		
		if (PlayOnAwake)
			Play ();
	}
	
	public void Play ()
	{
		if (Playlist) {
			StartCoroutine (PlayMusicList ());
		}
	}
	
	public void Stop (bool fade)
	{
		StopAllCoroutines ();
		if (fade)
			StartCoroutine (StopWithFade ());
		else
			source.Stop ();
	}
	
	public void Next ()
	{
		source.Stop ();
	}
	
	public void ChangePlaylist (MusicPlaylist list)
	{
		Playlist = list;
		_counter = 0;
		StopAllCoroutines ();
		StartCoroutine (ChangePlaylistE ());
	}
	
	private IEnumerator ChangePlaylistE ()
	{
		if (source.isPlaying)
			yield return StartCoroutine (StopWithFade ());
		StartCoroutine (PlayMusicList ());
	}
	
	private IEnumerator StopWithFade ()
	{
		if (FadeDuration > 0) {
			float lerpValue = 0f;
			while (lerpValue < 1f) {
				lerpValue += Time.deltaTime / FadeDuration;
				source.volume = Mathf.Lerp (volume, 0f, lerpValue);
				yield return null;
			}
		}
		source.Stop ();
	}
	
	public void PlaySong (AudioClip song)
	{
		StartCoroutine (PlaySongE (song));
	}
	
	private IEnumerator PlaySongE (AudioClip clip)
	{
		source.Stop ();
		source.clip = clip;
		source.Play ();
		StartCoroutine (FadeIn ());
		while (source.isPlaying) {
			if (source.clip.length - source.time <= FadeDuration) {
				yield return StartCoroutine (FadeOut ());
			}
			yield return null;
		}
	}
	
	private int _counter = 0;

	private IEnumerator PlayMusicList ()
	{
		while (true) {
			yield return StartCoroutine (PlaySongE (Playlist.MusicList [_counter]));
			if (Repeat == RepeatMode.Track) {
				
			} else if (Shuffle) {
				int newTrack = GetNewTrack ();
				while (newTrack == _counter) {
					newTrack = GetNewTrack ();
				}
				_counter = newTrack;
				
			} else {
				_counter++;
				if (_counter >= Playlist.MusicList.Length - 1) {
					if (Repeat == RepeatMode.Playlist) {
						_counter = 0;
					} else
						yield break;
				}
			}
		}
	}
	
	private IEnumerator FadeOut ()
	{
		if (FadeDuration > 0f) {
			float startTime = source.clip.length - FadeDuration;
			float lerpValue = 0f;
			while (lerpValue < 1f && source.isPlaying) {
				lerpValue = Mathf.InverseLerp (startTime, source.clip.length, source.time);
				source.volume = Mathf.Lerp (volume, 0f, lerpValue);
				yield return null;
			}
			source.volume = 0f;
		} else {
			yield break;
		}
	}
	
	private IEnumerator FadeIn ()
	{
		if (FadeDuration > 0f) {
			float lerpValue = 0f;
			while (lerpValue < 1f && source.isPlaying) {
				lerpValue = Mathf.InverseLerp (0f, FadeDuration, source.time);
				source.volume = Mathf.Lerp (0f, volume, lerpValue);
				yield return null;
			}
			source.volume = volume;
		} else {
			yield break;
		}
	}
	
	private int GetNewTrack ()
	{
		return Random.Range (0, Playlist.MusicList.Length);
	}
	
}

public enum RepeatMode
{
	Playlist,
	Track,
	None
}
