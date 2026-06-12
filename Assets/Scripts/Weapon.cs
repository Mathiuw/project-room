using System;
using UnityEngine;

namespace MaiNull
{
    [Serializable]
    public class Weapon
    {
        private int _currentAmmo;
        private RaycastHit _hit;
        private float _nextTimeToFire = 0;
        private WeaponData _weaponData;
        
        public event Action<Weapon, RaycastHit> OnWeaponShot;

        public WeaponData WeaponData => _weaponData;
        
        public int CurrentAmmo
        {
            get => _currentAmmo;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));
                _currentAmmo = Mathf.Max(value, 0);
            }
        }
        
        public Weapon(WeaponData weaponData)
        {
            _weaponData = weaponData;
            CurrentAmmo = weaponData.maxAmmo;
        }

        public virtual bool Shoot(Transform raycastStartPosition, LayerMask layerMask, Transform instigator)
        {
            if (CurrentAmmo == 0 || !(Time.time > _nextTimeToFire)) return false;

            // FireRate calculation
            _nextTimeToFire = Time.time + 1f / _weaponData.fireRate;

            CurrentAmmo -= 1;

            if (Physics.Raycast(raycastStartPosition.position, raycastStartPosition.forward, out _hit, 1000, layerMask))
            {
                Debug.DrawLine(raycastStartPosition.position, _hit.point, Color.blue, 1f);

                IDamageable[] damageables = _hit.transform.GetComponents<IDamageable>();

                if (damageables.Length != 0)
                {
                    foreach (IDamageable damageable in damageables)
                    {
                        damageable.Damage(_weaponData.damage, 
                            new Knockback(_weaponData.knockbackForce, 
                                WeaponData.knockbackDuration, 
                                _hit.transform.position - raycastStartPosition.transform.position), 
                            instigator);
                    }
                }
            }
            else
            {
                Debug.DrawRay(raycastStartPosition.position, raycastStartPosition.forward, Color.red, 1f);
            }

            OnWeaponShot?.Invoke(this, _hit);
            Debug.Log($"Shoot {WeaponData.itemName}");
            
            return true;
        }
    }
}

