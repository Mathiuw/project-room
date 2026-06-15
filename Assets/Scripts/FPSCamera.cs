using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace MaiNull
{
    public class FPSCamera : MonoBehaviour
    {
        [SerializeField] private Transform orientation;
        
        [Header("Camera Movement")]
        [FormerlySerializedAs("Sensibility")][SerializeField] private float sensibility = 2.5f;
        [SerializeField] private float multiplier = 1;
        private float _xRotation, _yRotation;
        
        public float Sensibility { get => sensibility; set => sensibility = value; }
        public Vector2 MoveVector { get; set; }
        public float AngleValue { get; set; }
        public Transform Orientation { get => orientation; set => orientation = value; }

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

            if (Orientation) return;
            
            Orientation = GameObject.FindGameObjectWithTag("Orientation")?.transform;
            
            if (Orientation) {
                Player player = Orientation.GetComponentInParent<Player>();
                if (player) player.FPSCamera = this;
            }

            if (!Orientation) {
                Debug.LogError("Error finding components");
            }
        }

        private void Update()
        {
            // Move camera
            CameraMove();
            
            if(!Orientation) return;
            Orientation.rotation = transform.rotation;
        }

        private void LateUpdate()
        {
            // Follows the player camera position
            if(!Orientation) return;
            transform.position = Orientation.position;
        }

        private void CameraMove()
        {
            _xRotation -= MoveVector.y * Sensibility * multiplier;
            _xRotation = Mathf.Clamp(_xRotation, -89, 89);
            
            _yRotation += MoveVector.x * Sensibility * multiplier;

            // Camera rotation with roll
            transform.rotation = canRoll ? Quaternion.Euler(_xRotation, _yRotation, CameraRollVector()) :
                // Camera rotation without roll
                Quaternion.Euler(_xRotation, _yRotation, 0);
        }

        private float CameraRollVector()
        {
            // _angle -= moveVector.x * smooth * Time.deltaTime;
            _angle -= AngleValue * smooth * Time.deltaTime;
            _angle = Mathf.Clamp(_angle, -angleLimit, angleLimit);

            if (AngleValue != 0) return _angle;

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