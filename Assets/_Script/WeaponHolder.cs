using System;
using UnityEngine;

namespace MaiNull.Item
{
    public class WeaponHolder : MonoBehaviour
    {
        [Header("Weapon settings")]
        [SerializeField] private Weapon currentWeapon;

        public event Action<Weapon> OnWeaponPickup;
        public event Action OnWeaponReload;
        public event Action OnWeaponDrop;

        public Weapon CurrentWeapon { get; private set; }

        public virtual void PickUpWeapon(Weapon newWeapon)
        {
            currentWeapon = newWeapon;
            OnWeaponPickup?.Invoke(newWeapon);
            Debug.Log($"{transform.name} picked weapon");
        }

        public virtual void ReloadWeapon()
        {
            currentWeapon.CurrentAmmo = currentWeapon.WeaponData.maxAmmo;
            OnWeaponReload?.Invoke();
            Debug.Log($"{transform.name} reloaded weapon");
        }

        public virtual void DropWeapon()
        {
            currentWeapon = null;
            OnWeaponDrop?.Invoke();
            Debug.Log($"{transform.name} dropped weapon");
        }
    }
}