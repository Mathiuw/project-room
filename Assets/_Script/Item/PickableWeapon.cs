using UnityEngine;

namespace MaiNull.Item
{
    public class PickableWeapon : Pickable
    {
        [SerializeField] private WeaponData weaponData;
        private Weapon Weapon;

        private void Awake()
        {
            if (weaponData)
            {
                Weapon = new(weaponData);
            }
        }

        public override string readName => weaponData?.itemName;

        public override void Interact(Transform interactor)
        {
            interactor.TryGetComponent(out WeaponHolder weaponHolder);

            if (weaponHolder)
            {
                weaponHolder.PickUpWeapon(Weapon);
                Destroy(gameObject);
            }
        }
    }
}