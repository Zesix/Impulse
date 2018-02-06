using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
/// <summary>
/// Use this class for initialization of core systems (core singleton manager instatiation)
/// </summary>
public class ZenjectMainSystemStartup : MonoInstaller
{
    [SerializeField]
    [Tooltip("List of singletons to instantiate")]
    private List<GameObject> _singletonsToSpawn = new List<GameObject>();

    /// <summary>
    /// Use this to install the zenject bindings
    /// </summary>
    public override void InstallBindings()
    {
        foreach (GameObject singleton in _singletonsToSpawn)
        {
            Container.Bind<AudioSource>().FromSubContainerResolve().ByNewPrefab(singleton).AsSingle().NonLazy();
            // todo: add extra injects here by copying the above line and changing its type
        }
    }


}
