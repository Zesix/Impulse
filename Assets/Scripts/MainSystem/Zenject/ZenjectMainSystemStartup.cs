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
        // Deactivate install warning, we don't need to worry about installation order as every singleton initialization should be order independant
        Container.ShouldCheckForInstallWarning = false;

        foreach (GameObject singleton in _singletonsToSpawn)
        {
            // Zenject based singletons (objects in need of reference injection)
            if (singleton.GetComponent<GameObjectContext>() != null)
            {
                Container.InstantiatePrefab(singleton);
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

    /// <summary>
    /// Use this for initialization
    /// </summary>
    public override void Start()
    {
        // Initialize core managers
        SaveManager.Instance.Initialize();

        // Load player profile
        SaveManager.Instance.LoadData();

        // Use presentation data
        PresentationManager.Instance.Initialize();
    }


}
