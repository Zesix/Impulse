using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MusicControllerInstaller : MonoInstaller<MusicControllerInstaller>
{
    public override void InstallBindings()
    {
        transform.GetComponent<GameObjectContext>().Container.BindInstance(GetComponent<AudioSource>());
    }
}
