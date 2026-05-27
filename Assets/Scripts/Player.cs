using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MaiNull
{
	public class Player : MonoBehaviour, IDamageable
	{
        [SerializeField] private PlayerData playerData;
        
        public Health Health { get; private set; } = new();
        private KinematicCharacterController _kinematicCharacterController;
        private Interactor _interactor;
        private WeaponHolder _weaponHolder;
        
        public static Action OnPlayerDie; 
        
        private void Awake()
        {
            _kinematicCharacterController = GetComponent<KinematicCharacterController>();
            _interactor = GetComponent<Interactor>();
            _weaponHolder = GetComponent<WeaponHolder>();
            
            if (playerData == null) return;
            Health = new Health(playerData.maxHealth);
            Health.OnDie += OnDead;
        }
        
        private void Start()
        {
            Transform orientation = GameObject.FindGameObjectWithTag("Orientation").transform;
            if (!orientation) return;
            
            _kinematicCharacterController.orientationPivot = orientation;
            _interactor.orientationTransform = orientation;
            _weaponHolder.shootOrientation = orientation;
        }
        
        private void OnEnable()
        {
            if (playerData.moveInputAction)
            {
                playerData.moveInputAction.action.performed += OnMovementPerformed;
                playerData.moveInputAction.action.canceled += OnMovementCanceled;
                playerData.moveInputAction.action.Enable();
            }

            if (playerData.jumpInputAction)
            {
                playerData.jumpInputAction.action.started += OnJumpStarted;
                playerData.jumpInputAction.action.Enable();
            }

            if (playerData.sprintInputAction)
            {
                playerData.jumpInputAction.action.started += OnSprintStart;
                playerData.jumpInputAction.action.canceled += OnSprintCancelled;
                playerData.jumpInputAction.action.Enable();
            }
            
            if (playerData.interactInputAction)
            {
                playerData.interactInputAction.action.started += OnInteractStarted;
                playerData.interactInputAction.action.Enable();
            }
            
            if (playerData.attackInputAction)
            {
                playerData.attackInputAction.action.started += OnAttackStarted;
                playerData.attackInputAction.action.Enable();
            }
            
            if (playerData.reloadInputAction)
            {
                playerData.reloadInputAction.action.started += OnReloadStart;
                playerData.reloadInputAction.action.Enable();
            }
            
            if (playerData.dropInputAction)
            {
                playerData.dropInputAction.action.started += OnDropStarted;
                playerData.dropInputAction.action.Enable();
            }
        }

        private void OnDisable()
        {
            if (!playerData.moveInputAction) return;
            playerData.moveInputAction.action.Disable();
            playerData.moveInputAction.action.performed -= OnMovementPerformed;
            playerData.moveInputAction.action.canceled -= OnMovementCanceled;

            if (!playerData.jumpInputAction) return;
            playerData.jumpInputAction.action.Disable();
            playerData.jumpInputAction.action.started -= OnJumpStarted;
            
            if (playerData.sprintInputAction)
            {
                playerData.jumpInputAction.action.Disable();
                playerData.jumpInputAction.action.started -= OnSprintStart;
                playerData.jumpInputAction.action.canceled -= OnSprintCancelled;
            }
            
            if (!playerData.interactInputAction) return;
            playerData.interactInputAction.action.Disable();
            playerData.interactInputAction.action.started -= OnInteractStarted;
            
            if (!playerData.attackInputAction) return;
            playerData.attackInputAction.action.Disable();
            playerData.attackInputAction.action.started -= OnAttackStarted;
            
            if (!playerData.reloadInputAction) return;
            playerData.reloadInputAction.action.Disable();
            playerData.reloadInputAction.action.started -= OnReloadStart;
            
            if (!playerData.dropInputAction) return;
            playerData.dropInputAction.action.Disable();
            playerData.dropInputAction.action.started -= OnDropStarted;
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
        
        private void OnAttackStarted(InputAction.CallbackContext obj)
        {
            _weaponHolder.ShootWeapon();
        }
        
        private void OnSprintCancelled(InputAction.CallbackContext obj)
        {
            throw new NotImplementedException();
        }

        private void OnSprintStart(InputAction.CallbackContext obj)
        {
            throw new NotImplementedException();
        }

        private void OnDropStarted(InputAction.CallbackContext obj)
        {
            _weaponHolder.DropCurrentWeapon();
        }

        private void OnReloadStart(InputAction.CallbackContext obj)
        {
            _weaponHolder.ReloadCurrentWeapon();
        }
        
        private void OnDead()
        {
            OnDisable();
            OnPlayerDie?.Invoke();
            Debug.Log("Player is dead!!");
        }
        
        public void Damage(float damageValue, Knockback knockback, Transform damageInstigator)
        {
            Health.RemoveHealth((int)damageValue);

            //Debug.Log("DAMAGE!");
        }
    }
}