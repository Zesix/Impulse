using UnityEngine;
using System.Collections;

namespace IsometricShooter3D
{

    public class ExtendedState : State
    {
        // Ref to our game controller.
        protected GameController controller;

        void Awake()
        {
            controller = GetComponent<GameController>();
        }
    }
}
