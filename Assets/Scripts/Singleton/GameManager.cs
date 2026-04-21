using System.Collections;
using MaiNull.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MaiNull.Singleton
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] UIFade fade;

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
            UIFade fade = Instantiate(this.fade, Vector3.zero, Quaternion.identity);
            fade.FadeOut();

            // Restart level when player die
            Player player = FindFirstObjectByType<Player>();

            if (player)
            {
                player.Health.OnDie += RestartLevelTransition;
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
            UIFade fade = Instantiate(this.fade, Vector3.zero, Quaternion.identity);
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


