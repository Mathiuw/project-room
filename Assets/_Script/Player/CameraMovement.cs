using UnityEngine;

namespace MaiNull.Player
{
    public class CameraMovement : MonoBehaviour
    {
        [Header("Camera Movement")]
        [field: SerializeField] public float Sensibility { get; set; } = 2.5f;
        [SerializeField] float multiplier = 1;
        float mouseX, mouseY;
        float xRotation, yRotation;
        CameraPivot cameraPivot;

        [Header("Camera Roll")]
        [SerializeField] bool cameraRoll = true;
        [Range(1, 5)]
        [SerializeField] float angleLimit = 2;
        [SerializeField] float smooth = 20;
        float angle;

        private void OnEnable()
        {
            // Lock cursor
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            // Searches for any camera pivot to attach
            CameraPivot cameraPivot = FindFirstObjectByType<CameraPivot>();

            // Get the player camera position
            if (cameraPivot)
            {
                this.cameraPivot = cameraPivot;
                cameraPivot.attatchedCamera = transform;
            }
            else Debug.LogError("No camera pivot found");
        }

        void Update()
        {
            // Follows the player camera position
            transform.position = cameraPivot.transform.position;

            // Move camera
            CameraMove();
        }

        void CameraMove()
        {
            mouseX = Input.GetAxisRaw("Mouse X") * Sensibility * multiplier;
            mouseY = Input.GetAxisRaw("Mouse Y") * Sensibility * multiplier;

            //Vector3 rot = transform.rotation.eulerAngles;

            yRotation += mouseX;
            xRotation -= mouseY;

            xRotation = Mathf.Clamp(xRotation, -89, 89);

            if (cameraRoll)
            {
                // Camera rotation with roll
                transform.rotation = Quaternion.Euler(xRotation, yRotation, CameraRollVector());
            }
            else
            {
                // Camera rotation without roll
                transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            }
        }

        float CameraRollVector()
        {
            angle -= (Input.GetAxisRaw("Horizontal")) * smooth * Time.deltaTime;
            angle = Mathf.Clamp(angle, -angleLimit, angleLimit);

            if (Input.GetAxisRaw("Horizontal") != 0) return angle;

            if (angle > 0f)
            {
                angle -= smooth * Time.deltaTime;
                angle = Mathf.Clamp(angle, 0f, angleLimit);
            }
            else if (angle < 0f)
            {
                angle += smooth * Time.deltaTime;
                angle = Mathf.Clamp(angle, -angleLimit, 0f);
            }
            return angle;
        }
    }
}