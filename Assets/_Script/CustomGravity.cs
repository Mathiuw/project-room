using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CustomGravity : MonoBehaviour
{
    [SerializeField] float gravityForce = -9.81f;
    Rigidbody rb;

    void Awake() 
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void FixedUpdate() 
    {
        SetGravity(); 
    }

    void SetGravity()          
    {
        Vector3 desiredLinearVelocity = rb.linearVelocity;
        desiredLinearVelocity.y += gravityForce;

        rb.linearVelocity = desiredLinearVelocity;
    }
}
