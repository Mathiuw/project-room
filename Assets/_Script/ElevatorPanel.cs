using System;
using UnityEngine;
using MaiNull.Interact;

public class ElevatorPanel : MonoBehaviour, IInteractable, IUIName
{
    bool used = false;

    [SerializeField] Buttom buttom;

    enum Buttom { up, down }

    [SerializeField] Material onMaterial;
    [SerializeField] Material offMaterial;
    MeshRenderer mesh;

    public string ReadName => SetReadName();

    public event Action onButtomPress;

    void Awake() 
    {
        mesh = GetComponentInChildren<MeshRenderer>();
    } 

    void Start() 
    {
        SetMaterials(mesh);

        //if (buttom == Buttom.down) onButtomPress += ManagerGame.instance.EndGame;
    }

    public void Interact(Transform interactor)
    {
        used = !used;
        SetMaterials(mesh);
        onButtomPress?.Invoke();
        Destroy(this);
    }

    void SetMaterials(MeshRenderer mesh) 
    {
        Material[] materials= mesh.materials;

        if (buttom == Buttom.up && !used)
        {
            materials[1] = offMaterial;
            materials[2] = onMaterial;
        }
        else if (used)
        {
            materials[1] = offMaterial;
            materials[2] = offMaterial;
        }

        mesh.materials = materials;
    }

    string SetReadName() 
    {
        if (used)
        {
            return "";
        }

        if (buttom == Buttom.up) return "Call Elevator";
        else return "Close Elevator";
    }

}
