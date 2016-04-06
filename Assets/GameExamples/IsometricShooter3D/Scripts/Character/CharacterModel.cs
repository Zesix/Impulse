using UnityEngine;
using System.Collections;

namespace IsometricShooter3D
{

    public class CharacterModel : MonoBehaviour, IDamageable
    {
        #region Properties
        // Are we dead?
        [SerializeField]
        protected bool dead;
        public bool Dead
        {
            get { return dead; }
        }

        // Starting health.
        [SerializeField]
        protected float startingHealth;
        public float StartingHealth
        { get { return startingHealth; } }

        // Current health.
        [SerializeField]
        protected float health;
        public float Health
        {
            get { return health; }
        }

        // Death event.
        public const string CharacterDeathNotification = "CharacterModel.DeathNotification";
        #endregion

        protected virtual void Start()
        {
            health = startingHealth;
        }

        public void TakeHit(float damage, RaycastHit hit)
        {
            TakeDamage(damage);
        }

        public void TakeDamage(float damage)
        {
            health -= damage;

            if (health <= 0 && !dead)
            {
                Die();
            }
        }

        public void Die()
        {
            dead = true;

            // Publish death event.
            this.PostNotification(CharacterDeathNotification);

            GameObject.Destroy(gameObject);
        }
    }
}
