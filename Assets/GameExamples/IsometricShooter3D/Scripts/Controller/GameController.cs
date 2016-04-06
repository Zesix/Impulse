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
            this.AddObserver(OnGameOver, GameplayState.GameOverNotification);
        }

        void OnDisable()
        {
            this.RemoveObserver(OnGameOver, GameplayState.GameOverNotification);
        }

        void OnGameOver(object sender, object args)
        {

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
