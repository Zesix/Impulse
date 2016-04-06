using UnityEngine;
using System.Collections;

namespace IsometricShooter3D
{
    public class ProjectileModel : MonoBehaviour
    {
        #region Properties

        // Amount of damage this projectile does.
        [SerializeField]
        float damage = 1f;
        public float Damage
        {
            get { return damage; }
        }

        // The speed this projectile moves forward.
        [SerializeField]
        float speed = 10f;
        public float Speed
        {
            get { return speed; }
        }

        // The distance this projectile has moved.
        [SerializeField]
        float moveDistance;

        // Collision mask. Used in collision detection.
        [SerializeField]
        LayerMask collisionMask;

        // A generic short distance used to fix edge cases where collisions may not be detected.
        float skinWidth = 0.1f;
        #endregion

        /// <summary>
        /// Sets the speed the projectile flies forward.
        /// </summary>
        /// <param name="amount">The speed as a float.</param>
        public void SetSpeed(float amount)
        {
            speed = amount;
        }

        void Start()
        {
            // Check for initial collisions. This allows us to register a hit even if we start inside another object.
            Collider[] initialCollisions = Physics.OverlapSphere(transform.position, skinWidth, collisionMask);

            if (initialCollisions.Length > 0)
            {
                OnHitObject(initialCollisions[0]);
            }
        }

        void Update()
        {
            MoveForward();
            CheckCollisions(moveDistance);
        }

        void MoveForward()
        {
            moveDistance = speed * Time.deltaTime;
            transform.Translate(Vector3.forward * moveDistance);
        }

        void CheckCollisions(float moveDistance)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, moveDistance + skinWidth, collisionMask, QueryTriggerInteraction.Collide))
            {
                OnHitObject(hit);
            }
        }

        void OnHitObject(RaycastHit hit)
        {
            IDamageable damageableObject = hit.collider.GetComponent <IDamageable>();
            if (damageableObject != null)
                damageableObject.TakeHit(damage, hit);

            GameObject.Destroy(gameObject);
        }

        void OnHitObject(Collider c)
        {
            IDamageable damageableObject = c.GetComponent<IDamageable>();
            if (damageableObject != null)
                damageableObject.TakeDamage(damage);

            GameObject.Destroy(gameObject);
        }
    }
}