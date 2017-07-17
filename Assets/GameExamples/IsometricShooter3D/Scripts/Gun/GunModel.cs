using UnityEngine;

namespace IsometricShooter3D
{
    public class GunModel : MonoBehaviour
    {
        // The muzzle where projectiles instantiate from.
        [SerializeField] private Transform _muzzle;
        public Transform Muzzle
        {
            get { return _muzzle; }
            set { _muzzle = value; }
        }

        // The current projectile being fired.
        [SerializeField] private ProjectileModel _projectile;
        public ProjectileModel Projectile
        {
            get { return _projectile; }
            set { _projectile = value; }
        }

        // The milliseconds between shots.
        [SerializeField] private float _msBetweenShots = 100f;
        public float MsBetweenShots
        {
            get { return _msBetweenShots; }
            set { _msBetweenShots = value; }
        }

        // The speed the projectile leaves the gun.
        [SerializeField] private float _muzzleVelocity = 35f;
        public float MuzzleVelocity
        {
            get { return _muzzleVelocity; }
            set { _muzzleVelocity = value; }
        }

        // The parent transform of all projectiles.
        [SerializeField] private Transform _projectileParent;
        public Transform ProjectileParent => _projectileParent;

        // Timer for tracking duration between shots.
        private float _nextShotTime;

        private void Start()
        {
            // Create a parent gameobject for our projectiles if one does not exist.
            if (_projectileParent == null)
                _projectileParent = new GameObject("Projectile Parent").transform;
        }

        /// <summary>
        /// Shoot a projectile.
        /// </summary>
        public void Shoot()
        {
            if (Time.time > _nextShotTime)
            {
                // Add time.
                _nextShotTime = Time.time + _msBetweenShots / 1000;

                // Fire projectile.
                var newProjectile = Instantiate(_projectile, _muzzle.position, _muzzle.rotation);
                newProjectile.gameObject.AddComponent<AutoDestroyDelay>();
                newProjectile.transform.parent = _projectileParent;
                newProjectile.SetSpeed(_muzzleVelocity);
            }
        }
    }
}