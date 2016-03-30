using UnityEngine;
using System.Collections;

namespace IsometricShooter3D
{
    [RequireComponent(typeof(PlayerModel))]
    public class PlayerController : BaseInputController
    {
        // Ref to PlayerModel.
        PlayerModel model;
        // Ref to PlayerView.
        PlayerView view;
        // Ref to main camera.
        Camera viewCamera;

        // Movement input as Vector3.
        Vector3 moveInput;
        // Mouse cursor position.
        Vector3 point;

        void Start()
        {
            model = GetComponent<PlayerModel>();
            view = GetComponent<PlayerView>();
            viewCamera = Camera.main;
        }

        void Update()
        {
            CheckInput();
            SendInput();
        }

        public override void CheckInput()
        {
            base.CheckInput();

            // Get movement input.
            moveInput = new Vector3(horizontal, 0, vertical);

            // Get mouse position.
            Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayDistance;
            if (groundPlane.Raycast(ray, out rayDistance))
            {
                point = ray.GetPoint(rayDistance);
                Debug.DrawLine(ray.origin, point, Color.red);
            }
        }

        void SendInput()
        {
            model.Move(moveInput);
            view.LookAt(point);
        }

    }
}