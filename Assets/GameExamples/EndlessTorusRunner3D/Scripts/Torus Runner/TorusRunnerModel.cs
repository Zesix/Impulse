using UnityEngine;
using System.Collections;

namespace EndlessTorusRunner3D
{
    public class TorusRunnerModel : MonoBehaviour
    {
        #region Properties
        [SerializeField]
        float forwardVelocity;
        public float ForwardVelocity
        {
            get { return forwardVelocity; }
        }

        [SerializeField]
        float rotationVelocity;
        public float RotationVelocity
        {
            get { return rotationVelocity; }
            set { rotationVelocity = value; }
        }

        [SerializeField]
        Transform rotater;
        float avatarRotation;

        // Distance the runner has traveled.
        [SerializeField]
        float distanceTraveled;
        public float DistanceTraveled
        {
            get { return distanceTraveled; }
        }

        // References.
        GameController controller;
        TorusRunnerView view;

        // Is the model movable? This prevents the player from moving the model after the death animation begins.
        bool movable = true;
        public bool Movable
        {
            get { return movable; }
            set { movable = value; }
        }
        #endregion

        // Listeners
        void OnEnable()
        {
            this.AddObserver(OnGameplayStartedNotification, GameplayState.GameplayStateStartedNotification);
        }

        void OnDisable()
        {
            this.RemoveObserver(OnGameplayStartedNotification, GameplayState.GameplayStateStartedNotification);
        }

        void Awake()
        {
            view = GetComponent<TorusRunnerView>();
        }

        // This is basically our Start() function since we don't want to do anything until the game has started.
        void OnGameplayStartedNotification(object sender, object args)
        {
            GameplayState gameplayState = sender as GameplayState;
            controller = gameplayState.GetComponent<GameController>();

            // Reset our distance traveled.
            distanceTraveled = 0f;

            // Show our runner object.
            view.ToggleRunnerObject(true);

            // Make the runner movable again.
            movable = true;
        }

        void Update()
        {
            // Gameplay has not started if controller has not been assigned.
            if (controller != null)
            {
                // Update distance traveled every second unless the game is paused or the game is over.
                if (controller.GameplayMode == true && controller.Paused == false && controller.GameOver == false)
                {
                    distanceTraveled += forwardVelocity * Time.deltaTime;
                }
            }
        }

        public void RotateAvatar(float rotation)
        {
            if (movable)
            {
                avatarRotation += rotationVelocity * Time.deltaTime * rotation;
                if (avatarRotation < 0f)
                {
                    avatarRotation += 360f;
                }
                else if (avatarRotation >= 360f)
                {
                    avatarRotation -= 360f;
                }
                rotater.localRotation = Quaternion.Euler(avatarRotation, 0f, 0f);
            }
        }

        public void Die()
        {
            // Hide the player object.
            gameObject.SetActive(false);
        }
    }
}
