using UnityEngine;

namespace MaiNull
{
    [RequireComponent(typeof(Rigidbody))]
    public class CustomGravity : MonoBehaviour
    {
        [SerializeField] private float gravityForce = -9.81f;
        private Rigidbody _rb;

        private void Awake() 
        {
            _rb = GetComponent<Rigidbody>();
            _rb.useGravity = false;
        }

        private void FixedUpdate() 
        {
            SetGravity(); 
        }

        private void SetGravity()          
        {
            Vector3 desiredLinearVelocity = _rb.linearVelocity;
            desiredLinearVelocity.y += gravityForce;

            _rb.linearVelocity = desiredLinearVelocity;
        }
    }
}
