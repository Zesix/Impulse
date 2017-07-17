using UnityEngine;

namespace IsometricShooter3D
{

    public class GameController : BaseGameController
    {
        // Map generator.
        [SerializeField] private SquareTileMapGenerator _mapGenerator;
        public SquareTileMapGenerator MapGenerator => _mapGenerator;

        // Interface Manager.
        [SerializeField] private InterfaceManager _hudManager;
        public InterfaceManager HudManager => _hudManager;

        // Reference to our player.
        [SerializeField] private PlayerModel _player;
        public PlayerModel Player => _player;

        // Used to check if the player is camping in a spot.
        [SerializeField] private float _campingThresholdDistance = 1.5f;
        public float CampingThresholdDistance => _campingThresholdDistance;

        [SerializeField] private float _timeBetweenCampingChecks = 2f;
        public float TimeBetweenCampingChecks
        {
            get { return _timeBetweenCampingChecks; }
            set { _timeBetweenCampingChecks = value; }
        }

        public float NextCampCheckTime { get; set; }

        public bool PlayerIsCamping { get; set; }

        public Vector3 CampPositionOld { get; set; }

        // Number of enemies alive.
        [SerializeField] private int _enemiesAlive;
        public int EnemiesAlive => _enemiesAlive;

        private bool _gamePlaying;

        private void OnEnable()
        {
            this.AddObserver(OnGameOverNotification, GameplayState.GameOverNotification);
            this.AddObserver(OnNextWaveNotification, GameplayState.NextWaveNotification);
        }

        private void OnDisable()
        {
            this.RemoveObserver(OnGameOverNotification, GameplayState.GameOverNotification);
            this.RemoveObserver(OnNextWaveNotification, GameplayState.NextWaveNotification);
        }

        private void OnGameOverNotification(object sender, object args)
        {
            _gamePlaying = false;

            // Show game over screen.
            _hudManager.ChangeScreen(_hudManager.GameOverUi);
        }

        private void OnNextWaveNotification (object sender, object args)
        {
            // Show gameplay UI at the start of the first wave.
            if (_gamePlaying == false)
            {
                _gamePlaying = true;
                // Show gameplay UI.
                _hudManager.ChangeScreen(_hudManager.GameplayUi);
            }

            // Reset player transform.
            _player.transform.position = new Vector3(0,1,0);
        }

        private void Update()
        {
            if (_gamePlaying)
            {
                if (Time.time > NextCampCheckTime)
                {
                    NextCampCheckTime = Time.time + _timeBetweenCampingChecks;
                    PlayerIsCamping = (Vector3.Distance(_player.transform.position, CampPositionOld) < _campingThresholdDistance);
                    CampPositionOld = _player.transform.position;
                }
            }
        }

        public virtual void Start()
        {
            ChangeState<InitState>();
        }

        public void ModifyEnemyCount(int amount)
        {
            _enemiesAlive += amount;
        }
    }
}
