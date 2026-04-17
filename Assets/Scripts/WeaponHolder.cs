using System;
using System.Collections.Generic;
using MaiNull.Item;
using UnityEngine;

namespace MaiNull
{
    public class WeaponHolder : MonoBehaviour
    {
        [Header("Weapon Inventory Settings")]
        [SerializeField] private int inventorySize = 1;
        [SerializeField] private List<Weapon> weapons = new List<Weapon>();
        private int _inventoryIndex = 0;
        
        public event Action<Weapon> OnWeaponPickup;
        public event Action OnWeaponReload;
        public event Action OnWeaponDrop;
        public event Action OnWeaponChange;

        public Weapon CurrentWeapon
        {
            get => weapons[_inventoryIndex];
            private set => weapons[_inventoryIndex] = value;
        }

        public void IncreaseIndex() => _inventoryIndex++;
        
        public  void DecreaseIndex() => _inventoryIndex--;
        
        public virtual void PickUpWeapon(Weapon newWeapon)
        {
            if (inventorySize > weapons.Count + 1)
            {
                ChangeWeapon(newWeapon);
                return;
            }
            
            weapons.Add(newWeapon);
            OnWeaponPickup?.Invoke(newWeapon);
            Debug.Log($"{transform.name} picked weapon");
        }

        private void ChangeWeapon(Weapon newWeapon)
        {
            CurrentWeapon = newWeapon;
            OnWeaponChange?.Invoke();
        }

        public virtual void ReloadWeapon()
        {
            CurrentWeapon.CurrentAmmo = CurrentWeapon.WeaponData.maxAmmo;
            OnWeaponReload?.Invoke();
            Debug.Log($"{transform.name} reloaded weapon");
        }

        protected virtual void DropWeapon()
        {
            weapons.RemoveAt(_inventoryIndex);
            OnWeaponDrop?.Invoke();
            Debug.Log($"{transform.name} dropped weapon");
        }
    }
}