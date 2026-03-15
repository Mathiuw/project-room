using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MaiNull.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        // Input class
        public GameActions Input { get; private set; }

        [Header("Movement")]
        [SerializeField] float moveSpeed = 300f;
        Vector2 moveVector;
        Rigidbody rb;

        [Header("Rotation")]
        [SerializeField] private CameraPivot cameraPivot;

        [Header("Sprint")]
        [SerializeField] bool canSprint = true;
        [field: SerializeField] public float MaxStamina { get; set; } = 30;
        [SerializeField] int staminaCost = 10;
        [SerializeField] int staminaRecover = 8;
        [SerializeField] float sprintingMultiplier = 1.5f;

        private float currentSprintMultiplier = 1;
        public float Stamina { get; set; } = 0;
        public bool IsSprinting { get; set; } = false;

        // Stamina update event
        public event Action<float> OnStaminaUpdated;

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
            Movement(moveVector.y, moveVector.x);

            // Rotate body According to attached camera view
            transform.localRotation = Quaternion.Euler(0, cameraPivot.attatchedCamera.transform.eulerAngles.y, 0);
        }

        void OnMovementPerformed(InputAction.CallbackContext value)
        {
            moveVector = value.ReadValue<Vector2>();
        }

        void OnMovementCanceled(InputAction.CallbackContext value)
        {
            moveVector = Vector2.zero;
        }

        public void Movement(float moveV, float moveH)
        {
            Vector3 moveDirection = transform.forward * moveV + transform.right * moveH;

            rb.linearVelocity = moveDirection.normalized * (moveSpeed * currentSprintMultiplier * Time.deltaTime);
        }

        public void Sprint(KeyCode RunInput)
        {
            if (!canSprint) return;

            if (Stamina > 0 && moveVector.y > 0 && UnityEngine.Input.GetKey(RunInput))
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
    }
}

