using System.Collections;
using UnityEngine;

public class EnemyWeaponInteraction : WeaponInteraction, IDead
{
    private void Start()
    {
        if (Weapon && !Weapon.owner)
        {
            StartCoroutine(PickUpWeapon(Weapon));
        }
    }

    public override IEnumerator PickUpWeapon(Weapon weapon)
    {
        Weapon = weapon;

        // Set weapon transform in the weapon container
        weapon.transform.SetParent(weaponContainer, false);
        weapon.transform.position = Vector3.zero;
        weapon.transform.rotation = Quaternion.Euler(Vector3.zero);
        weapon.transform.localScale = Vector3.one;

        weapon.SetHoldState(true, transform);

        Debug.Log(transform.name + " picked up gun");

        yield break;
    }

    public override IEnumerator ReloadWeapon()
    {
        if (!Weapon) yield break;

        Weapon.AddAmmo(Weapon.SOWeapon.maxAmmo);
        
        yield break;
    }

    public override void DropWeapon()
    {
        Weapon.SetHoldState(false, null);
        Weapon = null;

        Debug.Log(name + " dropped weapon");
    }

    public void Dead()
    {
        DropWeapon();
    }
}
