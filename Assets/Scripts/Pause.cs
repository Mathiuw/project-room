using System;
using UnityEngine;
using UnityEngine.UI;

namespace MaiNull
{
    public class Pause : MonoBehaviour
    {
        public static Pause instance { get; private set; }

        [SerializeField] Transform menuBackground;
        [SerializeField] Transform menuPause;
        [SerializeField] Transform menuOptions;
        [SerializeField] Button resume;
        [SerializeField] Button exit;

        bool isPaused = false;
        public event Action<bool> onPause;

        void Awake() 
        {
            instance = this;

            AlternateSprites();
            resume.onClick.AddListener(SetPause);
            //if (ManagerGame.instance != null) exit.onClick.AddListener(ManagerGame.instance.ExitGame);
            exit.onClick.AddListener(Application.Quit);
        } 

        void Update() 
        {
            if (Input.GetKeyDown(KeyCode.Escape)) SetPause();
        }

        void AlternateSprites() 
        {
            menuBackground.gameObject.SetActive(isPaused);
            menuPause.gameObject.SetActive(isPaused);
            menuOptions.gameObject.SetActive(false);
        }

        void SetPause()
        {
            isPaused = !isPaused;

            AlternateSprites();

            if (isPaused)
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
            else 
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            onPause?.Invoke(isPaused);
        }
    }
}
