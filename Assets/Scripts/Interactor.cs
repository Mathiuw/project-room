using UnityEngine;

namespace MaiNull
{
    public class Interactor : MonoBehaviour
    {
        [Header("Interact Settings")] 
        public Transform orientationTransform;
        [SerializeField] private LayerMask interactiveMask;
        [SerializeField] private float rayLength = 5;

        public void TryInteract()
        {
            print($"{name} tried to interact");

            if (Physics.Raycast(orientationTransform.transform.position, orientationTransform.transform.forward, out RaycastHit hit, rayLength, interactiveMask))
            {
                IInteractable interactable = hit.transform.GetComponentInParent<IInteractable>();

                interactable?.Interact(transform);

                Debug.DrawLine(orientationTransform.position, hit.point, Color.green, 1f);
            }
            else
            {
                Debug.DrawLine(orientationTransform.position, orientationTransform.position + orientationTransform.forward * rayLength, Color.red, 1f);
            }
        }
    }
}