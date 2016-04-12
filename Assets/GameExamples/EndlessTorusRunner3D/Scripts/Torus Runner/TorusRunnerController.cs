using UnityEngine;
using System.Collections;

namespace EndlessTorusRunner3D
{
    [RequireComponent(typeof(TorusRunnerModel))]
    public class TorusRunnerController : BaseInputController
    {
        #region Properties
        // Ref to our model.
        TorusRunnerModel model;

        // Movement rotation input.
        float rotation;
        #endregion

        void Start()
        {
            model = GetComponent<TorusRunnerModel>();
        }

        void Update()
        {
            CheckInput();
            SendInput();
        }

        public override void CheckInput()
        {
            base.CheckInput();

            rotation = horizontal;

            // Touch input.
            if (Application.isMobilePlatform)
            {
                if (Input.touchCount == 1)
                {
                    if (Input.GetTouch(0).position.x < Screen.width * 0.5f)
                    {
                        rotation = -1f;
                    }
                    else
                    {
                        rotation = 1f;
                    }
                }
            }
        }

        void SendInput()
        {
            model.RotateAvatar(rotation);
        }
    }
}
