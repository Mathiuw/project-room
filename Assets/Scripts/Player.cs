using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MaiNull
{
	public class Player : MonoBehaviour, IDamageable
	{
        public static Action OnPlayerDie; 
        
        [SerializeField] private PlayerData playerData;
        private KinematicCharacterController _kinematicCharacterController;
        private Interactor _interactor;
        private WeaponHolder _weaponHolder;
        private FPSCamera _fpsCamera;
        
        public Health Health { get; private set; }
        public FPSCamera FPSCamera { get => _fpsCamera; set => _fpsCamera = value; }
        
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
            foreach (Transform child in transform) {
                // Check If player has an orientation transform
                if (!child.CompareTag("Orientation")) continue;
                
                _kinematicCharacterController.orientationPivot = child;
                _interactor.orientationTransform = child;
                _weaponHolder.shootOrientation = child;
            }
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
            if (playerData.moveInputAction) 
            {
                playerData.moveInputAction.action.performed -= OnMovementPerform;
                playerData.moveInputAction.action.canceled -= OnMovementCancel;
            }

            if (!playerData.jumpInputAction) 
            {
                playerData.jumpInputAction.action.started -= OnJumpStart;
            }
            
            if (playerData.sprintInputAction)
            {
                playerData.sprintInputAction.action.started -= OnSprintStart;
                playerData.sprintInputAction.action.canceled -= OnSprintCancel;
            }

            if (!playerData.interactInputAction) 
            {
                playerData.interactInputAction.action.started -= OnInteractStart;
            }

            if (!playerData.reloadInputAction) 
            {
                playerData.reloadInputAction.action.started -= OnReloadStart;
            }

            if (!playerData.dropInputAction) 
            {
                playerData.dropInputAction.action.started -= OnDropStart;
            }
            
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
        
        private void OnAttack()
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

            // print(scrollValue.y);
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

        private void Update()
        {
            if (_weaponHolder.CurrentWeapon == null) return;
            
            switch (_weaponHolder.CurrentWeapon.WeaponData.inputType) {

                case EWeaponInputType.Tap:
                    if (playerData.attackInputAction.action.WasPressedThisFrame()) {
                        OnAttack();
                    }
                    break;
                case EWeaponInputType.Hold:
                    if (playerData.attackInputAction.action.IsPressed()) {
                        OnAttack();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void OnDead()
        {
            Destroy(gameObject);
            OnPlayerDie?.Invoke();
            print("Player Died!!");
        }
    }
}