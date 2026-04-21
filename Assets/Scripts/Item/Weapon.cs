using System;
using UnityEngine;

namespace MaiNull.Item
{
    [Serializable]
    public class Weapon
    {
        public Weapon(WeaponData weaponData)
        {
            WeaponData = weaponData;
            CurrentAmmo = weaponData.maxAmmo;
        }

        public WeaponData WeaponData { get; }
        
        public int CurrentAmmo
        {
            get => CurrentAmmo;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));
                CurrentAmmo = Mathf.Max(value, 0);
            }
        }

        private RaycastHit _hit;
        private float _nextTimeToFire = 0;

        public event Action<Weapon, RaycastHit> OnWeaponShot;


        public virtual bool Shoot(Transform raycastStartPosition)
        {
            if (CurrentAmmo == 0 || !(Time.time > _nextTimeToFire)) return false;

            // Firerate calculation
            _nextTimeToFire = Time.time + (1f / WeaponData.fireRate);

            CurrentAmmo -= 1;

            if (Physics.Raycast(raycastStartPosition.position, raycastStartPosition.forward, out _hit, 1000, WeaponData.ShootMask))
            {
                Debug.DrawLine(raycastStartPosition.position, _hit.point, Color.green, 1f);

                IDamageable[] damageables = _hit.transform.GetComponents<IDamageable>();

                if (damageables.Length != 0)
                {
                    foreach (IDamageable damageable in damageables)
                    {
                        damageable.Damage(WeaponData.damage, new Knockback(WeaponData.knockbackForce, WeaponData.knockbackDuration, _hit.transform.position - raycastStartPosition.transform.position), null);
                    }
                }
            }
            else
            {
                Debug.DrawRay(raycastStartPosition.position, raycastStartPosition.forward, Color.red, 1f);
            }

            OnWeaponShot?.Invoke(this, _hit);

            return true;
        }
    }
}

