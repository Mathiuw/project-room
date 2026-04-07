using TMPro;
using UnityEngine;

namespace MaiNull
{
    public class UI_ReadNames : MonoBehaviour
    {
        [SerializeField] LayerMask layersToRead;
        [SerializeField] TextMeshProUGUI displayText;
        [SerializeField] float maxDistance = 5f;
        Transform cameraTransform;
        RaycastHit hit;

        void Start()
        {
            cameraTransform = Camera.main.transform;
        }

        void Update()
        {
            if (Physics.Raycast(cameraTransform.position,cameraTransform.forward, out hit, maxDistance, layersToRead))
            {
                //Debug.Log("Interact Hit");

                IUIName uiName = hit.transform.GetComponentInParent<IUIName>();

                if (uiName != null)
                {
                    displayText.SetText(uiName.readName);
                }
                else
                {
                    displayText.SetText("");
                    //Debug.Log("Interact hit doesnt have IUIName Interface");
                }
            }
            else
            {
                displayText.SetText("");
            }

        }
    }
}