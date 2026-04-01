using UnityEngine;
using UnityEngine.InputSystem;
using MaiNull.Interact;

namespace MaiNull
{
    public class PlayerInteract : MonoBehaviour
    {
        [Header("Interact ")]
        [SerializeField] private InputActionReference interactAction;
        [SerializeField] private LayerMask interactiveMask;
        [SerializeField] private float rayLength = 5;
        private Transform playerCamera;

        private void OnEnable()
        {
            interactAction.action.started += Interact;
            interactAction.action.Enable();
        }

        void Start()
        {
            // Find PlayerCamera
            CameraMovement cameraMovement = FindFirstObjectByType<CameraMovement>();

            if (cameraMovement != null)
            {
                playerCamera = cameraMovement.transform;
            }
            else
            {
                Debug.Log("Cant find PlayerCamera");
                enabled = false;
                return;
            }
        }

        private void OnDisable()
        {
            interactAction.action.started -= Interact;
            interactAction.action.Disable();
        }

        public void Interact(InputAction.CallbackContext value)
        {
            Debug.Log("Interaction try");

            RaycastHit hit;

            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, rayLength, interactiveMask))
            {
                IInteractable interactable = hit.transform.GetComponentInParent<IInteractable>();

                if (interactable != null) interactable.Interact(transform);

                Debug.DrawLine(playerCamera.position, hit.point, Color.green, 1f);
            }
            else
            {
                Debug.DrawLine(playerCamera.position, playerCamera.position + playerCamera.forward * rayLength, Color.red, 1f);
            }
        }
    }
}