using UnityEngine;

namespace MaiNull
{
    [CreateAssetMenu(fileName = "Upgrade_Weapon_Slot", menuName = "Card/Upgrade/Weapon Slot")]
    public class CardDataUpgradeWeaponSlot : CardData
    {
        public override void ApplyEffect(Transform objectToApply)
        {
            if (objectToApply.TryGetComponent(out WeaponHolder weaponHolder))
            {
                
            }
        }
    }
}