/*****************************************
 * This file is part of Impulse Framework.

    Impulse Framework is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    any later version.

    Impulse Framework is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Impulse Framework.  If not, see <http://www.gnu.org/licenses/>.
*****************************************/

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
    public float WeaponColdown = 1.0f;
    [System.NonSerialized]
    public FiringMode fireMode;
    public bool firing = false;

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
    protected bool relativeMovement = false;

    // Debug variables
    [SerializeField]
    protected bool debugRay = false;

    virtual protected void Start()
    {
        myAudio = GetComponent<AudioSource>();
        fireMode = FiringMode.Single;
    }

    virtual public void SetWeaponFaction(Faction.Factions faction)
    {
        Faction = faction;
    }

    virtual public void StopFire()
    {
        firing = false;
    }

    /// <summary>
    /// Fire projectile
    /// </summary>
    virtual public void Fire(float speed, Vector3 direction)
    {
        // Fire a projecitle from each mount point
        for (int i = 0; i < gunMounts.Length; i++)
            FireMountPoint(gunMounts[i], speed, direction, i);

        // Execute weapon coldown
        ExecuteWeaponColdown();
    }

    virtual protected void ExecuteWeaponColdown()
    {
        // Execute coldown
        StopCoroutine("StartWeaponColdown");
        StartCoroutine("StartWeaponColdown");
    }

    IEnumerator StartWeaponColdown()
    {
        // Set weapon coldown
        firing = true;

        // Wait fire delay
        yield return new WaitForSeconds(WeaponColdown);

        // Stop firing and allow further fire
        StopFire();
    }

    /// <summary>
    /// Fires a projectile from the selected mount point
    /// </summary>
    virtual protected void FireMountPoint(Transform mountPoint, float speed, Vector3 direction, int MountPointIndex)
    {
        // Create bullet
        Rigidbody shot;
        shot = ((GameObject)Instantiate(projectile, mountPoint.position, mountPoint.rotation)).GetComponent<Rigidbody>();

        // Initialize bullet
        Projectile proj = shot.GetComponent<Projectile>();
        if (proj != null)
        {
            // Set projectile faction
            proj.Faction = Faction;

            // Play projectile shoot effect if it is available
            if (proj.shootFX != null)
                proj.PlayShotFX();
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