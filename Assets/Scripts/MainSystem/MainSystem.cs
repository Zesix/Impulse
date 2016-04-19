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

using System;
using UnityEngine;

/// <summary>
///     Contains all game systems.
/// </summary>
/// <remarks>
///     Game contains all game systems and persists accross scene loads.
/// </remarks>
public class MainSystem : MonoBehaviour
{
    private bool _isInitialized;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 	Inects necessary dependencies and initalizes the object.
    /// </summary>
    public void Initialize(GameSceneManager sceneManager, MusicManager musicManager)
    {
        if (_isInitialized)
            throw new InvalidOperationException("Already initialized.");

        if (sceneManager == null)
            throw new ArgumentNullException("sceneManager");

        if (musicManager == null)
            throw new ArgumentNullException("musicManager");

        _isInitialized = true;
    }
}
