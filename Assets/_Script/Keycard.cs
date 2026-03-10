using MaiNull.Item;
using UnityEngine;

public class Keycard : PickableItem
{
    [SerializeField] private Material[] materials = new Material[4];

    private void Awake()
    {
        KeycardData karcardData = (KeycardData)PickableItemData;
        Material[] materials = GetComponentInChildren<MeshRenderer>().materials;
        switch (karcardData.keycardColor)
        {
            case EKeycardColor.Red:
                materials[0] = this.materials[0];
                break;
            case EKeycardColor.Green:
                materials[0] = this.materials[1]; 
                break;
            case EKeycardColor.Blue:
                materials[0] = this.materials[2];
                break;
            case EKeycardColor.Yellow:
                materials[0] = this.materials[3];
                break;
            default:
                Debug.LogError("Error setting keycard color");
                break;
        }

        GetComponentInChildren<MeshRenderer>().materials = materials;
    }

}
