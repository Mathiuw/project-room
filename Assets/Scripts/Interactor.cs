using MaiNull.Interact;
using UnityEngine;

namespace MaiNull
{
    public class Interactor : MonoBehaviour
    {
        [Header("Interact Settings")]
        [SerializeField] private LayerMask interactiveMask;
        [SerializeField] private float rayLength = 5;

        public Transform OrientationTransform { get; set; }

        private void Start()
        {
            // Find PlayerCamera
            CameraMovement cameraMovement = FindFirstObjectByType<CameraMovement>();

            if (cameraMovement != null)
            {
                OrientationTransform = cameraMovement.transform;
            }
            else
            {
                Debug.Log("Cant find PlayerCamera");
                enabled = false;
            }
        }

        public void TryInteract()
        {
            Debug.Log("Interaction try");

            if (Physics.Raycast(OrientationTransform.transform.position, OrientationTransform.transform.forward, out RaycastHit hit, rayLength, interactiveMask))
            {
                IInteractable interactable = hit.transform.GetComponentInParent<IInteractable>();

                interactable?.Interact(transform);

                Debug.DrawLine(OrientationTransform.position, hit.point, Color.green, 1f);
            }
            else
            {
                Debug.DrawLine(OrientationTransform.position, OrientationTransform.position + OrientationTransform.forward * rayLength, Color.red, 1f);
            }
        }
    }
}