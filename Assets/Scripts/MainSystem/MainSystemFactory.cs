using UnityEngine;

namespace Impulse
{
    /// <summary>
    ///     A singleton factory that instantiates persistent controllers.
    /// </summary>
    /// <remarks>
    ///     Attach this to a prefab. You do not have to instantiate the prefab to use the factory.
    /// </remarks>
    public class MainSystemFactory : MonoBehaviour
    {

		[SerializeField] private MainSystem _mainSystemPrefab;

		[SerializeField] private SceneService _sceneManagerPrefab;

		[SerializeField] private MusicManager _musicManagerPrefab;

		[SerializeField] private JsonService _jsonServicePrefab;

		private static MainSystem _mainSystem;

        /// <summary>
        /// 	Returns a singleton instance of MainManagers. Creates a new one if necessary.
        /// </summary>
        public MainSystem CreateSingletonMainSystem()
        {
            // Enforce Singleton Factory
            if (_mainSystem != null)
            {
                return _mainSystem;
            }

            // Create main system
			_mainSystem = Instantiate(_mainSystemPrefab);

            // Create scene manager
            var sceneObject = Instantiate(_sceneManagerPrefab);

            // Create music manager
            var musicObject = Instantiate(_musicManagerPrefab);

			// Create music manager
			var jsonObject = Instantiate(_jsonServicePrefab);

            // Initialize main system
			_mainSystem.Initialize(sceneObject, musicObject,jsonObject);

            return _mainSystem;
        }
    }
}
