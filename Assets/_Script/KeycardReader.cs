using System;
using UnityEngine;
using MaiNull.Interact;

public class KeycardReader : MonoBehaviour, IInteractable, IUIName
{
    // Keycard item type scriptable object
    [field: SerializeField] public KeycardData keycardNeeded { get; private set; }

    // Keycard reader materials
    [field: SerializeField] public Material offMaterial { get; private set; }
    [field: SerializeField] public Material acceptedMaterial { get; private set; }
    [field: SerializeField] public Material recusedMaterial { get; private set; }

    public bool Used { get; private set; } = false;

    public string ReadName => SetReadName();

    public event Action OnAcceptKeycard;

    public void Interact(Transform interactor)
    {
        Inventory inventory = interactor.GetComponent<Inventory>();

        if (inventory.HaveKeycard(keycardNeeded))
        {
            Used = true;
            ChangeMeshMaterials();
            OnAcceptKeycard?.Invoke();
        }
    }

    void ChangeMeshMaterials() 
    {
        Material[] materials = transform.GetComponentInChildren<MeshRenderer>().materials;

        materials[1] = offMaterial;
        materials[2] = acceptedMaterial;

        GetComponentInChildren<MeshRenderer>().materials = materials;
    }

    private string SetReadName() 
    {
        if (!keycardNeeded)
        {
            return "Keycard type null";
        }

        if (!Used)
        {
            return "Need " + keycardNeeded.itemName;
        }
        else return "";
    }
}
