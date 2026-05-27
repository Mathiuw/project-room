using System;
using UnityEngine;

namespace MaiNull
{
	[RequireComponent (typeof(CharacterController))]
	public class KinematicCharacterController : MonoBehaviour
	{
        public Transform orientationPivot;
        [Header("Move Settings")]
        [SerializeField] private float moveSpeed = 50f;
        [SerializeField] private float jumpHeight = 100f;
        [SerializeField] private Vector3 gravity = new(0, -9.81f, 0);
        private CharacterController _characterController;
        private Vector3 _playerVelocity;
        private bool _jumpThisFrame;
        
        [Header("Sprint Settings")]
        [SerializeField] private float sprintMultiplier = 1.8f;
        [SerializeField] private float maxStamina = 100f;
        [SerializeField] private float staminaCostPerFrame = 3f;
        private float _currentStamina;
        public bool isSprinting = false;
        
        public float CurrentMultiplier => isSprinting ? sprintMultiplier : 1f;
        
        public Vector2 InputMoveVector { get; set; } = Vector2.zero;
        
        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            _currentStamina = isSprinting ? Mathf.Clamp(_currentStamina - Time.deltaTime, 0, maxStamina) : Mathf.Clamp(_currentStamina + Time.deltaTime, 0, maxStamina);
            
            RotateBody();
            Move();
        }

        private void Move()
        {
            if (_characterController.isGrounded)
            {
                // Slight downward velocity to keep grounded stable
                if (_playerVelocity.y < -2f)
                    _playerVelocity.y = -2f;
            }

            Vector3 move = (transform.forward * InputMoveVector.y) + (transform.right * InputMoveVector.x);
            move = Vector3.ClampMagnitude(move, 1f);

            if (move != Vector3.zero)
                transform.forward = move;

            // Jump
            if (_jumpThisFrame)
            {
                print("Jump");
                _playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity.y);
                _jumpThisFrame = false;
            }

            // Apply gravity
            _playerVelocity += gravity * Time.deltaTime;

            // Move
            Vector3 finalMove = move * (moveSpeed * CurrentMultiplier) + Vector3.up * _playerVelocity.y;
            _characterController.Move(finalMove * Time.deltaTime);
        }

        public void StartJump()
        {
            if (!_characterController.isGrounded) return;
            _jumpThisFrame = true;
        }

        public void StartSprint()
        {
            isSprinting = true;
        }

        public void StopSprint()
        {
            isSprinting = false;
        }
        
        private void RotateBody()
        {
            transform.localRotation = Quaternion.Euler(0, orientationPivot.eulerAngles.y, 0);
        }
    }
}