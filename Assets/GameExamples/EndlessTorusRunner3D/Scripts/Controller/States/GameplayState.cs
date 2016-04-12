using UnityEngine;
using Impulse;
using System.Collections;

namespace EndlessTorusRunner3D
{
    public class GameplayState : ExtendedState
    {
        #region Properties
        // Gameplay Started notification.
        public const string GameplayStateStartedNotification = "GameplayState.StartedNotification";
        #endregion

        public override void Enter()
        {
            base.Enter();

            StartCoroutine(InitGameplay());
        }

        public override void Exit()
        {
            base.Exit();
        }

        IEnumerator InitGameplay()
        {
            controller.GameOver = false;

            // Hide mouse cursor during gameplay.
            Cursor.visible = false;

            // Enable the gameplay interface.
            controller.ScreenManager.gameObject.SetActive(true);

            // Clear any previous screen.
            controller.ScreenManager.ClearScreen();

            // Show gameplay UI.
            controller.ScreenManager.ChangeScreenAndFade(controller.ScreenManager.GameplayUI);

            // Hide the menu.
            controller.MenuSystem.gameObject.SetActive(false);

            // Wait for fading out to complete before respawning pipe system.
            while (GameSceneManager.Instance.transitionPercent <= 0.5f)
            {
                yield return null;
            }

            // Hide main camera since we use a separate camera for gameplay.
            controller.MainCamera.gameObject.SetActive(false);

            // Destroy previous pipes.
            controller.DestroyPreviousPipes();
            // Initialize the Pipe System.
            controller.PipeSystem.Init();
            // Set up first pipe.
            controller.CurrentPipe = controller.PipeSystem.SetupFirstPipe();
            // Set up current pipe.
            controller.PipeSystem.OrientWorld(controller.CurrentPipe);

            // Show the runner.
            controller.RunnerModel.gameObject.SetActive(true);

            // Post gamestarted notification.
            this.PostNotification(GameplayStateStartedNotification);
        }
    }
}
