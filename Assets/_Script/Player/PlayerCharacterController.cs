using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

namespace MaiNull
{
	[RequireComponent (typeof(CharacterController))]
	public class PlayerCharacterController : MonoBehaviour
	{
        [SerializeField] private InputActionReference moveInputAction;
        [SerializeField] private InputActionReference jumpInputAction;
        [SerializeField] private float moveSpeed = 50f;
        [SerializeField] private float jumpHeight = 100f;
        [SerializeField] private Vector3 gravity = new Vector3(0, -9.81f, 0);
        private CameraPivot cameraPivot;
		private CharacterController characterController;
        private Vector3 playerVelocity;
        private Vector2 inputMoveVector;

        private void Awake()
        {
            cameraPivot = GetComponentInChildren<CameraPivot>();
            characterController = GetComponent<CharacterController>();
        }

        private void OnEnable()
        {
            if (moveInputAction)
            {
                moveInputAction.action.performed += OnMovementPerformed;
                moveInputAction.action.canceled += OnMovementCanceled;
                moveInputAction.action.Enable();

                jumpInputAction.action.Enable();
            }
        }



        private void Update()
        {
            RotateBody();
            Move();
        }

        private void OnDisable()
        {
            if (moveInputAction)
            {
                moveInputAction.action.performed -= OnMovementPerformed;
                moveInputAction.action.canceled -= OnMovementCanceled;
                moveInputAction.action.Disable();

                jumpInputAction.action.Disable();
            }
        }

        private void OnMovementPerformed(InputAction.CallbackContext context)
        {
            inputMoveVector = context.ReadValue<Vector2>();
        }

        private void OnMovementCanceled(InputAction.CallbackContext value)
        {
            inputMoveVector = Vector2.zero;
        }


        //private void Move()
        //{
        //    // input move
        //    Vector3 motion = (transform.forward * inputMoveVector.y) + (transform.right * inputMoveVector.x);

        //    // gravity
        //    if (characterController.isGrounded)
        //    {
        //        motion.y = 0;
        //    }
        //    else
        //    {
        //        motion.y = gravity.y * (Time.deltaTime);
        //    }

        //    Debug.Log(motion);
        //    characterController.Move(motion.normalized * moveSpeed * Time.deltaTime);
        //}

        private void Move()
        {
            if (characterController.isGrounded)
            {
                // Slight downward velocity to keep grounded stable
                if (playerVelocity.y < -2f)
                    playerVelocity.y = -2f;
            }

            Vector3 move = (transform.forward * inputMoveVector.y) + (transform.right * inputMoveVector.x);
            move = Vector3.ClampMagnitude(move, 1f);

            if (move != Vector3.zero)
                transform.forward = move;

            // Jump using WasPressedThisFrame()
            if (characterController.isGrounded && jumpInputAction.action.WasPressedThisFrame())
            {
                Debug.Log("Jump");
                playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity.y);
            }

            // Apply gravity
            playerVelocity += gravity * Time.deltaTime;

            // Move
            Vector3 finalMove = move * moveSpeed + Vector3.up * playerVelocity.y;
            characterController.Move(finalMove * Time.deltaTime);
        }

        private void RotateBody()
        {
            transform.localRotation = Quaternion.Euler(0, cameraPivot.attatchedCamera.transform.eulerAngles.y, 0);
        }
    }
}