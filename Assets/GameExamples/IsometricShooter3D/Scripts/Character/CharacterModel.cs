using UnityEngine;

namespace IsometricShooter3D
{

    public class CharacterModel : MonoBehaviour, IDamageable
    {
        #region Properties
        // Are we dead?
        [SerializeField]
        protected bool dead;
        public bool Dead => dead;

        // Starting health.
        [SerializeField]
        protected float startingHealth;
        public float StartingHealth => startingHealth;

        // Current health.
        [SerializeField]
        protected float health;
        public float Health => health;

        // Damage event.
        public const string CharacterDamagedNotification = "CharacterModel.DamageNotification";

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
            else
            {
                this.PostNotification(CharacterDamagedNotification);
            }
        }

        public void Die()
        {
            dead = true;

            // Publish death event.
            this.PostNotification(CharacterDeathNotification);

            Destroy(gameObject);
        }
    }
}
