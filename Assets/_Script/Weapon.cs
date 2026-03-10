using System;
using UnityEngine;
using MaiNull.Interact;
using MaiNull.Item;

public class Weapon : MonoBehaviour, IInteractable, IUIName
{
    [Header("Weapon Scriptable object")]
    [field: SerializeField] public WeaponData WeaponData { get; private set; }

    [Header("Particles")]
    [field: SerializeField] Transform muzzleFlashTransform;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] ParticleSystem blood;

    public RaycastHit hit;
    protected AudioSource gunSound;
    protected float nextTimeToFire = 0;

    public int Ammo { get; private set; } = 0;

    public Transform owner { get; private set; }

    public string ReadName => WeaponData.itemName;

    private void Awake()
    {
        // Set hold state to false
        SetHoldState(false);

        // Set ammo to max
        AddAmmo(WeaponData.maxAmmo);

        gunSound = GetComponent<AudioSource>();
    }

    public void Interact(Transform interactor)
    {
        interactor.TryGetComponent(out WeaponInteraction weaponInteraction);

        if (weaponInteraction)
        {
            weaponInteraction.StartCoroutine(weaponInteraction.PickUpWeapon(this));
        }
    }

    public void AddAmmo(int amount)
    {
        Ammo += amount;
        Ammo = Mathf.Clamp(Ammo, 0, WeaponData.maxAmmo);
    }

    public void RemoveAmmo(int amount)
    {
        Ammo -= amount;
        Ammo = Mathf.Clamp(Ammo, 0, WeaponData.maxAmmo);
    }
 
    public void SetHoldState(bool hasOwner, Transform owner = null) 
    {
        this.owner = owner;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = hasOwner;

        if (hasOwner)
        {
            rb.interpolation = RigidbodyInterpolation.None;
        }
        else 
        {
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            transform.SetParent(null);
        }

        Collider[] colliders = GetComponentsInChildren<Collider>();
        for (int i = 0; i < colliders.Length; i++) colliders[i].isTrigger = hasOwner;
    }

    public virtual bool Shoot(Transform raycastPos, Action<RaycastHit> hitEvent = null)
    {
        if (Ammo == 0 || !(Time.time > nextTimeToFire)) return false;

        // Firerate calculation
        nextTimeToFire = Time.time + (1f / WeaponData.firerate);

        PlayGunSound();
        PlayMuzzleFlashParticle();
        RemoveAmmo(1);

        if (Physics.Raycast(raycastPos.position, raycastPos.forward, out hit, 1000, WeaponData.shootMask))
        {
            Debug.DrawLine(raycastPos.position, hit.point, Color.green, 1f);

            IDamageable[] damageables = hit.transform.GetComponents<IDamageable>();

            if (damageables.Length != 0)
            {
                foreach (IDamageable damageable in damageables)
                {
                    damageable.Damage(WeaponData.damage, owner);
                }

                hitEvent?.Invoke(hit);
                PlayBloodParticle();
                //AddForceToRbs(hit.transform, raycastPos, SOWeapon.bulletForce);
            }
        }
        else 
        {
            Debug.DrawRay(raycastPos.position, raycastPos.forward, Color.red, 1f);
        }

        return true;
    }

    protected void PlayGunSound()
    {
        gunSound.Play();
    }

    protected void AddForceToRbs(Transform hitTransform, Transform directionForce, float forceAmount)
    {
        hitTransform.TryGetComponent(out Rigidbody rb);

        if (rb) 
        {
            rb.AddForce(directionForce.forward * forceAmount, ForceMode.Impulse);
        }                         
    }

    protected void PlayMuzzleFlashParticle()
    {
        Instantiate(muzzleFlash, muzzleFlashTransform.position, muzzleFlashTransform.rotation, transform);
    }

    protected void PlayBloodParticle()
    {
        ParticleSystem particleSystem = Instantiate(blood, hit.point, Quaternion.identity, hit.transform);
        particleSystem.transform.forward = hit.normal;
    }
}