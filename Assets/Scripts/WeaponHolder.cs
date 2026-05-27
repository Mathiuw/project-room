using System;
using System.Collections.Generic;
using UnityEngine;

namespace MaiNull
{
    public class WeaponHolder : MonoBehaviour
    {
        [Header("Weapon Inventory Settings")]
        public Transform shootOrientation;
        [SerializeField] private int inventorySize = 1;
        [SerializeField] private List<Weapon> weapons = new List<Weapon>();
        [SerializeField] private LayerMask shootMask;
        private int _inventoryIndex;
        
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
        
        public void IncreaseInventorySize() => inventorySize += 1;

        public void DecreaseInventorySize()
        {
            inventorySize -= 1;
            inventorySize = Mathf.Max(inventorySize, 1);
        } 

        
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

        public void ChangeWeapon(Weapon newWeapon)
        {
            CurrentWeapon = newWeapon;
            OnWeaponChange?.Invoke();
        }

        public void ShootWeapon(Transform orientation)
        {
            CurrentWeapon?.Shoot(orientation, shootMask);
        }
        
        public void ShootWeapon()
        {
            CurrentWeapon?.Shoot(shootOrientation, shootMask);
        }
        
        public virtual void ReloadWeapon()
        {
            CurrentWeapon.CurrentAmmo = CurrentWeapon.WeaponData.maxAmmo;
            OnWeaponReload?.Invoke();
            Debug.Log($"{transform.name} reloaded weapon");
        }

        public virtual void DropWeapon()
        {
            weapons.RemoveAt(_inventoryIndex);
            OnWeaponDrop?.Invoke();
            Debug.Log($"{transform.name} dropped weapon");
        }
    }
}