using System;
using UnityEngine;

namespace MaiNull.Item
{
    public class Weapon
    {
        public WeaponData WeaponData { get; private set; }

        public RaycastHit hit;
        protected float nextTimeToFire = 0;
        private int currentAmmo = 0;

        public int CurrentAmmo
        {
            get
            {
                return currentAmmo;
            }

            set
            {
                currentAmmo = Mathf.Max(value, 0);
            }

        }

        public virtual bool Shoot(Transform raycastPos, Action<RaycastHit> hitEvent = null)
        {
            if (CurrentAmmo == 0 || !(Time.time > nextTimeToFire)) return false;

            // Firerate calculation
            nextTimeToFire = Time.time + (1f / WeaponData.firerate);

            CurrentAmmo -= 1;

            if (Physics.Raycast(raycastPos.position, raycastPos.forward, out hit, 1000, WeaponData.shootMask))
            {
                Debug.DrawLine(raycastPos.position, hit.point, Color.green, 1f);

                IDamageable[] damageables = hit.transform.GetComponents<IDamageable>();

                if (damageables.Length != 0)
                {
                    foreach (IDamageable damageable in damageables)
                    {
                        damageable.Damage(WeaponData.damage, new Knockback(WeaponData.knockbackForce, WeaponData.knockbackDuration, hit.transform.position - raycastPos.transform.position), null);
                    }

                    hitEvent?.Invoke(hit);
                    //AddForceToRbs(hit.transform, raycastPos, SOWeapon.bulletForce);
                }
            }
            else
            {
                Debug.DrawRay(raycastPos.position, raycastPos.forward, Color.red, 1f);
            }

            return true;
        }

        protected void AddForceToRbs(Transform hitTransform, Transform directionForce, float forceAmount)
        {
            hitTransform.TryGetComponent(out Rigidbody rb);

            if (rb)
            {
                rb.AddForce(directionForce.forward * forceAmount, ForceMode.Impulse);
            }
        }
    }
}

