using UnityEngine;
using System.Collections;

namespace IsometricShooter3D
{

    public class Spawner : MonoBehaviour
    {
        #region Properties
        // Enemy to spawn.
        [SerializeField]
        EnemyModel enemyToSpawn;
        public EnemyModel EnemyToSpawn
        {
            get { return enemyToSpawn; }
        }

        // Parent transform for spawned enemies.
        [SerializeField]
        Transform enemySpawnParent;

        // Number of enemies remaining to spawn.
        [SerializeField]
        int enemiesRemainingToSpawn;

        // Time until we spawn the next enemy.
        float nextSpawnTime;

        // The current wave.
        Wave currentWave;
        [SerializeField]
        int currentWaveNumber;

        // Array of Waves to spawn.
        [SerializeField]
        Wave[] waves;

        // The color a tile flashes when we spawn an enemy on that tile.
        [SerializeField]
        Color spawningFlashColor = Color.red;

        // How long a tile flashes before the enemy is spawned.
        [SerializeField]
        float spawnDelay = 1f;

        // How many times per second a tile flashes when an enemy is being spawned on it.
        [SerializeField]
        float spawningFlashSpeed = 4f;

        // Ref to map generator. Used to determine where we can spawn.
        SquareTileMapGenerator generator;

        // Ref to GameController.
        GameController gameController;
        #endregion

        void OnEnable()
        {
            this.AddObserver(OnNextWaveNotification, GameplayState.NextWaveNotification);
        }

        void OnDisable()
        {
            this.RemoveObserver(OnNextWaveNotification, GameplayState.NextWaveNotification);
        }

        void Awake()
        {
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
            generator = FindObjectOfType<SquareTileMapGenerator>();
        }

        public void OnNextWaveNotification(object sender, object args)
        {
            if (enemySpawnParent == null)
                enemySpawnParent = new GameObject("Enemy Spawn Parent").transform;

            NextWave();
        }

        void Update()
        {
            SpawnEnemy();
        }

        void SpawnEnemy()
        {
            if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
            {
                enemiesRemainingToSpawn--;
                nextSpawnTime = Time.time + currentWave.TimeBetweenSpawns;

                StartCoroutine(SpawnEnemyUnit());
            }
        }

        void NextWave()
        {
            currentWaveNumber++;
            if (currentWaveNumber - 1 < waves.Length)
            {
                currentWave = waves[currentWaveNumber - 1];

                enemiesRemainingToSpawn = currentWave.EnemyCountToSpawn;
            }
        }

        IEnumerator SpawnEnemyUnit()
        {
            Transform spawnTile = generator.GetRandomUnoccupiedTile();

            // If player is camping, spawn on top of the player instead of a random tile.
            if (gameController.PlayerIsCamping)
            {
                spawnTile = generator.GetTransformFromPosition(gameController.Player.transform.position);
            }

            Material tileMat = spawnTile.GetComponent<Renderer>().material;
            Color originalColor = tileMat.color;
            float spawnTimer = 0f;

            // Make the spawning tile flash.
            while (spawnTimer < spawnDelay)
            {
                tileMat.color = Color.Lerp(originalColor, spawningFlashColor, Mathf.PingPong(spawnTimer * spawningFlashSpeed, 1f));
                spawnTimer += Time.deltaTime;
                yield return null;
            }

            EnemyModel spawnedEnemy = Instantiate(enemyToSpawn, spawnTile.position + Vector3.up, Quaternion.identity) as EnemyModel;
            spawnedEnemy.transform.parent = enemySpawnParent;

            gameController.ModifyEnemyCount(1);
        }
    }
}