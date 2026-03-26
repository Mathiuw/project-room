using System;
using UnityEngine;

namespace MaiNull.Item
{
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
            set => CurrentAmmo = Mathf.Max(value, 0); 
        }

        protected RaycastHit hit;
        protected float nextTimeToFire = 0;

        public event Action<Weapon, RaycastHit> OnWeaponShot;


        public virtual bool Shoot(Transform raycastStartPosition)
        {
            if (CurrentAmmo == 0 || !(Time.time > nextTimeToFire)) return false;

            // Firerate calculation
            nextTimeToFire = Time.time + (1f / WeaponData.firerate);

            CurrentAmmo -= 1;

            if (Physics.Raycast(raycastStartPosition.position, raycastStartPosition.forward, out hit, 1000, WeaponData.shootMask))
            {
                Debug.DrawLine(raycastStartPosition.position, hit.point, Color.green, 1f);

                IDamageable[] damageables = hit.transform.GetComponents<IDamageable>();

                if (damageables.Length != 0)
                {
                    foreach (IDamageable damageable in damageables)
                    {
                        damageable.Damage(WeaponData.damage, new Knockback(WeaponData.knockbackForce, WeaponData.knockbackDuration, hit.transform.position - raycastStartPosition.transform.position), null);
                    }
                }
            }
            else
            {
                Debug.DrawRay(raycastStartPosition.position, raycastStartPosition.forward, Color.red, 1f);
            }

            OnWeaponShot?.Invoke(this, hit);

            return true;
        }
    }
}

