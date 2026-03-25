using MaiNull.Item;
using UnityEngine;

public class EnemyWeaponInteraction : WeaponHolder
{
    private void Start()
    {
        //if (CurrentWeapon && !CurrentWeapon.Owner)
        //{
        //    SetCurrentWeaponTransform();
        //}
    }

    public override void PickUpWeapon(Weapon newWeapon)
    {
        base.PickUpWeapon(newWeapon);
        //SetCurrentWeaponTransform();
        Debug.Log(transform.name + " picked up gun");
    }

    //private void SetCurrentWeaponTransform()
    //{
    //    //newWeapon.transform.SetParent(weaponContainer, false);
    //    CurrentWeapon.transform.SetPositionAndRotation(Vector3.zero, Quaternion.Euler(Vector3.zero));
    //    CurrentWeapon.transform.localScale = Vector3.one;
    //    CurrentWeapon.SetHoldState(true, transform);
    //}

    public override void ReloadWeapon()
    {
        if (CurrentWeapon != null) return;

        CurrentWeapon.CurrentAmmo += CurrentWeapon.WeaponData.maxAmmo;
    }

    public override void DropWeapon()
    {
        //CurrentWeapon.SetHoldState(false, null);
        base.DropWeapon();
    }

    public void Dead()
    {
        DropWeapon();
    }
}
