using UnityEngine;

namespace IsometricShooter3D
{
    public class ProjectileModel : MonoBehaviour
    {
        // Amount of damage this projectile does.
        [SerializeField] private float _damage = 1f;
        public float Damage => _damage;

        // The speed this projectile moves forward.
        [SerializeField] private float _speed = 10f;
        public float Speed => _speed;

        // The distance this projectile has moved.
        [SerializeField] private float _moveDistance;

        // Collision mask. Used in collision detection.
        [SerializeField] private LayerMask _collisionMask;

        // A generic short distance used to fix edge cases where collisions may not be detected.
        private const float SkinWidth = 0.1f;

        /// <summary>
        /// Sets the speed the projectile flies forward.
        /// </summary>
        /// <param name="amount">The speed as a float.</param>
        public void SetSpeed(float amount)
        {
            _speed = amount;
        }

        private void Start()
        {
            // Check for initial collisions. This allows us to register a hit even if we start inside another object.
            var initialCollisions = Physics.OverlapSphere(transform.position, SkinWidth, _collisionMask);

            if (initialCollisions.Length > 0)
            {
                OnHitObject(initialCollisions[0]);
            }
        }

        private void Update()
        {
            MoveForward();
            CheckCollisions(_moveDistance);
        }

        private void MoveForward()
        {
            _moveDistance = _speed * Time.deltaTime;
            transform.Translate(Vector3.forward * _moveDistance);
        }

        private void CheckCollisions(float moveDistance)
        {
            var ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, moveDistance + SkinWidth, _collisionMask, QueryTriggerInteraction.Collide))
            {
                OnHitObject(hit);
            }
        }

        private void OnHitObject(RaycastHit hit)
        {
            var damageableObject = hit.collider.GetComponent <IDamageable>();
            damageableObject?.TakeHit(_damage, hit);

            Destroy(gameObject);
        }

        private void OnHitObject(Component c)
        {
            var damageableObject = c.GetComponent<IDamageable>();
            damageableObject?.TakeDamage(_damage);

            Destroy(gameObject);
        }
    }
}