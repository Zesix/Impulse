using UnityEngine;
using System.Collections;

namespace IsometricShooter3D
{
    public class GunController : BaseInputController
    {
        #region Properties
        // The weapon slot.
        [SerializeField]
        private Transform weaponSlot;
        public Transform WeaponSlot
        {
            get { return weaponSlot; }
            set { weaponSlot = value; }
        }

        // Starting gun. Used only if assigned.
        [SerializeField]
        private GunModel startingGun;
        public GunModel StartingGun
        {
            get { return startingGun; }
            set { startingGun = value; }
        }

        // The currently equipped gun.
        GunModel equippedGun;
        #endregion

        void OnEnable()
        {
            fireEvent += OnFireEvent;
        }

        void OnDisable()
        {
            fireEvent -= OnFireEvent;
        }

        void OnFireEvent(object sender, InfoEventArgs<int> e)
        {
            if (e.info == 0)
            {
                equippedGun.Shoot();
            }
        }

        void Start()
        {
            if (startingGun != null)
                EquipGun(startingGun);
        }

        public void EquipGun (GunModel gunToEquip)
        {
            // If there already is an equipped gun, then destroy it.
            if (equippedGun != null)
                Destroy(equippedGun.gameObject);

            equippedGun = Instantiate(gunToEquip, weaponSlot.position, weaponSlot.rotation) as GunModel;
            equippedGun.transform.parent = weaponSlot;
        }
    }
}