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

public class BaseGameManager : MonoBehaviour
{

    bool paused;

    public virtual void PlayerLostHealth()
    {
        // deal with player health lost (update U.I. etc.)
    }

    public virtual void SpawnPlayer()
    {
        // spawn the player
    }

    public virtual void Respawn()
    {
        // respawn the player, possibly in a respawn location
    }

    public virtual void StartGame()
    {
        // handle start of game
    }

    public virtual void RestartCurrentLevel()
    {
        // Reload current scene.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public virtual void EnemyDestroyed(Vector3 aPosition, int pointsValue, int hitByID)
    {
        // handle what happens (score update, etc.) when an enemy is destroyed
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
                // pause time
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