using UnityEngine;
using System.Collections;

namespace IsometricShooter3D
{
    public class InitState : ExtendedState
    {

        public override void Enter()
        {
            base.Enter();
            StartCoroutine(Init());
        }

        IEnumerator Init()
        {
            // Hide UI screens.
            controller.HUDManager.ClearScreen();

            // Set initial enemy count to zero and then generate the map.
            controller.ModifyEnemyCount(0);
            controller.MapGenerator.GenerateMap();

            yield return null;

            controller.ChangeState<GameplayState>();

        }
    }
}