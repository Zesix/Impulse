using UnityEngine;

namespace Impulse
{
    /// <summary>
    ///     Instantiates factories at start.
    /// </summary>
    public class MainSystemStartup : MonoBehaviour
    {
        [Tooltip("The factory to construct persistent singleton controllers.")] [SerializeField]
        private MainSystemFactory _mainSystemFactory;

        public void Start()
        {
            // Create MainManagers
            _mainSystemFactory.CreateSingletonMainSystem();

            // Self destruct.
            Destroy(gameObject);
        }
    }
}
