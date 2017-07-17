using UnityEngine;

namespace SpaceShooter2D
{
    /// <summary>
    /// Destroys a gameObject after a set delay.
    /// </summary>
    public class AutoDestroyDelay : MonoBehaviour
    {
        [SerializeField] private float _delayTime = 1.5f;

        private void Update()
        {
            Destroy(gameObject, _delayTime);
        }
    }
}