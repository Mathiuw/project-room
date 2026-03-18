using System.Collections;
using UnityEngine;

namespace MaiNull
{
    public abstract class WeaponInteraction : MonoBehaviour
    {
        [Header("Weapon settings")]
        [SerializeField] protected Transform weaponContainer;
        [field: SerializeField] public Weapon Weapon { get; set; }

        public abstract IEnumerator PickUpWeapon(Weapon weapon);

        public abstract IEnumerator ReloadWeapon();

        public abstract void DropWeapon();

    }
}