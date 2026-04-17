using TMPro;
using UnityEngine;

namespace MaiNull.UI
{
    public class UIReadNames : MonoBehaviour
    {
        [SerializeField] private LayerMask layersToRead;
        [SerializeField] private TextMeshProUGUI displayText;
        [SerializeField] private float maxDistance = 5f;
        [SerializeField] private float tickRate = 0.1f;
        private Transform _cameraTransform;

        private void Start()
        {
            if (Camera.main != null) _cameraTransform = Camera.main.transform;
            
            InvokeRepeating(nameof(CheckAndDisplayName), 0f, tickRate);
        }

        private void CheckAndDisplayName()
        {
            if (Physics.Raycast(_cameraTransform.position,_cameraTransform.forward, out RaycastHit hit, maxDistance, layersToRead))
            {
                //Debug.Log("Interact Hit");

                IUIName uiName = hit.transform.GetComponentInParent<IUIName>();

                displayText.SetText(uiName != null ? uiName.readName : "");
                //Debug.Log("Interact hit doesnt have IUIName Interface");
            }
            else
            {
                displayText.SetText("");
            }
        }
    }
}