using UnityEngine;

namespace IsometricShooter3D
{
    public class GunController : BaseInputController
    {
        // The weapon slot.
        [SerializeField] private Transform _weaponSlot;
        public Transform WeaponSlot
        {
            get { return _weaponSlot; }
            set { _weaponSlot = value; }
        }

        // Starting gun. Used only if assigned.
        [SerializeField] private GunModel _startingGun;
        public GunModel StartingGun
        {
            get { return _startingGun; }
            set { _startingGun = value; }
        }

        // The currently equipped gun.
        private GunModel _equippedGun;

        private void OnEnable()
        {
            FireEvent += OnFireEvent;
        }

        private void OnDisable()
        {
            FireEvent -= OnFireEvent;
        }

        private void OnFireEvent(object sender, InfoEventArgs<int> e)
        {
            if (e.Info == 0)
            {
                _equippedGun.Shoot();
            }
        }

        private void Start()
        {
            if (_startingGun != null)
                EquipGun(_startingGun);
        }

        public void EquipGun (GunModel gunToEquip)
        {
            // If there already is an equipped gun, then destroy it.
            if (_equippedGun != null)
                Destroy(_equippedGun.gameObject);

            _equippedGun = Instantiate(gunToEquip, _weaponSlot.position, _weaponSlot.rotation);
            _equippedGun.transform.parent = _weaponSlot;
        }
    }
}