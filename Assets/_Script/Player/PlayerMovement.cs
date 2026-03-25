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
    public class PlayerMovement : MonoBehaviour
    {
        // Input class
        public GameActions Input { get; private set; }
        Vector2 moveInputVector;

        // Movement
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 300f;
        [SerializeField] private float gravity = 40f;
        [SerializeField] private float maxSlopeAngle = 40f;
        Rigidbody rb;

        [Header("Rotation")]
        [SerializeField] private CameraPivot cameraPivot;

        // Sprint
        [Header("Sprint")]
        [SerializeField] bool canSprint = true;
        [field: SerializeField] public float MaxStamina { get; set; } = 30;
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
                Debug.Log("Knockback");
                currentKnockback.duration -= Time.deltaTime;
                desiredVelocity = currentKnockback.knockbackDirection.normalized * currentKnockback.force * Time.deltaTime;
            }
            else
            {
                Vector3 moveDirection = transform.forward * moveV + transform.right * moveH;
                RaycastHit slopeHit;

                if (OnSlope(out slopeHit))
                {
                    moveDirection = GetSlopeMoveDirection(moveDirection, slopeHit.normal);
                }

                desiredVelocity = moveDirection.normalized * (moveSpeed * currentSprintMultiplier * Time.deltaTime);
            }

            // Gravity
            if (IsGrounded())
            {
                desiredVelocity.y = 0f;
            }
            else
            {
                desiredVelocity.y = gravity * Time.deltaTime;
            }

            rb.linearVelocity = desiredVelocity;
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
            RaycastHit hit;
            float maxRay = 1.35f;

            return Physics.Raycast(transform.position, Vector3.down, out hit, maxRay);
        }

        private bool OnSlope(out RaycastHit slopeHit)
        {
            float playerHeight = GetComponent<CapsuleCollider>().height;
            float aditional = 0.35f;

            if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + aditional))
            {
                float angle = Vector3.Angle(moveInputVector, slopeHit.normal);

                if (angle < maxSlopeAngle && angle != 0)
                {
                    Debug.Log($"{transform.name} is on a Slope");
                }

                return angle < maxSlopeAngle && angle != 0;
            }

            return false;
        }

        private Vector3 GetSlopeMoveDirection(Vector3 moveDirection, Vector3 slopeNormal)
        {
            return Vector3.ProjectOnPlane(moveDirection, slopeNormal);
        }
    }
}

