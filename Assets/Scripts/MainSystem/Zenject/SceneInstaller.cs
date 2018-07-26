using UnityEngine;
using Zenject;

/// <summary>
/// Base class used to inject values to a new scene, specific scene installers MUST inherit from this class
/// </summary>
public class SceneInstaller : MonoInstaller<SceneInstaller>
{
    [InjectOptional]
    public string LevelName = "default_level";

    public override void InstallBindings()
    {
    }
}