using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MaiNull
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
    public class PlayerMovementRB : MonoBehaviour
    {
        // Input class
        public GameActions Input { get; private set; }
        Vector2 moveInputVector;

        // Movement
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 300f;
        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private float maxSlopeAngle = 35f;
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


        void Awake()
        {
            // Create input class
            Input = new GameActions();

            Input.Player.Move.performed += OnMovementPerformed;
            Input.Player.Move.canceled += OnMovementCanceled;

            Input.Enable();

            rb = GetComponent<Rigidbody>();

            Stamina = MaxStamina;
        }

        void OnDisable()
        {
            Input.Player.Move.performed -= OnMovementPerformed;
            Input.Player.Move.canceled -= OnMovementCanceled;

            Input.Disable();
        }

        void Update()
        {
            Sprint(KeyCode.LeftShift);
        }

        void FixedUpdate()
        {
            Movement(moveInputVector.y, moveInputVector.x);

            // Rotate body According to attached camera view
            transform.localRotation = Quaternion.Euler(0, cameraPivot.attatchedCamera.transform.eulerAngles.y, 0);
        }

        void OnMovementPerformed(InputAction.CallbackContext value)
        {
            moveInputVector = value.ReadValue<Vector2>();
        }

        void OnMovementCanceled(InputAction.CallbackContext value)
        {
            moveInputVector = Vector2.zero;
        }

        public void Movement(float moveV, float moveH)
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
                    Debug.Log($"{transform.name} is on a Slope");
                    moveDirection = GetSlopeMoveDirection(moveDirection, slopeHit.normal);
                }

                desiredVelocity = moveDirection.normalized * (moveSpeed * currentSprintMultiplier * Time.deltaTime);
            }

            // Gravity
            Debug.Log($"{transform.name} is gounded: {IsGrounded()}");
            if (IsGrounded())
            {
                desiredVelocity.y = 0f;
            }
            else
            {
                desiredVelocity.y = rb.linearVelocity.y + (gravity * Time.deltaTime);
            }


            rb.linearVelocity = desiredVelocity;
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
            //RaycastHit hit;
            float groundDistance = 1f;
            float sphereRadius = 0.15f;

            
            //Physics.Raycast(transform.position, Vector3.down, out hit, maxRay);
            return Physics.CheckSphere(transform.position + Vector3.down * groundDistance, sphereRadius, groundMask);
        }

        private bool OnSlope(out RaycastHit slopeHit)
        {
            float playerHeight = GetComponent<CapsuleCollider>().height;
            const float Aditional = 0.1f;

            if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + Aditional, groundMask))
            {
                float angle = Vector3.Angle(moveInputVector, slopeHit.normal);

                return angle < maxSlopeAngle && angle != 0;
            }

            return false;
        }

        private Vector3 GetSlopeMoveDirection(Vector3 moveDirection, Vector3 slopeNormal)
        {
            return Vector3.ProjectOnPlane(moveDirection, slopeNormal);
        }

        private void OnDrawGizmos()
        {
            if (IsGrounded())
            {
                Gizmos.color = Color.red;
            }
            else 
            {
                Gizmos.color = Color.green;
            }

            Gizmos.DrawSphere(transform.position + Vector3.down * 1f, 0.15f);

            float playerHeight = GetComponent<CapsuleCollider>().height;
            float aditional = 0.15f;

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, Vector3.down * (playerHeight / 2 + aditional));
        }
    }
}

