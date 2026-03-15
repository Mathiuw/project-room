using UnityEngine;
using UnityEngine.InputSystem;
using MaiNull.Interact;

namespace MaiNull.Player
{
    public class PlayerInteract : MonoBehaviour
    {
        [Header("Interact ")]
        [SerializeField] private LayerMask interactiveMask;
        [SerializeField] private float rayLength = 5;
        private PlayerMovement playerMovement;
        private Transform playerCamera;

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

            if (TryGetComponent(out playerMovement))
            {
                playerMovement.Input.Player.Interact.started += Interact;
            }
            else
            {
                Debug.Log("Cant find playerMovement");
                enabled = false;
                return;
            }
        }

        private void OnDisable()
        {
            playerMovement.Input.Player.Interact.started -= Interact;
        }

        public void Interact(InputAction.CallbackContext value)
        {
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