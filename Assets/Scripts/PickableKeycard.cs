using MaiNull.Item;
using UnityEngine;

namespace MaiNull
{
    public class PickableKeycard : PickableItem
    {
        private void Awake()
        {
            if (PickableItemData is not KeycardData keycardData)
            {
                Debug.LogError("Pickable data is the wrong type");
                return;
            }

            Material[] newMaterials = GetComponentInChildren<MeshRenderer>().materials;
            newMaterials[0] = keycardData.GetColorMaterial();

            GetComponentInChildren<MeshRenderer>().materials = newMaterials;
        }

    }
}
