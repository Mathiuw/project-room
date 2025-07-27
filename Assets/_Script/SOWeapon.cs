using UnityEngine;

public enum EShootType { Single, Automatic }

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon")]
public class SOWeapon : ScriptableObject
{
    [Header("Weapon")]
    public string weaponName;
    public int damage;
    public float bulletForce;
    public int maxAmmo;
    public float firerate;
    public bool waitToShoot;
    public EShootType shootType;
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