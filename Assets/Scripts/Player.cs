using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MaiNull
{
	public class Player : MonoBehaviour, IDamageable
	{
        public static Action OnPlayerDie; 
        
        [SerializeField] private PlayerData playerData;
        
        public Health Health { get; private set; }

        public FPSCamera FPSCamera { get => _fpsCamera; set => _fpsCamera = value; }
        
        private KinematicCharacterController _kinematicCharacterController;
        private Interactor _interactor;
        private WeaponHolder _weaponHolder;
        private FPSCamera _fpsCamera;
        
        private void Awake()
        {
            Health = new Health(playerData.maxHealth);
            
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
                playerData.moveInputAction.action.performed += OnMovementPerform;
                playerData.moveInputAction.action.canceled += OnMovementCancel;
            }

            if (playerData.lookInputAction)
            {
                playerData.lookInputAction.action.started += OnLookStart;
                playerData.lookInputAction.action.canceled += OnLookCancel;
            }
            
            if (playerData.jumpInputAction)
            {
                playerData.jumpInputAction.action.started += OnJumpStart;
                playerData.jumpInputAction.action.Enable();
            }

            if (playerData.sprintInputAction)
            {
                playerData.sprintInputAction.action.started += OnSprintStart;
                playerData.sprintInputAction.action.canceled += OnSprintCancel;
            }
            
            if (playerData.interactInputAction)
            {
                playerData.interactInputAction.action.started += OnInteractStart;
            }
            
            if (playerData.attackInputAction)
            {
                playerData.attackInputAction.action.started += OnAttackStart;
            }
            
            if (playerData.reloadInputAction)
            {
                playerData.reloadInputAction.action.started += OnReloadStart;
            }
            
            if (playerData.dropInputAction)
            {
                playerData.dropInputAction.action.started += OnDropStart;
            }

            if (playerData.switchWeaponAction) 
            {
                playerData.switchWeaponAction.action.started += OnWeaponSwitchStart;
            }

            if (playerData.switchCardAction) 
            {
                playerData.switchCardAction.action.started += OnCardSwitchStart;
            }
            
            InputSystem.actions.FindActionMap("Player").Enable();
        }


        private void OnDisable()
        {
            if (playerData.moveInputAction) {
                playerData.moveInputAction.action.performed -= OnMovementPerform;
                playerData.moveInputAction.action.canceled -= OnMovementCancel;
            }

            if (!playerData.jumpInputAction) return;
            playerData.jumpInputAction.action.started -= OnJumpStart;
            
            if (playerData.sprintInputAction)
            {
                playerData.sprintInputAction.action.started -= OnSprintStart;
                playerData.sprintInputAction.action.canceled -= OnSprintCancel;
            }
            
            if (!playerData.interactInputAction) return;
            playerData.interactInputAction.action.started -= OnInteractStart;
            
            if (!playerData.attackInputAction) return;
            playerData.attackInputAction.action.started -= OnAttackStart;
            
            if (!playerData.reloadInputAction) return;
            playerData.reloadInputAction.action.started -= OnReloadStart;
            
            if (!playerData.dropInputAction) return;
            playerData.dropInputAction.action.started -= OnDropStart;
            
            InputSystem.actions.FindActionMap("Player").Disable();
        }

        #region Input

        private void OnInteractStart(InputAction.CallbackContext obj)
        {
            _interactor.TryInteract();
        }

        private void OnMovementPerform(InputAction.CallbackContext context)
        {
            Vector2 value =  context.ReadValue<Vector2>();
            _kinematicCharacterController.InputMoveVector = value;
            if (FPSCamera) {
                FPSCamera.AngleValue = value.x;
            }
        }

        private void OnMovementCancel(InputAction.CallbackContext value)
        {
            _kinematicCharacterController.InputMoveVector = Vector2.zero;
        }

        private void OnLookStart (InputAction.CallbackContext obj)
        {
            if (FPSCamera) {
                FPSCamera.MoveVector = obj.ReadValue<Vector2>();
            }
        }
        
        private void OnLookCancel (InputAction.CallbackContext obj)
        {
            if (FPSCamera) {
                FPSCamera.MoveVector = Vector2.zero;
            }
        }
        
        private void OnJumpStart(InputAction.CallbackContext obj)
        {
            _kinematicCharacterController.StartJump();
        }
        
        private void OnAttackStart(InputAction.CallbackContext obj)
        {
            _weaponHolder.ShootWeapon();
        }
        
        private void OnSprintStart(InputAction.CallbackContext obj)
        {
            _kinematicCharacterController.StartSprint();
        }
        
        private void OnSprintCancel(InputAction.CallbackContext obj)
        {
            _kinematicCharacterController.StopSprint();
        }

        private void OnDropStart(InputAction.CallbackContext obj)
        {
            _weaponHolder.DropCurrentWeapon();
        }

        private void OnReloadStart(InputAction.CallbackContext obj)
        {
            _weaponHolder.ReloadCurrentWeapon();
        }

        private void OnWeaponSwitchStart (InputAction.CallbackContext obj)
        {
            Vector2 scrollValue = obj.ReadValue<Vector2>();
            
            if (scrollValue == Vector2.zero) return;

            if (scrollValue.y > 0) {
                _weaponHolder.IncreaseIndex();
            } 
            else {
                _weaponHolder.DecreaseIndex();
            }
        }
        
        private void OnCardSwitchStart (InputAction.CallbackContext obj)
        {
            print($"Switch Card Value: {obj.ReadValue<float>()}");
        }
        
        #endregion
        
        private void OnDead()
        {
            OnDisable();
            OnPlayerDie?.Invoke();
            Debug.Log("Player Died!!");
        }
        
        public void Damage(float damageValue, Knockback knockback, Transform damageInstigator)
        {
            Health.RemoveHealth((int)damageValue);

            //Debug.Log("DAMAGE!");
        }
    }
}