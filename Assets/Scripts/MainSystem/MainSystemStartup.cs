using UnityEngine;

/// <summary>
///     A class that creates MainManagers on awake.
/// </summary>
public class MainSystemStartup : MonoBehaviour
{

    [Tooltip("The factory to construct the MainManagers.")]
    [SerializeField]
    private MainSystemFactory _mainSystemFactory;

    public void Start()
    {
        // Create MainManagers
        _mainSystemFactory.CreateSingletonMainSystem();

        // Self destruct.
        Destroy(gameObject);
    }
}
