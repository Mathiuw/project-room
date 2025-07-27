using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] bool _activateOnStart = false;
   
    void Start() 
    {
        if (_activateOnStart) SetRagdollState(true);
        else SetRagdollState(false);
    }   

    public void SetRagdollState(bool state) 
    {
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>()) 
        {
            rb.isKinematic = !state;
        }
    }
}
