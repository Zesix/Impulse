using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Impulse
{
    /// <summary>
    /// Use this class for initialization of core systems (core singleton service instantiation)
    /// </summary>
    public class ZenjectMainSystemStartup : MonoInstaller
    {
        [SerializeField] [Tooltip("List of singleton services to instantiate")]
        private List<GameObject> _singletonsToSpawn = new List<GameObject>();

        /// <summary>
        /// Use this to install the Zenject bindings
        /// </summary>
        public override void InstallBindings()
        {
            foreach (var singleton in _singletonsToSpawn)
            {
                // Zenject based singletons (objects in need of reference injection)
                if (singleton.GetComponent<GameObjectContext>() != null)
                {
                    Container.InstantiatePrefab(singleton);
                }
                // Regular based singletons
                else
                {
                    var instancedSingleton = Instantiate(singleton);
                    instancedSingleton.transform.parent = transform;
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
            LocalPlayerProfileService.Instance.Initialize();

            // Load player profile
            LocalPlayerProfileService.Instance.LoadData();

            // Use presentation data
            PresentationService.Instance.Initialize();
        }
    }
}
