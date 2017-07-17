using UnityEngine;

namespace EndlessTorusRunner3D
{
    public class GameOverState : ExtendedState
    {
        public override void Enter()
        {
            base.Enter();
            controller.GameOver = true;
            controller.GameplayMode = false;

            // Enable main camera again.
            controller.MainCamera.gameObject.SetActive(true);

            // Show game over menu.
            controller.ScreenManager.ChangeScreen(controller.ScreenManager.GameOverUi);

            // Show mouse cursor.
            Cursor.visible = true;
        }
    }
}
