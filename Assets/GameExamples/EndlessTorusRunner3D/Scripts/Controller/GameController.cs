using UnityEngine;
using System.Collections;

namespace EndlessTorusRunner3D
{
    public class GameController : BaseGameController
    {
        #region Properties
        // The pipe system.
        [SerializeField]
        PipeSystem pipeSystem;
        public PipeSystem PipeSystem
        {
            get { return pipeSystem; }
        }

        // The current pipe.
        Pipe currentPipe;
        public Pipe CurrentPipe
        {
            get { return currentPipe; }
            set { currentPipe = value; }
        }

        // The torus runner.
        [SerializeField]
        TorusRunnerModel runnerModel;
        public TorusRunnerModel RunnerModel
        {
            get { return runnerModel; }
        }

        // The menu system.
        [SerializeField]
        MenuManager menuSystem;
        public MenuManager MenuSystem
        {
            get { return menuSystem; }
        }

        // The UI manager.
        [SerializeField]
        InterfaceManager screenManager;
        public InterfaceManager ScreenManager
        {
            get { return screenManager; }
        }

        // The main camera.
        [SerializeField]
        Camera mainCamera;
        public Camera MainCamera
        {
            get { return mainCamera; }
        }

        // Are we in gameplay?
        bool gameplayMode = false;
        public bool GameplayMode
        {
            get { return gameplayMode; }
            set { gameplayMode = value; }
        }

        // Is the game over?
        bool gameOver = false;
        public bool GameOver
        {
            get { return gameOver; }
            set { gameOver = value; }
        }

        // The player's score.
        int score;
        public int Score
        {
            get { return score; }
            set { score = value; }
        }
        #endregion

        #region Event Handlers
        void OnGameStartedNotification(object sender, object args)
        {
            gameplayMode = true;
        }

        void OnTorusRunnerDeathNotification(object sender, object args)
        {
             ChangeState<GameOverState>();
        }
        #endregion

        #region Active/Inactive
        void OnEnable()
        {
            this.AddObserver(OnGameStartedNotification, GameplayState.GameplayStateStartedNotification);
            this.AddObserver(OnTorusRunnerDeathNotification, TorusRunnerView.TorusRunnerDeathNotification);
        }

        void OnDisable()
        {
            this.RemoveObserver(OnGameStartedNotification, GameplayState.GameplayStateStartedNotification);
            this.RemoveObserver(OnTorusRunnerDeathNotification, TorusRunnerView.TorusRunnerDeathNotification);
        }
        #endregion

        public override void Start()
        {
            // Prevent 30FPS limit on mobile devices.
            Application.targetFrameRate = 1000;
            DisplayMenu();
        }

        void Update()
        {
            UpdatePipeSystemRotation();
            if (gameplayMode == true && gameOver == false)
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
            float delta = runnerModel.ForwardVelocity * Time.deltaTime;
            pipeSystem.SystemRotation += delta * pipeSystem.DeltaToRotation;
            if (pipeSystem.SystemRotation >= currentPipe.CurveAngle)
            {
                delta = (pipeSystem.SystemRotation - currentPipe.CurveAngle) / pipeSystem.DeltaToRotation;
                currentPipe = pipeSystem.SetupNextPipe();
                pipeSystem.OrientWorld(currentPipe);
                pipeSystem.SystemRotation = delta * pipeSystem.DeltaToRotation;
            }

            pipeSystem.transform.localRotation = Quaternion.Euler(0f, 0f, pipeSystem.SystemRotation);
        }

        /// <summary>
        /// Destroys the previously generated pipes.
        /// </summary>
        public void DestroyPreviousPipes()
        {
            for (int i = 0; i < pipeSystem.transform.childCount; i++)
            {
                Destroy(pipeSystem.transform.GetChild(i).gameObject);
            }
        }

        void UpdateScore()
        {
            score = (int)runnerModel.DistanceTraveled;
        }
    }
}
