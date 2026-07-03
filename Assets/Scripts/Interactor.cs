using UnityEngine;

namespace MaiNull
{
    public class Interactor : MonoBehaviour
    {
        [Header("Interact Settings")] 
        public Transform orientationTransform;
        [SerializeField] private LayerMask interactiveMask;
        [SerializeField] private float interactiveMaxLength = 5;
        
        public float InteractiveMaxLength => interactiveMaxLength;

        public void TryInteract()
        {
            print($"{transform.name} tried to interact");

            if (Physics.Raycast(orientationTransform.transform.position, orientationTransform.transform.forward, out RaycastHit hit, interactiveMaxLength, interactiveMask))
            {
                IInteractable interactable = hit.transform.GetComponentInParent<IInteractable>();

                interactable?.Interact(transform);

                Debug.DrawLine(orientationTransform.position, hit.point, Color.green, 1f);
            }
            else
            {
                Debug.DrawLine(orientationTransform.position, orientationTransform.position + orientationTransform.forward * interactiveMaxLength, Color.red, 1f);
            }
        }
    }
}