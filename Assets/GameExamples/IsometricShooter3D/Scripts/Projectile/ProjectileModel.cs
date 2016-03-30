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
        public float MoveDistance
        {
            get { return moveDistance; }
        }

        // Collision mask. Used in collision detection.
        [SerializeField]
        LayerMask collisionMask;
        public LayerMask CollisionMask
        {
            get { return collisionMask; }
        }
        #endregion

        /// <summary>
        /// Sets the speed the projectile flies forward.
        /// </summary>
        /// <param name="amount">The speed as a float.</param>
        public void SetSpeed(float amount)
        {
            speed = amount;
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

            if (Physics.Raycast(ray, out hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide))
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
    }
}