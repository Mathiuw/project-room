using UnityEngine;

namespace MaiNull
{
    [CreateAssetMenu(fileName = "Upgrade_Weapon_Slot", menuName = "Card/Upgrade/Weapon Slot")]
    public class CardUpgradeWeaponSlot : Card
    {
        public override void ApplyCardEffect(Transform objectToApply)
        {
            if (objectToApply.TryGetComponent(out WeaponHolder weaponHolder))
            {
                
            }
        }
    }
}