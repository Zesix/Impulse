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
	/// 	Calls the SceneManager to change scene.
	/// </summary>
	/// <remarks>
	/// 	Attach to a game object in a scene in order to use UGUI OnClick() for changing scenes in the SceneManager.
	/// </remarks>
	public class ChangeScene : MonoBehaviour
	{
		public void LoadScene (int index, bool animate)
		{
			GameSceneManager.Instance.LoadLevelFadeInDelegate (index, animate);
		}

		public void LoadScene (string name, bool animate)
		{
			GameSceneManager.Instance.LoadLevelFadeInDelegate (name);
		}

	}
}
