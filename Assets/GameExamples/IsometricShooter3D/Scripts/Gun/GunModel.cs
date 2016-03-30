using UnityEngine;
using System.Collections;

namespace IsometricShooter3D
{
    public class GunModel : MonoBehaviour
    {
        #region Properties
        // The muzzle where projectiles instantiate from.
        [SerializeField]
        Transform muzzle;
        public Transform Muzzle
        {
            get { return muzzle; }
            set { muzzle = value; }
        }

        // The current projectile being fired.
        [SerializeField]
        ProjectileModel projectile;
        public ProjectileModel Projectile
        {
            get { return projectile; }
            set { projectile = value; }
        }

        // The milliseconds between shots.
        [SerializeField]
        float msBetweenShots = 100f;
        public float MSBetweenShots
        {
            get { return msBetweenShots; }
            set { msBetweenShots = value; }
        }

        // The speed the projectile leaves the gun.
        [SerializeField]
        float muzzleVelocity = 35f;
        public float MuzzleVelocity
        {
            get { return muzzleVelocity; }
            set { muzzleVelocity = value; }
        }

        // The parent transform of all projectiles.
        [SerializeField]
        Transform projectileParent;
        public Transform ProjectileParent
        {
            get { return projectileParent; }
        }

        // Timer for tracking duration between shots.
        float nextShotTime;

        #endregion

        void Start()
        {
            // Create a parent gameobject for our projectiles if one does not exist.
            if (projectileParent == null)
                projectileParent = new GameObject("Projectile Parent").transform;
        }

        /// <summary>
        /// Shoot a projectile.
        /// </summary>
        public void Shoot()
        {
            if (Time.time > nextShotTime)
            {
                // Add time.
                nextShotTime = Time.time + msBetweenShots / 1000;

                // Fire projectile.
                ProjectileModel newProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation) as ProjectileModel;
                newProjectile.transform.parent = projectileParent;
                newProjectile.SetSpeed(muzzleVelocity);
            }
        }
    }
}