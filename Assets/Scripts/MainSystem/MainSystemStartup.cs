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
	///     A class that creates MainManagers on awake.
	/// </summary>
	public class MainSystemStartup : MonoBehaviour {

		[Tooltip("The factory to construct the MainManagers.")]
		[SerializeField]
		private MainSystemFactory _mainSystemFactory;

		public void Start ()
		{
			// Create MainManagers
			_mainSystemFactory.CreateSingletonMainSystem ();

			// Self destruct.
			Destroy (gameObject);
		}
	}
}
