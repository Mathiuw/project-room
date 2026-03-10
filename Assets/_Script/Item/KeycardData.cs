using MaiNull.Item;
using UnityEngine;

public enum EKeycardColor
{
    Red,
    Green,
    Blue,
    Yellow
}

[CreateAssetMenu(fileName = "KeycardData", menuName = "KeycardData")]
public class KeycardData : PickableItemData
{
    [Header("Keycard")]
    public EKeycardColor keycardColor;
}
