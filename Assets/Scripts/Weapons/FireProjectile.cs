using UnityEngine;
using System.Collections;


/// <summary>
/// Fires a projectile in 3D.
/// </summary>
public class FireProjectile : MonoBehaviour
{
    // Ship parameters
    protected AudioSource myAudio;

    // Shooter Parameters
    [Range(0, 100)]
    public float WeaponCooldown = 1.0f;
    [System.NonSerialized]
    public FiringMode fireMode;
    public bool firing;

    // Where the projectile should spawn from.
    [SerializeField]
    protected Transform[] gunMounts;
    // The projectile prefab.
    [SerializeField]
    protected GameObject projectile;
    // The parent transform for the projectiles.
    [SerializeField]
    protected Transform ProjectilesParent;
    protected Faction.Factions Faction;

    // Relative movement
    [SerializeField]
    protected bool relativeMovement;

    // Debug variables
    [SerializeField]
    protected bool debugRay;

    protected virtual void Start()
    {
        myAudio = GetComponent<AudioSource>();
        fireMode = FiringMode.Single;
    }

    public virtual void SetWeaponFaction(Faction.Factions faction)
    {
        Faction = faction;
    }

    public virtual void StopFire()
    {
        firing = false;
    }

    /// <summary>
    /// Fire projectile
    /// </summary>
    public virtual void Fire(float speed, Vector3 direction)
    {
        // Fire a projecitle from each mount point
        for (var i = 0; i < gunMounts.Length; i++)
            FireMountPoint(gunMounts[i], speed, direction, i);

        // Execute weapon coldown
        ExecuteWeaponCooldown();
    }

    protected virtual void ExecuteWeaponCooldown()
    {
        // Execute coldown
        StopCoroutine(nameof(StartWeaponCooldown));
        StartCoroutine(nameof(StartWeaponCooldown));
    }

    private IEnumerator StartWeaponCooldown()
    {
        // Set weapon cooldown
        firing = true;

        // Wait fire delay
        yield return new WaitForSeconds(WeaponCooldown);

        // Stop firing and allow further fire
        StopFire();
    }

    /// <summary>
    /// Fires a projectile from the selected mount point
    /// </summary>
    protected virtual void FireMountPoint(Transform mountPoint, float speed, Vector3 direction, int mountPointIndex)
    {
        // Create bullet
        var shot = Instantiate(projectile, mountPoint.position, mountPoint.rotation).GetComponent<Rigidbody>();

        // Initialize bullet
        var proj = shot.GetComponent<Projectile>();
        if (proj != null)
        {
            // Set projectile faction
            proj.Faction = Faction;

            // Play projectile shoot effect if it is available
            if (proj.ShootFx != null)
                proj.PlayShotFx();
        }

        // Fire bullet taking into consideration the firing speed (gunMount.up * speed) and 
        // the directional modifier (direction), in most cases the direction should be the ship's momentum
        shot.AddForce(mountPoint.up * speed + direction, ForceMode.Impulse);

        // Set shot parent
        if (ProjectilesParent != null)
            shot.transform.parent = ProjectilesParent;

        // Debug raycasting
        if (debugRay)
            Debug.DrawRay(transform.position, (mountPoint.up) * speed + direction, Color.red, 5.0f);

    }

    /// <summary>
    /// Set firing mode
    /// </summary>
    public enum FiringMode
    {
        Single,
        Continuous
    }
}