using UnityEngine;
using UnityEngine.Serialization;

namespace MaiNull
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
        public bool canDrop = true;
        
        [Header("Crosshair")]
        public Sprite crosshair;

        [Header("Camera Shake")]
        public float intensity;
        public float speed;

        [Header("Reload")]
        [FormerlySerializedAs("reloadTime")] public float reloadDuration;

        [Header("Drop")]
        public GameObject dropPrefab;
        
        [Header("UI")]
        public Sprite ammoSprite;
    }
}