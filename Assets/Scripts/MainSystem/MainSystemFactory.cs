using UnityEngine;

/// <summary>
///     A Singleton Factory that creates the MainSystem and wires it up for use.
/// </summary>
/// <remarks>
///     Attach this to a prefab. You do not have to instantiate the prefab to use the factory.
/// </remarks>
public class MainSystemFactory : MonoBehaviour
{

    [SerializeField]
    private GameObject _mainSystemPrefab;

    [SerializeField]
    private GameObject _sceneManagerPrefab;

    [SerializeField]
    private GameObject _musicManagerPrefab;

    private static MainSystem _mainSystem;

    /// <summary>
    /// 	Returns a singletone instance of MainManagers. Creates a new one if necessary.
    /// </summary>
    public MainSystem CreateSingletonMainSystem()
    {
        // Enforce Singleton Factory
        if (_mainSystem != null)
        {
            return _mainSystem;
        }

        // Create main system
        var mainObject = Instantiate(_mainSystemPrefab);
        _mainSystem = mainObject.GetComponent<MainSystem>();

        // Create scene manager
        var sceneObject = Instantiate(_sceneManagerPrefab);
        var sceneManager = sceneObject.GetComponent<GameSceneManager>();

        // Create music manager
        var musicObject = Instantiate(_musicManagerPrefab);
        var musicManager = musicObject.GetComponent<MusicManager>();

        // Initialize main system
        _mainSystem.Initialize(sceneManager, musicManager);

        return _mainSystem;
    }
}
