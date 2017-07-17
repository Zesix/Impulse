using UnityEngine;

namespace EndlessTorusRunner3D
{
    public class TorusRunnerModel : MonoBehaviour
    {
        [SerializeField] private float _forwardVelocity;
        public float ForwardVelocity => _forwardVelocity;

        [SerializeField] private float _rotationVelocity;
        public float RotationVelocity
        {
            get { return _rotationVelocity; }
            set { _rotationVelocity = value; }
        }

        [SerializeField] private Transform _rotater;
        private float _avatarRotation;

        // Distance the runner has traveled.
        [SerializeField] private float _distanceTraveled;
        public float DistanceTraveled => _distanceTraveled;

        // References.
        private GameController _controller;

        private TorusRunnerView _view;

        // Is the model movable? This prevents the player from moving the model after the death animation begins.
        public bool Movable { get; set; } = true;

        // Listeners
        private void OnEnable()
        {
            this.AddObserver(OnGameplayStartedNotification, GameplayState.GameplayStateStartedNotification);
        }

        private void OnDisable()
        {
            this.RemoveObserver(OnGameplayStartedNotification, GameplayState.GameplayStateStartedNotification);
        }

        private void Awake()
        {
            _view = GetComponent<TorusRunnerView>();
        }

        // This is basically our Start() function since we don't want to do anything until the game has started.
        private void OnGameplayStartedNotification(object sender, object args)
        {
            var gameplayState = sender as GameplayState;
            _controller = gameplayState.GetComponent<GameController>();

            // Reset our distance traveled.
            _distanceTraveled = 0f;

            // Show our runner object.
            _view.ToggleRunnerObject(true);

            // Make the runner movable again.
            Movable = true;
        }

        private void Update()
        {
            // Gameplay has not started if controller has not been assigned.
            if (_controller != null)
            {
                // Update distance traveled every second unless the game is paused or the game is over.
                if (_controller.GameplayMode && _controller.Paused == false && _controller.GameOver == false)
                {
                    _distanceTraveled += _forwardVelocity * Time.deltaTime;
                }
            }
        }

        public void RotateAvatar(float rotation)
        {
            if (Movable)
            {
                _avatarRotation += _rotationVelocity * Time.deltaTime * rotation;
                if (_avatarRotation < 0f)
                {
                    _avatarRotation += 360f;
                }
                else if (_avatarRotation >= 360f)
                {
                    _avatarRotation -= 360f;
                }
                _rotater.localRotation = Quaternion.Euler(_avatarRotation, 0f, 0f);
            }
        }

        public void Die()
        {
            // Hide the player object.
            gameObject.SetActive(false);
        }
    }
}
