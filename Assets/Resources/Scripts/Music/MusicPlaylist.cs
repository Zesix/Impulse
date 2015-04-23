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

public class MusicPlaylist : MonoBehaviour {
	public bool ActivateOnAwake = true;
	public AudioClip[] MusicList;

	void Awake() {
		if (ActivateOnAwake && MusicManager.Instance)
			MusicManager.Instance.ChangePlaylist (this);
	}
	
	void Start () {
		// Have playlist persist across scenes.
		DontDestroyOnLoad (gameObject); // Don't destroy this object

		// When a new scene is loaded, destroy the other playlists.
		foreach (MusicPlaylist playlist in GameObject.FindObjectsOfType<MusicPlaylist>()) {
			if (playlist.name != this.name) {
				Destroy (playlist.gameObject);
			}
		}
	}

}
