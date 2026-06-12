using System;
using System.Collections.Generic;
using UnityEngine;

namespace MaiNull
{
    public class WeaponHolder : MonoBehaviour
    {
        [Header("Weapon Inventory Settings")]
        public Transform shootOrientation;
        [SerializeField] private int startInventorySize = 2;
        [SerializeField] private int maxInventorySize = 5;
        [SerializeField] private LayerMask shootMask;
        private readonly List<Weapon> _weapons = new List<Weapon>();
        private int _inventorySize = 1;
        private int _inventoryIndex;
        
        public event Action<Weapon> OnWeaponPickup;
        public event Action OnWeaponReload;
        public event Action OnWeaponDrop;
        public event Action<Weapon> OnWeaponChange;

        public Weapon CurrentWeapon
        {
            get => _weapons.Count <= 0 ? null : _weapons[_inventoryIndex];
            private set => _weapons[_inventoryIndex] = value;
        }

        private void Awake()
        {
            _inventorySize = startInventorySize;
        }

        public void IncreaseIndex()
        {
            _inventoryIndex++;

            if (_inventoryIndex >= _weapons.Count)
            {
                _inventoryIndex = 0;
            }
            
            OnWeaponChange?.Invoke(CurrentWeapon);
        } 

        public void DecreaseIndex()
        {
            _inventoryIndex--;
            if (_inventoryIndex <= 0 && _weapons.Count > 0)
            {
                _inventoryIndex = _weapons.Count - 1;
            }
            else _inventoryIndex = 0;
            
            OnWeaponChange?.Invoke(CurrentWeapon);
        }

        public void IncreaseInventorySize()
        {
            _inventorySize += 1;
            _inventorySize = Mathf.Clamp(_inventorySize, startInventorySize, maxInventorySize);
        } 

        public void DecreaseInventorySize()
        {
            _inventorySize -= 1;
            _inventorySize = Mathf.Clamp(_inventorySize, startInventorySize, maxInventorySize);
        } 
        
        public virtual void PickUpWeapon(Weapon newWeapon)
        {
            if (_weapons.Count + 1 > _inventorySize)
            {
                ChangeWeapon(newWeapon); 
                return;
            }
            
            _weapons.Add(newWeapon);
            OnWeaponPickup?.Invoke(newWeapon);
            print($"{transform.name} picked weapon");
        }

        public void ChangeWeapon(Weapon newWeapon)
        {
            CurrentWeapon = newWeapon;
            OnWeaponChange?.Invoke(CurrentWeapon);
            print($"{transform.name} changed weapon to {newWeapon}");
        }

        public void ShootWeapon(Transform orientation)
        {
            CurrentWeapon?.Shoot(orientation, shootMask, transform);
        }
        
        public void ShootWeapon()
        {
            CurrentWeapon?.Shoot(shootOrientation, shootMask, transform);
        }
        
        public virtual void ReloadCurrentWeapon()
        {
            if (CurrentWeapon == null) return;
            
            CurrentWeapon.CurrentAmmo = CurrentWeapon.WeaponData.maxAmmo;
            OnWeaponReload?.Invoke();
            print($"{transform.name} reloaded weapon");
        }

        public virtual void DropCurrentWeapon()
        {
            if (CurrentWeapon == null || !CurrentWeapon.WeaponData.canDrop ||  _inventoryIndex >= _weapons.Count) return;

            Instantiate(CurrentWeapon.WeaponData.dropPrefab, transform.position, Quaternion.identity);
            
            _weapons.RemoveAt(_inventoryIndex);
            DecreaseInventorySize();
            OnWeaponDrop?.Invoke();
            print($"{transform.name} dropped weapon");
        }
    }
}