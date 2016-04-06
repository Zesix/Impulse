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

                EnemyModel spawnedEnemy = Instantiate(enemyToSpawn, Vector3.zero, Quaternion.identity) as EnemyModel;
                spawnedEnemy.transform.parent = enemySpawnParent;

                gameController.ModifyEnemyCount(1);
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
    }
}