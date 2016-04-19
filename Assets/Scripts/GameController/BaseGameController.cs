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
using UnityEngine.SceneManagement;
using System.Collections;

public class BaseGameController : StateMachine
{
    #region Properties
    [SerializeField]
    protected bool paused;
    #endregion

    // Paused notification.
    public const string GameControllerPausedNotification = "GameController.PausedNotification";

    public virtual void Start()
    {
        // Begin initial game setup state. You should define this per your own project requirements.
        // ChangeState<InitState>();
    }

    public virtual void RestartCurrentLevel()
    {
        // Reload current scene.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public bool Paused
    {
        get
        {
            // get paused
            return paused;
        }
        set
        {
            // set paused 
            paused = value;

            if (paused)
            {
                // Post the paused notification and then pause the game.
                this.PostNotification(GameControllerPausedNotification);
                Time.timeScale = 0f;
            }
            else
            {
                // unpause Unity
                Time.timeScale = 1f;
            }
        }
    }

}