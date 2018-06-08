using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SaveControllerInstaller : MonoInstaller<SaveControllerInstaller>
{
    public override void InstallBindings()
    {
        transform.GetComponent<GameObjectContext>().Container.BindInstance(GetComponent<ILocalDataManager>());
    }
}
