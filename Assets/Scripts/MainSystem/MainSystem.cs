using System;
using UnityEngine;

namespace Impulse
{
    /// <summary>
    ///     Contains all game systems.
    /// </summary>
    /// <remarks>
    ///     Game contains all game systems and persists accross scene loads.
    /// </remarks>
    public class MainSystem : MonoBehaviour
    {
        private bool _isInitialized;

        public void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// 	Injects necessary dependencies and initializes the object.
        /// </summary>
		public void Initialize(SceneService sceneController, MusicManager musicManager, JsonService jsonManager)
        {
            if (_isInitialized)
                throw new InvalidOperationException("Already initialized.");

            if (sceneController == null)
                throw new ArgumentNullException(nameof(sceneController));

            if (musicManager == null)
                throw new ArgumentNullException(nameof(musicManager));

			if (jsonManager == null)
				throw new ArgumentNullException(nameof(jsonManager));

            _isInitialized = true;
        }
    }
}
