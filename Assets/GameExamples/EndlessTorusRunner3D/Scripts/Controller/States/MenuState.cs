using UnityEngine;
using System.Collections;

namespace EndlessTorusRunner3D
{
    public class MenuState : ExtendedState
    {
        public override void Enter()
        {
            base.Enter();

            controller.DestroyPreviousPipes();
            controller.GameOver = false;
            // Hide the gameplay interface.
            controller.ScreenManager.gameObject.SetActive(false);
            // Hide the runner.
            controller.RunnerModel.gameObject.SetActive(false);
            // Enable main camera.
            controller.MainCamera.gameObject.SetActive(true);
            // Show the main menu.
            controller.MenuSystem.gameObject.SetActive(true);
            // Initialize the Pipe System.
            controller.PipeSystem.Init();
            // Set up first pipe.
            controller.CurrentPipe = controller.PipeSystem.SetupFirstPipe();
            // Set up current pipe.
            controller.PipeSystem.OrientWorld(controller.CurrentPipe);
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
