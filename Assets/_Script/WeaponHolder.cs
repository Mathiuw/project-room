using UnityEngine;

namespace MaiNull.Item
{
    public class WeaponHolder : MonoBehaviour
    {
        [Header("Weapon settings")]
        [SerializeField] private Weapon currentWeapon;

        public Weapon CurrentWeapon { get; private set; }

        public virtual void PickUpWeapon(Weapon newWeapon)
        {
            currentWeapon = newWeapon;
            Debug.Log($"{transform.name} picked weapon");
        }

        public virtual void ReloadWeapon()
        {
            currentWeapon.CurrentAmmo = currentWeapon.WeaponData.maxAmmo;
            Debug.Log($"{transform.name} reloaded weapon");
        }

        public virtual void DropWeapon()
        {
            currentWeapon = null;
            Debug.Log($"{transform.name} dropped weapon");
        }
    }
}