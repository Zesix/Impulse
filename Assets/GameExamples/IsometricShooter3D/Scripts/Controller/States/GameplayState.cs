using UnityEngine;
using System.Collections;

namespace IsometricShooter3D
{
    public class GameplayState : ExtendedState
    {
        #region Properties
        // Start next wave notification.
        public const string NextWaveNotification = "GameplayState.NextWaveNotification";

        // Game over notification.
        public const string GameOverNotification = "GameplayState.GameOverNotification";
        #endregion

        void OnEnable()
        {
            this.AddObserver(OnCharacterDeath, CharacterModel.CharacterDeathNotification);
        }

        void OnDisable()
        {
            this.RemoveObserver(OnCharacterDeath, CharacterModel.CharacterDeathNotification);
        }

        void OnCharacterDeath(object sender, object args)
        {
            if (sender is EnemyModel)
            {
                controller.ModifyEnemyCount(-1);

                // If all enemies are dead, start the next wave.
                if (controller.EnemiesAlive == 0)
                    this.PostNotification(NextWaveNotification);
            }
            else if (sender is PlayerModel)
            {
                // If the player has died, the game is over.
                this.PostNotification(GameOverNotification);
            }
        }

        public override void Enter()
        {
            base.Enter();
            this.PostNotification(NextWaveNotification);
            controller.NextCampCheckTime = controller.TimeBetweenCampingChecks + Time.time;
            controller.CampPositionOld = controller.Player.transform.position;
        }

        void Update()
        {

        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
