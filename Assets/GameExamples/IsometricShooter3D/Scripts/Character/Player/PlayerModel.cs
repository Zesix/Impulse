using UnityEngine;
using System.Collections;

namespace IsometricShooter3D
{
    [RequireComponent (typeof(PlayerController))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerModel : CharacterModel
    {
        #region Properties
        // The speed this character moves.
        [SerializeField]
        private float moveSpeed = 50f;
        public float MoveSpeed
        {
            get { return moveSpeed; }
            set { moveSpeed = value; }
        }

        // This character's velocity.
        [SerializeField]
        private Vector3 velocity;
        public Vector3 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        // References.
        PlayerController controller;
        Rigidbody myRigidbody;
        #endregion

        void Awake()
        {
            // Set this to Player tag so enemy NavMesh will see it as the target.
            transform.tag = "Player";
        }

        protected override void Start()
        {
            base.Start();
            controller = GetComponent<PlayerController>();
            myRigidbody = GetComponent<Rigidbody>();
        }

        // We use fixed update since we are moving a rigidbody.
        void FixedUpdate()
        {
            myRigidbody.MovePosition(myRigidbody.position + velocity * Time.fixedDeltaTime);
        }

        public void Move(Vector3 moveVelocity)
        {
            velocity = moveVelocity.normalized * moveSpeed;
        }
    }
}