using UnityEngine;
using System.Collections;

namespace EndlessTorusRunner3D
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
