using UnityEngine;
using System.Collections;

namespace IsometricShooter3D
{
    [System.Serializable]
    public class Wave
    {
        #region Properties
        // Number of enemies to spawn.
        [SerializeField]
        int enemyCountToSpawn;
        public int EnemyCountToSpawn
        {
            get { return enemyCountToSpawn; }
        }

        // Time between spawns.
        [SerializeField]
        float timeBetweenSpawns;
        public float TimeBetweenSpawns
        {
            get { return timeBetweenSpawns; }
        }
        #endregion
    }
}
