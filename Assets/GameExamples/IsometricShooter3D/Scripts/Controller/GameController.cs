using UnityEngine;
using System.Collections;

namespace IsometricShooter3D
{

    public class GameController : BaseGameController
    {

        #region Properties
        // Number of enemies alive.
        [SerializeField]
        int enemiesAlive;
        public int EnemiesAlive
        {
            get { return enemiesAlive; }
        }
        #endregion

        void OnEnable()
        {
            this.AddObserver(OnCharacterDeath, CharacterModel.CharacterDeathNotification);
        }

        void OnDisable()
        {
            this.RemoveObserver(OnCharacterDeath, CharacterModel.CharacterDeathNotification);
        }

        void OnCharacterDeath (object sender, object args)
        {
            // TODO Determine if type of character that died is the player.
        }

        public override void Start()
        {
            ChangeState<InitState>();
        }

        public void ModifyEnemyCount(int amount)
        {
            enemiesAlive += amount;
        }
    }
}
