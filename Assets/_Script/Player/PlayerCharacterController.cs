using UnityEngine;

namespace MaiNull.Player
{
	[RequireComponent (typeof(CharacterController))]
	public class PlayerCharacterController : MonoBehaviour
	{
        [SerializeField] private float moveSpeed = 50f;
		private CharacterController characterController;
        private Vector2 inputMovementVector;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            Vector3 motion = inputMovementVector * (moveSpeed * Time.deltaTime);

            characterController.Move(motion);
        }
    }
}