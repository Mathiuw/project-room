using UnityEngine;
using UnityEngine.InputSystem;

namespace MaiNull
{
	[RequireComponent (typeof(CharacterController))]
	public class PlayerCharacterController : MonoBehaviour
	{
        [SerializeField] private InputActionReference moveInputAction;
        [SerializeField] private float moveSpeed = 50f;
        [SerializeField] private Vector3 gravity = new Vector3(0, -9.81f, 0);
        private CameraPivot cameraPivot;
		private CharacterController characterController;
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


        private void Move()
        {
            // input move
            Vector3 motion = (transform.forward * inputMoveVector.y) + (transform.right * inputMoveVector.x);

            // gravity
            if (characterController.isGrounded)
            {
                motion.y = 0;
            }
            else
            {
                motion.y = gravity.y * (Time.deltaTime);
            }

            Debug.Log(motion);
            characterController.Move(motion.normalized * moveSpeed * Time.deltaTime);
        }

        private void RotateBody()
        {
            transform.localRotation = Quaternion.Euler(0, cameraPivot.attatchedCamera.transform.eulerAngles.y, 0);
        }
    }
}