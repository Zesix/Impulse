using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseGameController : StateMachine
{
    #region Properties
    [SerializeField]
    protected bool paused;
    #endregion

    // Paused notification.
    public const string GameControllerPausedNotification = "GameController.PausedNotification";

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