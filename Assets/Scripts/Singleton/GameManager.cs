using System;
using System.Collections;
using MaiNull.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MaiNull.Singleton
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private UIFade fade;

        private void Start()
        {
            // Start fade out
            UIFade newFade = Instantiate(this.fade, Vector3.zero, Quaternion.identity);
            newFade.FadeOut();
            
            // Restart level when player die
            Player.OnPlayerDie += RestartLevelTransition;
        }

        private void OnDisable()
        {
            Player.OnPlayerDie -= RestartLevelTransition;
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
            UIFade transitionFade = Instantiate(this.fade, Vector3.zero, Quaternion.identity);
            transitionFade.FadeIn();

            yield return transitionFade.OnFadeFinish;

            SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
        }
    }
}


