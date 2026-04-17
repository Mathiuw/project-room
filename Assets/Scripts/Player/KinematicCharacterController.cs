using UnityEngine;
using UnityEngine.InputSystem;

namespace MaiNull.Player
{
	[RequireComponent (typeof(CharacterController))]
	public class KinematicCharacterController : MonoBehaviour
	{
        [SerializeField] private float moveSpeed = 50f;
        [SerializeField] private float jumpHeight = 100f;
        [SerializeField] private Vector3 gravity = new Vector3(0, -9.81f, 0);
        private CharacterController _characterController;
        private Vector3 _playerVelocity;
        private bool _jumpThisFrame = false; 
        
        public Vector2 InputMoveVector { get; set; } = Vector2.zero;

        public Transform OrientationPivot { get; set; }
        
        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
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
                Debug.Log("Jump");
                _playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity.y);
                _jumpThisFrame = false;
            }

            // Apply gravity
            _playerVelocity += gravity * Time.deltaTime;

            // Move
            Vector3 finalMove = move * moveSpeed + Vector3.up * _playerVelocity.y;
            _characterController.Move(finalMove * Time.deltaTime);
        }

        public void StartJump()
        {
            if (!_characterController.isGrounded) return;
            _jumpThisFrame = true;
        }
        
        private void RotateBody()
        {
            transform.localRotation = Quaternion.Euler(0, OrientationPivot.eulerAngles.y, 0);
        }
    }
}