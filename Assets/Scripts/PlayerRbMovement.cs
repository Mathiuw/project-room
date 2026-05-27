using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MaiNull
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerRbMovement : MonoBehaviour
    {
        // Movement
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 300f;
        [SerializeField] private float linearDamp = 30f;
        [SerializeField] private Vector3 gravity = new Vector3(0, -9.81f, 0);
        private Vector2 _moveDirection;
        
        [Header("Slope")]
        [SerializeField] private float maxSlopeAngle = 35f;
        [SerializeField] private float slopeDownMultiplier = 1.3f;
        
        [Header("Grounded")]
        [SerializeField] [Range(0f,1f)] private float airControlPercentage = 0.65f;
        [SerializeField] private float groundedSphereRadius = 0.3f;
        [SerializeField] private float groundedDownMultiplier = 0.9f;
        [SerializeField] private LayerMask groundMask;
        private Rigidbody _rb;

        [Header("Rotation")]
        [SerializeField] private Transform forwardPivot;

        // Sprint
        [Header("Sprint")]
        [SerializeField] private bool canSprint = true;
        [SerializeField] private float maxStamina = 30;
        [SerializeField] private int staminaCost = 10;
        [SerializeField] private int staminaRecover = 8;
        [SerializeField] private float sprintMultiplier = 1.5f;
        private bool _isSprinting = false;
        private float _currentStamina; 
        
        public float CurrentSprintMultiplier => IsSprinting ?  sprintMultiplier : 1f;
        public float Stamina => _currentStamina;
        public float MaxStamina => maxStamina;
        public bool IsSprinting => _isSprinting;
        
        public event Action<float> OnStaminaUpdated;

        // Knockback
        private Knockback _knockback;
        public Knockback Knockback { get => _knockback; set => _knockback = value; }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _currentStamina = maxStamina;
        }

        private void Update()
        {
            if (!canSprint) return;

            if (_isSprinting && _currentStamina > 0)
            {
                _currentStamina -= staminaCost * Time.deltaTime;
            }
            else
            {
                _isSprinting = false;
                _currentStamina += (staminaRecover * Time.deltaTime);
            }

            // Clamp stamina value
            _currentStamina = Math.Clamp(Stamina, 0, maxStamina);

            OnStaminaUpdated?.Invoke(Stamina);
        }

        private void FixedUpdate()
        {
            Move(_moveDirection.y, _moveDirection.x);

            // Rotate body according to the forward pivot
            transform.localRotation = Quaternion.Euler(0, forwardPivot.eulerAngles.y, 0);
        }

        private void Move(float moveV, float moveH)
        {
            Vector3 desiredVelocity;

            if (Knockback.Duration > 0f)
            {
                //Debug.Log("Knockback");
                _knockback.Duration -= Time.deltaTime;
                desiredVelocity = _knockback.KnockbackDirection.normalized * (_knockback.Force * Time.deltaTime);
            }
            else 
            {
                Vector3 moveDirection = transform.forward * moveV + transform.right * moveH;

                if (OnSlope(out RaycastHit slopeHit))
                {
                    print($"{transform.name} is on a slope");
                    moveDirection = GetSlopeMoveDirection(moveDirection, slopeHit.normal);
                }
                else print($"{transform.name} is not on a slope");

                desiredVelocity = moveDirection.normalized * (moveSpeed * CurrentSprintMultiplier * Time.deltaTime);
            }
            
            print($"{transform.name} is grounded: {IsGrounded()}");

            // Gravity
            if (IsGrounded())
            {
                _rb.linearDamping = linearDamp;
            }
            else
            {
                _rb.linearDamping = 0;
                Vector3 gravityForce = new Vector3(0, _rb.linearVelocity.y, 0) + gravity * Time.deltaTime;

                desiredVelocity += gravityForce;
            }

            _rb.linearVelocity = desiredVelocity;
            
            print(_rb.linearVelocity);
        }

        public void StartSprinting()
        {
            if (!canSprint && _currentStamina - staminaCost * Time.deltaTime < 0) return;

            _isSprinting = true;
        }

        public void StopSprinting()
        {
            _isSprinting = false;
        }

        private bool IsGrounded()
        {    
            //Physics.Raycast(transform.position, Vector3.down, out hit, maxRay);
            return Physics.CheckSphere(transform.position + Vector3.down * groundedDownMultiplier, groundedSphereRadius, groundMask);
        }

        private bool OnSlope(out RaycastHit slopeHit)
        {
            if (!Physics.Raycast(transform.position, Vector3.down, out slopeHit, slopeDownMultiplier, groundMask)) return false;
            
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        private static Vector3 GetSlopeMoveDirection(Vector3 moveDirection, Vector3 slopeNormal)
        {
            return Vector3.ProjectOnPlane(moveDirection, slopeNormal);
        }

        // GIZMOS
        private void OnDrawGizmos()
        {
            // grounded sphere
            Gizmos.color = IsGrounded() ? Color.red : Color.green;
            Gizmos.DrawSphere(transform.position + (Vector3.down * groundedDownMultiplier), groundedSphereRadius);
            
            // slope ray
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, Vector3.down * slopeDownMultiplier);
        }
    }
}

