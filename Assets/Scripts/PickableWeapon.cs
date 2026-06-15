using UnityEngine;

namespace MaiNull
{
    public class PickableWeapon : Pickable
    {
        [SerializeField] private WeaponData weaponData;
        public Weapon WeaponInstance { get; set; }

        private void Awake()
        {
            if (weaponData)
            {
                WeaponInstance = new Weapon(weaponData);
            }
        }

        public override string readName => weaponData? weaponData.itemName : "WeaponData not found";

        public override void Interact(Transform interactor)
        {
            interactor.TryGetComponent(out WeaponHolder weaponHolder);
            if (!weaponHolder) return;
            
            weaponHolder.PickUpWeapon(WeaponInstance);
            Destroy(gameObject);
        }
    }
}