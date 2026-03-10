using UnityEngine;


namespace MaiNull.Item
{
    public enum EShootType { Single, Automatic, Burst }

    [CreateAssetMenu(fileName = "WeaponData", menuName = "WeaponData")]
    public class WeaponData : ItemBaseData
    {
        [Header("Weapon")]
        public int damage;
        public float bulletForce;
        public int maxAmmo;
        public float firerate;
        public bool waitToShoot;
        public EShootType shootType = EShootType.Single;
        public EAmmoType ammoType;
        public LayerMask shootMask;

        [Header("Crosshair")]
        public Sprite crosshair;

        [Header("Camera Shake")]
        public float intensity;
        public float speed;

        [Header("Animation")]
        public AnimatorOverrideController animatorOverride;

        [Header("Reload")]
        public float reloadTime;

        [Header("UI")]
        public Sprite ammoSprite;
    }
}