using UnityEngine;

namespace EndlessTorusRunner3D
{
    public class GameController : BaseGameController
    {
        // The pipe system.
        [SerializeField] private PipeSystem _pipeSystem;
        public PipeSystem PipeSystem => _pipeSystem;

        // The current pipe.
        public Pipe CurrentPipe { get; set; }

        // The torus runner.
        [SerializeField] private TorusRunnerModel _runnerModel;
        public TorusRunnerModel RunnerModel => _runnerModel;

        // The menu system.
        [SerializeField] private MenuManager _menuSystem;
        public MenuManager MenuSystem => _menuSystem;

        // The UI manager.
        [SerializeField] private InterfaceManager _screenManager;
        public InterfaceManager ScreenManager => _screenManager;

        // The main camera.
        [SerializeField] private Camera _mainCamera;
        public Camera MainCamera => _mainCamera;

        // Are we in gameplay?
        public bool GameplayMode { get; set; }

        // Is the game over?
        public bool GameOver { get; set; }

        // The player's score.
        public int Score { get; set; }

        private void OnGameStartedNotification(object sender, object args)
        {
            GameplayMode = true;
        }

        private void OnTorusRunnerDeathNotification(object sender, object args)
        {
             ChangeState<GameOverState>();
        }

        private void OnEnable()
        {
            this.AddObserver(OnGameStartedNotification, GameplayState.GameplayStateStartedNotification);
            this.AddObserver(OnTorusRunnerDeathNotification, TorusRunnerView.TorusRunnerDeathNotification);
        }

        private void OnDisable()
        {
            this.RemoveObserver(OnGameStartedNotification, GameplayState.GameplayStateStartedNotification);
            this.RemoveObserver(OnTorusRunnerDeathNotification, TorusRunnerView.TorusRunnerDeathNotification);
        }

        public virtual void Start()
        {
            // Prevent 30FPS limit on mobile devices.
            Application.targetFrameRate = 1000;
            DisplayMenu();
        }

        private void Update()
        {
            UpdatePipeSystemRotation();
            if (GameplayMode && GameOver == false)
            {
                UpdateScore();
            }
        }

        public void StartGame()
        {
            ChangeState<GameplayState>();
        }

        public void DisplayMenu()
        {
            ChangeState<MenuState>();
        }

        /// <summary>
        /// Rotates the pipe position about the origin.
        /// </summary>
        public void UpdatePipeSystemRotation()
        {
            var delta = _runnerModel.ForwardVelocity * Time.deltaTime;
            _pipeSystem.SystemRotation += delta * _pipeSystem.DeltaToRotation;
            if (_pipeSystem.SystemRotation >= CurrentPipe.CurveAngle)
            {
                delta = (_pipeSystem.SystemRotation - CurrentPipe.CurveAngle) / _pipeSystem.DeltaToRotation;
                CurrentPipe = _pipeSystem.SetupNextPipe();
                _pipeSystem.OrientWorld(CurrentPipe);
                _pipeSystem.SystemRotation = delta * _pipeSystem.DeltaToRotation;
            }

            _pipeSystem.transform.localRotation = Quaternion.Euler(0f, 0f, _pipeSystem.SystemRotation);
        }

        /// <summary>
        /// Destroys the previously generated pipes.
        /// </summary>
        public void DestroyPreviousPipes()
        {
            for (var i = 0; i < _pipeSystem.transform.childCount; i++)
            {
                Destroy(_pipeSystem.transform.GetChild(i).gameObject);
            }
        }

        private void UpdateScore()
        {
            Score = (int)_runnerModel.DistanceTraveled;
        }
    }
}
