using UnityEngine;
using UnityEngine.InputSystem;

namespace MaiNull
{
	public class Player : MonoBehaviour, IDamageable
	{
        [SerializeField] private PlayerData playerData;
        
        public Health Health { get; private set; } = new();
        private CameraPivot _cameraPivot;
        private KinematicCharacterController _kinematicCharacterController;
        private Interactor _interactor;
        
        private void Awake()
        {
            _cameraPivot = GetComponentInChildren<CameraPivot>();
            _kinematicCharacterController = GetComponent<KinematicCharacterController>();
            _interactor = GetComponent<Interactor>();
            
            if (playerData == null) return;
            Health = new Health(playerData.maxHealth);
            Health.OnDie += OnDead;
        }
        
        private void Start()
        {
            _kinematicCharacterController.OrientationPivot = GetComponentInChildren<CameraPivot>().transform;
            _interactor.OrientationTransform = _cameraPivot.transform;
        }
        
        private void OnEnable()
        {
            if (!playerData.moveInputAction) return;
            playerData.moveInputAction.action.performed += OnMovementPerformed;
            playerData.moveInputAction.action.canceled += OnMovementCanceled;
            playerData.moveInputAction.action.Enable();

            if(!playerData.jumpInputAction) return;
            playerData.jumpInputAction.action.started += OnJumpStarted;
            playerData.jumpInputAction.action.Enable();
            
            playerData.interactInputAction.action.started += OnInteractStarted;
            playerData.interactInputAction.action.Enable();
        }

        private void OnDisable()
        {
            if (!playerData.moveInputAction) return;
            playerData.moveInputAction.action.performed -= OnMovementPerformed;
            playerData.moveInputAction.action.canceled -= OnMovementCanceled;
            playerData.moveInputAction.action.Disable();

            if (!playerData.jumpInputAction) return;
            playerData.jumpInputAction.action.started -= OnJumpStarted;
            playerData.jumpInputAction.action.Disable();
            
            if (!playerData.interactInputAction) return;
            playerData.interactInputAction.action.started -= OnInteractStarted;
            playerData.interactInputAction.action.Disable();
        }

        private void OnInteractStarted(InputAction.CallbackContext obj)
        {
            _interactor.TryInteract();
        }

        private void OnMovementPerformed(InputAction.CallbackContext context)
        {
            _kinematicCharacterController.InputMoveVector = context.ReadValue<Vector2>();
        }

        private void OnMovementCanceled(InputAction.CallbackContext value)
        {
            _kinematicCharacterController.InputMoveVector = Vector2.zero;
        }
        
        private void OnJumpStarted(InputAction.CallbackContext obj)
        {
            _kinematicCharacterController.StartJump();
        }
        
        private void OnDead()
        {
            OnDisable();
            
            Debug.Log("Player is dead!!");
        }
        
        public void Damage(float damageValue, Knockback knockback, Transform damageInstigator)
        {
            Health.RemoveHealth((int)damageValue);

            //Debug.Log("DAMAGE!");
        }
    }
}