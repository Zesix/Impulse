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
            // Zenject based singletons (objects in need of reference injection)
            if (singleton.GetComponent<GameObjectContext>() != null)
            {
                if(singleton.GetComponent<AudioSource>() != null)
                    Container.Bind<AudioSource>().FromSubContainerResolve().ByNewPrefab(singleton).AsSingle().NonLazy();
            }
            // Regular based singletons
            else
            {
                GameObject instancedSingleton = Instantiate(singleton);
                instancedSingleton.transform.parent = this.transform;
                instancedSingleton.transform.localPosition = Vector3.zero;
                instancedSingleton.transform.localRotation = Quaternion.identity;
                instancedSingleton.transform.localScale = Vector3.one;
            }
        }
    }


}
