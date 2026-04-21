using UnityEngine;
using UnityEngine.InputSystem;

namespace MaiNull
{
    public class CameraMovement : MonoBehaviour
    {
        [Header("Camera Movement")]
        [field: SerializeField] public float Sensibility { get; set; } = 2.5f;
        [SerializeField] private float multiplier = 1;
        private float _mouseX, _mouseY;
        private float _xRotation, _yRotation;
        private CameraPivot _cameraPivot;
        private  KinematicCharacterController _kinematicCharacterController;
        
        [Header("Camera Roll")]
        [SerializeField] private bool cameraRoll = true;
        [Range(1, 5)]
        [SerializeField] private float angleLimit = 2;
        [SerializeField] private float smooth = 20;
        private float _angle;

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
                _cameraPivot = cameraPivot;
                cameraPivot.attatchedCamera = transform;
                
                _kinematicCharacterController =  cameraPivot.GetComponentInParent<KinematicCharacterController>();
            }
            else Debug.LogError("No camera pivot found in scene");
        }

        private void Update()
        {
            // Follows the player camera position
            transform.position = _cameraPivot.transform.position;

            // Move camera
            CameraMove();
        }

        private void CameraMove()
        {
            _mouseX = Mouse.current.delta.ReadValue().x * Sensibility * multiplier;
            _mouseY = Mouse.current.delta.ReadValue().y * Sensibility * multiplier;

            //Vector3 rot = transform.rotation.eulerAngles;

            _yRotation += _mouseX;
            _xRotation -= _mouseY;

            _xRotation = Mathf.Clamp(_xRotation, -89, 89);

            // Camera rotation with roll
            transform.rotation = cameraRoll ? Quaternion.Euler(_xRotation, _yRotation, CameraRollVector()) :
                // Camera rotation without roll
                Quaternion.Euler(_xRotation, _yRotation, 0);
        }

        private float CameraRollVector()
        {
            Vector2 moveVector = _kinematicCharacterController?.InputMoveVector ?? Vector2.zero;
            
            _angle -= moveVector.x * smooth * Time.deltaTime;
            _angle = Mathf.Clamp(_angle, -angleLimit, angleLimit);

            if (moveVector.x != 0) return _angle;

            switch (_angle)
            {
                case > 0f:
                    _angle -= smooth * Time.deltaTime;
                    _angle = Mathf.Clamp(_angle, 0f, angleLimit);
                    break;
                case < 0f:
                    _angle += smooth * Time.deltaTime;
                    _angle = Mathf.Clamp(_angle, -angleLimit, 0f);
                    break;
            }
            return _angle;
        }
    }
}