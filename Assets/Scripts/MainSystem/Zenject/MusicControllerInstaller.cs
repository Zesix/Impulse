using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MusicControllerInstaller : MonoInstaller<MusicControllerInstaller>
{

    private AudioSource _audioSource;

    public override void InstallBindings()
    {
        _audioSource = GetComponent<AudioSource>();

        transform.GetComponent<GameObjectContext>().Container.BindInstance(_audioSource);
    }
}
