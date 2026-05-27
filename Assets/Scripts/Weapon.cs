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

        public event Action<Weapon, RaycastHit> OnWeaponShot;
        
        public WeaponData WeaponData { get; }
        
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
            WeaponData = weaponData;
            CurrentAmmo = weaponData.maxAmmo;
        }

        public virtual bool Shoot(Transform raycastStartPosition, LayerMask layerMask)
        {
            if (CurrentAmmo == 0 || !(Time.time > _nextTimeToFire)) return false;

            // FireRate calculation
            _nextTimeToFire = Time.time + (1f / WeaponData.fireRate);

            CurrentAmmo -= 1;

            if (Physics.Raycast(raycastStartPosition.position, raycastStartPosition.forward, out _hit, 1000, layerMask))
            {
                Debug.DrawLine(raycastStartPosition.position, _hit.point, Color.green, 1f);

                IDamageable[] damageables = _hit.transform.GetComponents<IDamageable>();

                if (damageables.Length != 0)
                {
                    foreach (IDamageable damageable in damageables)
                    {
                        damageable.Damage(WeaponData.damage, 
                            new Knockback(WeaponData.knockbackForce, WeaponData.knockbackDuration, _hit.transform.position - raycastStartPosition.transform.position), null);
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

