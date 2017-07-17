using System;
using UnityEngine;

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
    /// 	Inects necessary dependencies and initalizes the object.
    /// </summary>
    public void Initialize(GameSceneManager sceneManager, MusicManager musicManager)
    {
        if (_isInitialized)
            throw new InvalidOperationException("Already initialized.");

        if (sceneManager == null)
            throw new ArgumentNullException(nameof(sceneManager));

        if (musicManager == null)
            throw new ArgumentNullException(nameof(musicManager));

        _isInitialized = true;
    }
}
