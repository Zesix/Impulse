using UnityEngine;

/// <summary>
/// Attach to any object or prefab that should be a projectile.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class Projectile : MonoBehaviour
{

    public float Damage;
    public Faction.Factions Faction;
    public AudioClip ShootFx;
    [Range(0, 1)]
    public float ShootFxVolume = 0.5f;
    protected AudioSource myAudio;

    private void OnEnable()
    {
        myAudio = GetComponent<AudioSource>();
    }

    public void PlayShotFx()
    {
        myAudio.PlayOneShot(ShootFx, ShootFxVolume);
    }

    /// <summary>
    /// Deacivates this gameobject
    /// </summary>
    public void Deactivate()
    {
        // This will be replaced with gameObject.SetActive(False) when the pooling system is implemented
        Destroy(gameObject);
    }
}
