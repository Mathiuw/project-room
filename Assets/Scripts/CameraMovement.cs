using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace MaiNull
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private Transform orientation;
        [SerializeField] private  KinematicCharacterController kinematicCharacterController;
        
        [Header("Camera Movement")]
        [FormerlySerializedAs("Sensibility")][SerializeField] private float sensibility = 2.5f;
        [SerializeField] private float multiplier = 1;
        private float _mouseX, _mouseY;
        private float _xRotation, _yRotation;
        
        public float Sensibility { get => sensibility; set => sensibility = value; }
        
        [Header("Camera Roll")]
        [SerializeField] private bool canRoll = true;
        [Range(1, 5)]
        [SerializeField] private float angleLimit = 2;
        [SerializeField] private float smooth = 20;
        private float _angle;

        private void OnEnable()
        {
            // Lock cursor
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            if (orientation) return;
            
            orientation = GameObject.FindGameObjectWithTag("Orientation")?.transform;
            
            if (orientation)
            { 
                if (kinematicCharacterController) return;
                
                kinematicCharacterController =  orientation.GetComponentInParent<KinematicCharacterController>();
            }

            if (!orientation || !kinematicCharacterController)
            {
                Debug.LogError("Error finding components");
            }
        }

        private void Update()
        {
            // Move camera
            CameraMove();
            
            orientation.rotation = transform.rotation;
        }

        private void LateUpdate()
        {
            // Follows the player camera position
            transform.position = orientation.position;
        }

        private void CameraMove()
        {
            _mouseX = Mouse.current.delta.ReadValue().x * Sensibility * multiplier;
            _mouseY = Mouse.current.delta.ReadValue().y * Sensibility * multiplier;

            //Vector3 rot = transform.rotation.eulerAngles;
            _xRotation -= _mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -89, 89);
            _yRotation += _mouseX;

            // Camera rotation with roll
            transform.rotation = canRoll ? Quaternion.Euler(_xRotation, _yRotation, CameraRollVector()) :
                // Camera rotation without roll
                Quaternion.Euler(_xRotation, _yRotation, 0);
        }

        private float CameraRollVector()
        {
            Vector2 moveVector = kinematicCharacterController?.InputMoveVector ?? Vector2.zero;
            
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