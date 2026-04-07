using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MaiNull.Singleton
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] UI_Fade fade;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            // Start fade out
            UI_Fade fade = Instantiate(this.fade, Vector3.zero, Quaternion.identity);
            fade.FadeOut();

            // Restart level when player die
            Player.Player player = FindFirstObjectByType<Player.Player>();

            if (player)
            {
                player.health.OnDie += RestartLevelTransition;
            }
        }

        public void RestartLevelTransition()
        {
            SceneTransition(SceneManager.GetActiveScene().buildIndex);
        }

        public void SceneTransition(int sceneIndex)
        {
            StartCoroutine(SceneTransitionCoroutine(sceneIndex));
        }

        private IEnumerator SceneTransitionCoroutine(int sceneIndex)
        {
            UI_Fade fade = Instantiate(this.fade, Vector3.zero, Quaternion.identity);
            fade.FadeIn();

            while (fade.alpha < 1)
            {
                yield return null;
            }

            SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);

            yield break;
        }
    }
}


