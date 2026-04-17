using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MaiNull
{
    public class PauseManager : MonoBehaviour
    {
        [SerializeField] private Transform pauseMenuCanvas;

        public static bool IsPaused { get; private set; } = false;
        
        public event Action<bool> OnPauseUpdate;
        
        private void Update()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame) TogglePause();
        }

        private void SpawnMenu() 
        {
            if (!pauseMenuCanvas) return;
            
            Instantiate(pauseMenuCanvas, Vector3.zero, Quaternion.identity);
        }

        private void TogglePause()
        {
            IsPaused = !IsPaused;

            SpawnMenu();

            if (IsPaused)
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
            else 
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            OnPauseUpdate?.Invoke(IsPaused);
        }
    }
}
