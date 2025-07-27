using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class PlayerWeaponInteraction : WeaponInteraction, IDead
{
    [Header("Weapon Interation")]
    [SerializeField] float aimTime;

    [Header("Weapon Sway")]
    [SerializeField] float smooth = 8;
    [SerializeField] float swayMultiplier = 4;

    public bool IsReloading { get; private set; } = false;
    public bool isLerping { get; private set; } = false;

    Transform cameraTransform;
    Inventory inventory;

    public event Action<Weapon> onWeaponShot;
    public event Action<Weapon> onWeaponPickup;
    public event Action<RaycastHit> onWeaponHit;
    public event Action onReloadStart;
    public event Action onReloadEnd;
    public event Action onWeaponDrop;

    void Awake() 
    {
        inventory = GetComponent<Inventory>();
    } 

    void Start() 
    {
        CameraPivot cameraPivot = GetComponentInChildren<CameraPivot>();

        if (cameraPivot)
        {
            weaponContainer = cameraPivot.attatchedCamera.GetComponentInChildren<WeaponHolder>().transform;
            cameraTransform = cameraPivot.attatchedCamera.transform;
        } 
    } 

    void Update()
    {
        if (Weapon)
        {
            SwayWeapon(swayMultiplier);

            if (IsReloading) return;
            if (isLerping) return;

            if (InputShoot())
            {
                if (Weapon.Shoot(cameraTransform, onWeaponHit))
                {
                    onWeaponShot?.Invoke(Weapon);
                }                          
            }
        }

        if (Input.GetKeyDown(KeyCode.R)) StartCoroutine(ReloadWeapon());

        if (Input.GetKeyDown(KeyCode.G)) DropWeapon();
    }

    IEnumerator LerpWeaponCoroutine(float time, Transform weapon, Vector3 desiredPosition, Quaternion desiredRotation, Transform parent = null) 
    {
        isLerping = true;

        if (parent != null)
        {
            weapon.SetParent(parent);
            weapon.localScale = Vector3.one;
        }

        float elapsedTime = 0f;
        float percentageComplete = 0f;
        
        Vector3 startPosition = weapon.localPosition;
        Quaternion startRotation = weapon.localRotation;

        while (elapsedTime < time)
        {
            weapon.localPosition = Vector3.Lerp(startPosition, desiredPosition, percentageComplete);
            weapon.localRotation = Quaternion.Lerp(startRotation, desiredRotation, percentageComplete);

            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / time;
            yield return null;
        }

        weapon.localPosition = desiredPosition;
        weapon.localRotation = desiredRotation;
        isLerping = false;
    }

    void SwayWeapon(float swayMultiplier)
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        Weapon.transform.localRotation = Quaternion.Slerp(Weapon.transform.localRotation, targetRotation, smooth * Time.deltaTime);
    }

    public override IEnumerator PickUpWeapon(Weapon weapon)
    {
        if (Weapon) yield break;
        if (isLerping) yield break;
        if (weapon.owner != null)
        {
            Debug.LogError("Gun already Picked up");
            yield break;
        }

        // Set current weapon
        Weapon = weapon;

        // Set weapon hold state
        Weapon.SetHoldState(true, transform);
        
        // Lerp weapon to player
        StartCoroutine(LerpWeaponCoroutine(0.2f, weapon.transform, Vector3.zero, Quaternion.identity, weaponContainer));
        while (isLerping) yield return null;

        onWeaponPickup?.Invoke(Weapon);

        Debug.Log("Picked up weapon");
    }

    bool InputShoot() 
    {
        switch (Weapon.SOWeapon.shootType)
        {
            case EShootType.Single:
                return Input.GetKeyDown(KeyCode.Mouse0);
            case EShootType.Automatic:
                return Input.GetKey(KeyCode.Mouse0);
            default: 
                return false;
        }
    }

    public override IEnumerator ReloadWeapon()  
    {
        if (!Weapon) yield break;
        if (IsReloading) yield break;
        if (Weapon.Ammo == Weapon.SOWeapon.maxAmmo) yield break;
        if (inventory.GetAmmoAmountByType(Weapon.SOWeapon.ammoType) == 0) yield break;

        onReloadStart?.Invoke();

        IsReloading = true;
        yield return new WaitForSeconds(Weapon.SOWeapon.reloadTime);

        EAmmoType ammoType = Weapon.SOWeapon.ammoType;
        int amountToReload = 0;
        int inventoryAmmoAmount = inventory.GetAmmoAmountByType(ammoType);

        // Reload amount logic
        for (int i = Weapon.Ammo; i < Weapon.SOWeapon.maxAmmo; i++)
        {
            if (inventoryAmmoAmount == 0) break;

            inventoryAmmoAmount--;
            amountToReload++;
        }

        Weapon.AddAmmo(amountToReload);
        inventory.RemoveAmmo(ammoType, amountToReload);

        IsReloading = false;
        onReloadEnd?.Invoke();

        yield break;
    }

    public override void DropWeapon()
    {
        if (!Weapon) return;
        if (IsReloading) return;

        StopAllCoroutines();
        onWeaponDrop?.Invoke();

        Transform weaponTransform = Weapon.transform;
        Rigidbody weaponRb = weaponTransform.GetComponent<Rigidbody>();

        Weapon.SetHoldState(false, null);
        weaponTransform.SetParent(null);
        weaponTransform.transform.position = transform.position;
        weaponRb.AddForce(transform.forward * 5, ForceMode.VelocityChange);
        weaponTransform.localScale = Vector3.one;

        isLerping = false;
        IsReloading = false;

        Weapon = null;

        Debug.Log("Dropped weapon");
    }

    public void Dead()
    {
        DropWeapon();
        Destroy(this);
    }
}
