using System.Collections;
using MaiNull.Singleton;
using MaiNull.UI;
using UnityEngine;

namespace MaiNull
{
    public class SceneEnd : MonoBehaviour
    {
        [SerializeField] private UIFade fade;

        IEnumerator Start() 
        {
            UIFade fade = Instantiate(this.fade, Vector3.zero, Quaternion.identity);
            fade.FadeOut();

            yield return new WaitForSeconds(5f);

            GameManager.Instance.SceneTransition(0);
        }  
    }
}
