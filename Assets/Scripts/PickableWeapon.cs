using UnityEngine;

namespace MaiNull
{
    public class PickableWeapon : Pickable
    {
        [SerializeField] private WeaponData weaponData;
        private Weapon _weapon;

        private void Awake()
        {
            if (weaponData)
            {
                _weapon = new Weapon(weaponData);
            }
        }

        public override string readName => weaponData? weaponData.itemName : "WeaponData not found";

        public override void Interact(Transform interactor)
        {
            interactor.TryGetComponent(out WeaponHolder weaponHolder);
            if (!weaponHolder) return;
            
            weaponHolder.PickUpWeapon(_weapon);
            Destroy(gameObject);
        }
    }
}