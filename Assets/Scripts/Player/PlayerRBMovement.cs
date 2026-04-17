using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MaiNull.Player
{
    public struct Knockback
    {
        public float force; 
        public float duration;
        public Vector3 knockbackDirection;

        public Knockback(float force, float duration, Vector3 knockbackDirection)
        {
            this.force = force;
            this.duration = duration;
            this.knockbackDirection = knockbackDirection;
        }
    }                    


    [RequireComponent(typeof(Rigidbody))]
    public class PlayerRBMovement : MonoBehaviour
    {
        // Input class
        public GameActions Input { get; private set; }
        Vector2 moveInputVector;

        // Movement
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 300f;
        [SerializeField] private float linearDamp = 30f;
        [SerializeField] private Vector3 gravity = new Vector3(0, -9.81f, 0);
        [Header("Slope")]
        [SerializeField] private float maxSlopeAngle = 35f;
        [SerializeField] private float slopeDownMultiplier = 1.3f;
        [Header("Grounded")]
        [SerializeField] [Range(0f,1f)] private float airControlPercentage = 0.65f;
        [SerializeField] private float groundedSphereRadius = 0.3f;
        [SerializeField] private float groundedDownMultiplier = 0.9f;
        [SerializeField] private LayerMask groundMask;
        Rigidbody rb;

        [Header("Rotation")]
        [SerializeField] private CameraPivot cameraPivot;

        // Sprint
        [Header("Sprint")]
        [SerializeField] bool canSprint = true;
        [field: SerializeField] public float MaxStamina { get; private set; } = 30;
        [SerializeField] int staminaCost = 10;
        [SerializeField] int staminaRecover = 8;
        [SerializeField] float sprintingMultiplier = 1.5f;

        private float currentSprintMultiplier = 1;
        public float Stamina { get; set; } = 0;
        public bool IsSprinting { get; set; } = false;
        
        public event Action<float> OnStaminaUpdated;

        // Kockback
        private Knockback currentKnockback;
        public Knockback CurrentKnockback { get => currentKnockback; set => currentKnockback = value; }

        private void Awake()
        {
            // Create input class
            Input = new GameActions();
            Input.Player.Move.performed += OnMovementPerformed;
            Input.Player.Move.canceled += OnMovementCanceled;
            Input.Enable();

            rb = GetComponent<Rigidbody>();

            Stamina = MaxStamina;
        }

        private void OnDisable()
        {
            Input.Player.Move.performed -= OnMovementPerformed;
            Input.Player.Move.canceled -= OnMovementCanceled;

            Input.Disable();
        }

        private void Update()
        {
            Sprint(KeyCode.LeftShift);
        }

        private void FixedUpdate()
        {
            Move(moveInputVector.y, moveInputVector.x);

            // Rotate body According to attached camera view
            transform.localRotation = Quaternion.Euler(0, cameraPivot.attatchedCamera.transform.eulerAngles.y, 0);
        }

        private void OnMovementPerformed(InputAction.CallbackContext value)
        {
            moveInputVector = value.ReadValue<Vector2>();
        }

        private void OnMovementCanceled(InputAction.CallbackContext value)
        {
            moveInputVector = Vector2.zero;
        }

        private void Move(float moveV, float moveH)
        {
            Vector3 desiredVelocity;

            if (CurrentKnockback.duration > 0f)
            {
                //Debug.Log("Knockback");
                currentKnockback.duration -= Time.deltaTime;
                desiredVelocity = currentKnockback.knockbackDirection.normalized * currentKnockback.force * Time.deltaTime;
            }
            else 
            {
                Vector3 moveDirection = transform.forward * moveV + transform.right * moveH;
                RaycastHit slopeHit;

                if (OnSlope(out slopeHit))
                {
                    Debug.Log($"{transform.name} is on a slope");
                    moveDirection = GetSlopeMoveDirection(moveDirection, slopeHit.normal);
                }
                else
                {
                    Debug.Log($"{transform.name} is not on a slope");
                }

                desiredVelocity = moveDirection.normalized * (moveSpeed * currentSprintMultiplier * Time.deltaTime);

                rb.AddForce(desiredVelocity, ForceMode.VelocityChange);
            }

            Debug.Log($"{transform.name} is gounded: {IsGrounded()}");

            // Gravity
            if (IsGrounded())
            {
                rb.linearDamping = linearDamp;
            }
            else
            {
                rb.linearDamping = 0;
                Vector3 gravityForce = new Vector3(0, rb.linearVelocity.y, 0) + gravity * Time.deltaTime;

                //rb.AddForce(gravityForce, ForceMode.VelocityChange);
            }

            Debug.Log(rb.linearVelocity);
        }

        public void Sprint(KeyCode RunInput)
        {
            if (!canSprint) return;

            if (Stamina > 0 && moveInputVector.y > 0 && UnityEngine.Input.GetKey(RunInput))
            {
                IsSprinting = true;

                currentSprintMultiplier = sprintingMultiplier;

                Stamina -= staminaCost * Time.deltaTime;
            }
            else
            {
                IsSprinting = false;

                currentSprintMultiplier = 1f;
            }

            if (!UnityEngine.Input.GetKey(RunInput) && !IsSprinting)
            {
                Stamina += (staminaRecover * Time.deltaTime);
            }

            // Clamp stamina value
            Stamina = Math.Clamp(Stamina, 0, MaxStamina);

            OnStaminaUpdated?.Invoke(Stamina);
        }

        private bool IsGrounded()
        {    
            //Physics.Raycast(transform.position, Vector3.down, out hit, maxRay);
            return Physics.CheckSphere(transform.position + Vector3.down * groundedDownMultiplier, groundedSphereRadius, groundMask);
        }

        private bool OnSlope(out RaycastHit slopeHit)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, slopeDownMultiplier, groundMask))
            {
                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);

                return angle < maxSlopeAngle && angle != 0;
            }

            return false;
        }

        private Vector3 GetSlopeMoveDirection(Vector3 moveDirection, Vector3 slopeNormal)
        {
            return Vector3.ProjectOnPlane(moveDirection, slopeNormal);
        }


        // GIZMOS
        private void OnDrawGizmos()
        {
            // grounded sphere
            if (IsGrounded())
            {
                Gizmos.color = Color.red;
            }
            else 
            {
                Gizmos.color = Color.green;
            }
            
            Gizmos.DrawSphere(transform.position + (Vector3.down * groundedDownMultiplier), groundedSphereRadius);

            // slope ray
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, Vector3.down * slopeDownMultiplier);
        }
    }
}

