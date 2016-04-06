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
            // Initial setup here.
            controller.ModifyEnemyCount(0);

            yield return null;

            controller.ChangeState<GameplayState>();

        }
    }
}