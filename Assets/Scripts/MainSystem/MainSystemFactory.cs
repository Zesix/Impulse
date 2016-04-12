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

namespace Impulse
{
	using UnityEngine;
	using System.Collections;

	/// <summary>
	///     A Singleton Factory that creates the MainSystem and wires it up for use.
	/// </summary>
	/// <remarks>
	///     Attach this to a prefab. You do not have to instantiate the prefab to use the factory.
	/// </remarks>
	public class MainSystemFactory : MonoBehaviour
	{

		[SerializeField]
		private GameObject _mainSystemPrefab;

		[SerializeField]
		private GameObject _sceneManagerPrefab;

		[SerializeField]
		private GameObject _musicManagerPrefab;

		private static MainSystem _mainSystem;

		/// <summary>
		/// 	Returns a singletone instance of MainManagers. Creates a new one if necessary.
		/// </summary>
		public MainSystem CreateSingletonMainSystem()
		{
			// Enforce Singleton Factory
			if (_mainSystem != null)
			{
				return _mainSystem;
			}

			// Create main system
			GameObject mainObject = Instantiate (_mainSystemPrefab);
			_mainSystem = mainObject.GetComponent<MainSystem>();

			// Create scene manager
			GameObject sceneObject = Instantiate (_sceneManagerPrefab);
			GameSceneManager sceneManager = sceneObject.GetComponent<GameSceneManager>();

			// Create music manager
			GameObject musicObject = Instantiate (_musicManagerPrefab);
			MusicManager musicManager = musicObject.GetComponent<MusicManager>();

			// Initialize main system
			_mainSystem.Initialize (sceneManager, musicManager);

			return _mainSystem;
		}
	}

}