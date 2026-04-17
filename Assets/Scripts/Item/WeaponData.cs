using UnityEngine;
using UnityEngine.Serialization;

namespace MaiNull.Item
{
    public enum EShootType { Single, Automatic, Burst }

    [CreateAssetMenu(fileName = "WeaponData", menuName = "Weapon Data")]
    public class WeaponData : ItemBaseData
    {
        [Header("Weapon Stats")]
        public int damage;
        public float knockbackForce;
        public float knockbackDuration;
        public int maxAmmo;
        [FormerlySerializedAs("firerate")] public float fireRate;
        [FormerlySerializedAs("waitToShoot")] public bool shootCooldown;
        public EShootType shootType = EShootType.Single;
        public EAmmoType ammoType;
        public static LayerMask ShootMask;

        [Header("Crosshair")]
        public Sprite crosshair;

        [Header("Camera Shake")]
        public float intensity;
        public float speed;

        [Header("Animation")]
        public AnimatorOverrideController animatorOverride;

        [Header("Reload")]
        [FormerlySerializedAs("reloadTime")] public float reloadCooldown;

        [Header("UI")]
        public Sprite ammoSprite;
    }
}