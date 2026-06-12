using UnityEngine;
using UnityEngine.Serialization;

namespace MaiNull
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Weapon Data")]
    public class WeaponData : ItemBaseData
    {
        [Header("Weapon Stats")]
        public int damage;
        public int maxAmmo;
        [Tooltip("Higher is faster!")] public float fireRate;
        public float knockbackForce;
        public float knockbackDuration;
        public EShootType shootType;
        public EAmmoType ammoType;
        public bool canDrop = true;
        
        [Header("Input")]
        public EWeaponInputType inputType;
            
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