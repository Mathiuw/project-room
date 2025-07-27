using System;
using UnityEngine;

public class Weapon : MonoBehaviour, IInteractable, IUIName
{
    [Header("Weapon Scriptable object")]
    [field: SerializeField] public SOWeapon SOWeapon { get; private set; }

    [Header("Particles")]
    [field: SerializeField] Transform muzzleFlashTransform;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] ParticleSystem blood;

    public RaycastHit hit;
    protected AudioSource gunSound;
    protected float nextTimeToFire = 0;

    public int Ammo { get; private set; } = 0;

    public Transform owner { get; private set; }

    public string ReadName => SOWeapon.weaponName;

    private void Awake()
    {
        // Set hold state to false
        SetHoldState(false);

        // Set ammo to max
        AddAmmo(SOWeapon.maxAmmo);

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
        Ammo = Mathf.Clamp(Ammo, 0, SOWeapon.maxAmmo);
    }

    public void RemoveAmmo(int amount)
    {
        Ammo -= amount;
        Ammo = Mathf.Clamp(Ammo, 0, SOWeapon.maxAmmo);
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
        nextTimeToFire = Time.time + (1f / SOWeapon.firerate);

        PlayGunSound();
        PlayMuzzleFlashParticle();
        RemoveAmmo(1);

        if (Physics.Raycast(raycastPos.position, raycastPos.forward, out hit, 1000, SOWeapon.shootMask))
        {
            Debug.DrawLine(raycastPos.position, hit.point, Color.green, 1f);

            hit.transform.TryGetComponent(out Health health);
            hit.transform.TryGetComponent(out EnemyAi enemyAi);

            if (enemyAi && owner != null) enemyAi.Target = owner;

            if (health)
            {
                if (health.Dead) 
                {
                    AddForceToRbs(hit.transform, raycastPos, SOWeapon.bulletForce);
                }
                else
                {
                    health.RemoveHealth(SOWeapon.damage);
                    hitEvent?.Invoke(hit);
                }

                PlayBloodParticle();        
            }
            else
            {
                AddForceToRbs(hit.transform, raycastPos, SOWeapon.bulletForce);
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