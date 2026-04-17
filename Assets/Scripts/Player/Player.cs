using UnityEngine;
using UnityEngine.InputSystem;

namespace MaiNull.Player
{
	public class Player : MonoBehaviour, IDamageable
	{
        [SerializeField] private PlayerData playerData;
        
        public Health Health { get; private set; } = new(100);
        private KinematicCharacterController _kinematicCharacterController;

        private void OnEnable()
        {
            if (!playerData.moveInputAction) return;
            playerData.moveInputAction.action.performed += OnMovementPerformed;
            playerData.moveInputAction.action.canceled += OnMovementCanceled;
            playerData.moveInputAction.action.Enable();

            if(!playerData.jumpInputAction) return;
            playerData.jumpInputAction.action.started += OnJumpStarted;
            playerData.jumpInputAction.action.Enable();
        }

        private void OnJumpStarted(InputAction.CallbackContext obj)
        {
            _kinematicCharacterController.StartJump();
        }

        private void Awake()
        {
            Health = new Health(playerData.maxHealth);
            Health.OnDie += OnDead;
            
        }

        private void Start()
        {
            _kinematicCharacterController.OrientationPivot = GetComponentInChildren<CameraPivot>().transform;
        }

        private void OnDisable()
        {
            if (!playerData.moveInputAction) return;
            playerData.moveInputAction.action.performed -= OnMovementPerformed;
            playerData.moveInputAction.action.canceled -= OnMovementCanceled;
            playerData.moveInputAction.action.Disable();

            if (!playerData.jumpInputAction) return;
            playerData.jumpInputAction.action.Disable();
        }
        
        private void OnMovementPerformed(InputAction.CallbackContext context)
        {
            _kinematicCharacterController.InputMoveVector = context.ReadValue<Vector2>();
        }

        private void OnMovementCanceled(InputAction.CallbackContext value)
        {
            _kinematicCharacterController.InputMoveVector = Vector2.zero;
        }
        
        private void OnDead()
        {
            if (!playerData.moveInputAction) return;
            playerData.moveInputAction.action.performed -= OnMovementPerformed;
            playerData.moveInputAction.action.canceled -= OnMovementCanceled;
            playerData.moveInputAction.action.Disable();

            if (!playerData.jumpInputAction) return;
            playerData.jumpInputAction.action.Disable();
            
            Debug.Log("Player is dead!!");
        }

        public void Damage(float damageValue, Knockback knockback, Transform damageInstigator)
        {
            Health.RemoveHealth((int)damageValue);

            //Debug.Log("DAMAGE!");
        }
    }
}