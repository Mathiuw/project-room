using MaiNull.Singleton;
using System.Collections;
using UnityEngine;

public class SceneEnd : MonoBehaviour
{
    [SerializeField] private UI_Fade fade;

    IEnumerator Start() 
    {
        UI_Fade fade = Instantiate(this.fade, Vector3.zero, Quaternion.identity);
        fade.FadeOut();

        yield return new WaitForSeconds(5f);

        GameManager.Instance.SceneTransition(0);
    }  
}
