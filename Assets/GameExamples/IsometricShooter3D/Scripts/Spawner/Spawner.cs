using UnityEngine;
using System.Collections;

namespace IsometricShooter3D
{

    public class Spawner : MonoBehaviour
    {
        // Enemy to spawn.
        [SerializeField] private EnemyModel _enemyToSpawn;
        public EnemyModel EnemyToSpawn => _enemyToSpawn;

        // Parent transform for spawned enemies.
        [SerializeField] private Transform _enemySpawnParent;

        // Number of enemies remaining to spawn.
        [SerializeField] private int _enemiesRemainingToSpawn;

        // Time until we spawn the next enemy.
        private float _nextSpawnTime;

        // The current wave.
        private Wave _currentWave;
        [SerializeField] private int _currentWaveNumber;

        // Array of Waves to spawn.
        [SerializeField] private Wave[] _waves;

        // The color a tile flashes when we spawn an enemy on that tile.
        [SerializeField] private Color _spawningFlashColor = Color.red;

        // How long a tile flashes before the enemy is spawned.
        [SerializeField] private float _spawnDelay = 1f;

        // How many times per second a tile flashes when an enemy is being spawned on it.
        [SerializeField] private float _spawningFlashSpeed = 4f;

        // Ref to map generator. Used to determine where we can spawn.
        private SquareTileMapGenerator _generator;

        // Ref to GameController.
        private GameController _gameController;

        private void OnEnable()
        {
            this.AddObserver(OnNextWaveNotification, GameplayState.NextWaveNotification);
        }

        private void OnDisable()
        {
            this.RemoveObserver(OnNextWaveNotification, GameplayState.NextWaveNotification);
        }

        private void Awake()
        {
            _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
            _generator = FindObjectOfType<SquareTileMapGenerator>();
        }

        public void OnNextWaveNotification(object sender, object args)
        {
            if (_enemySpawnParent == null)
                _enemySpawnParent = new GameObject("Enemy Spawn Parent").transform;

            NextWave();
        }

        private void Update()
        {
            SpawnEnemy();
        }

        private void SpawnEnemy()
        {
            if (_enemiesRemainingToSpawn > 0 && Time.time > _nextSpawnTime)
            {
                _enemiesRemainingToSpawn--;
                _nextSpawnTime = Time.time + _currentWave.TimeBetweenSpawns;

                StartCoroutine(SpawnEnemyUnit());
            }
        }

        private void NextWave()
        {
            _currentWaveNumber++;
            if (_currentWaveNumber - 1 < _waves.Length)
            {
                _currentWave = _waves[_currentWaveNumber - 1];

                _enemiesRemainingToSpawn = _currentWave.EnemyCountToSpawn;
            }
        }

        private IEnumerator SpawnEnemyUnit()
        {
            var spawnTile = _generator.GetRandomUnoccupiedTile();

            // If player is camping, spawn on top of the player instead of a random tile.
            if (_gameController.PlayerIsCamping)
            {
                spawnTile = _generator.GetTransformFromPosition(_gameController.Player.transform.position);
            }

            var tileMat = spawnTile.GetComponent<Renderer>().material;
            var originalColor = tileMat.color;
            var spawnTimer = 0f;

            // Make the spawning tile flash.
            while (spawnTimer < _spawnDelay)
            {
                tileMat.color = Color.Lerp(originalColor, _spawningFlashColor, Mathf.PingPong(spawnTimer * _spawningFlashSpeed, 1f));
                spawnTimer += Time.deltaTime;
                yield return null;
            }

            var spawnedEnemy = Instantiate(_enemyToSpawn, spawnTile.position + Vector3.up, Quaternion.identity);
            spawnedEnemy.transform.parent = _enemySpawnParent;

            _gameController.ModifyEnemyCount(1);
        }
    }
}