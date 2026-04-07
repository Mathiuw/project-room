using UnityEngine;

namespace MaiNull.Item
{
    public enum EKeycardColor
    {
        Red,
        Green,
        Blue,
        Yellow
    }

    [CreateAssetMenu(fileName = "KeycardData", menuName = "KeycardData")]
    public class KeycardData : InventoryItemData
    {
        [Header("Keycard")]
        public EKeycardColor keycardColor;
    }
}