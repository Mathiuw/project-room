using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MaiNull.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private UIFade fade;
    
        [Header("Screens")]
        [SerializeField] RectTransform menu;
        [SerializeField] RectTransform options;
        [SerializeField] RectTransform loadingScreen;

        [Header("Buttoms")]
        [SerializeField] Button newGame;
        [SerializeField] Button exit;

        void Start() 
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            UIFade fade = Instantiate(this.fade, Vector3.zero, Quaternion.identity);
            fade.FadeOut();

            menu.gameObject.SetActive(true);
            options.gameObject.SetActive(false);
            loadingScreen.gameObject.SetActive(false);

            newGame.onClick.AddListener(StartGame);
            newGame.onClick.AddListener(SetLoadScreen);

            exit.onClick.AddListener(Application.Quit);
        }

        private void StartGame() 
        {
            SceneManager.LoadScene(1);
        }

        public void SetLoadScreen() 
        {
            loadingScreen.gameObject.SetActive(true);
            Destroy(menu.gameObject);
        }
    }
}
