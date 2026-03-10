using System;
using UnityEngine;
using MaiNull.Item;

public class WeaponProjectile : Weapon
{
    [Header("Projectile settings")]
    [SerializeField] private int projectileAmount = 8;

    public override bool Shoot(Transform raycastPos, Action<RaycastHit> hitEvent = null)
    {
        if (Ammo == 0) return false;
        if (!(Time.time > nextTimeToFire)) return false;

        // Firerate calculation
        nextTimeToFire = Time.time + (1f / WeaponData.firerate);
        PlayGunSound();
        PlayMuzzleFlashParticle();
        RemoveAmmo(1);

        //CameraShake.AddCameraShake(soWeapon.intensity, soWeapon.speed);

        // Raycast para checar se atingi algo
        if (Physics.Raycast(raycastPos.position, raycastPos.forward, out hit, 1000, WeaponData.shootMask))
        {
            Debug.DrawLine(raycastPos.position, hit.point, Color.green, 1f);

            hit.transform.TryGetComponent(out Health health);
            hit.transform.TryGetComponent(out EnemyAi enemyAi);

            if (enemyAi && owner != null) enemyAi.Target = owner;

            if (health)
            {
                PlayBloodParticle();
                health.RemoveHealth(WeaponData.damage/projectileAmount);

                if (health.Dead) AddForceToRbs(hit.transform, raycastPos, WeaponData.bulletForce);
            }
            else
            {
                AddForceToRbs(hit.transform, raycastPos, WeaponData.bulletForce);
            }
        }
        else
        {
            Debug.DrawRay(raycastPos.position, raycastPos.forward, Color.red, 1f);
        }

        return true;
    }
}