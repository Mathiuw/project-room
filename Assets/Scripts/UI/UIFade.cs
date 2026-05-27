using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MaiNull.UI
{
    public enum EFadeType
    {
        FadeIn, FadeOut
    }

    public class UIFade : MonoBehaviour
    {
        [SerializeField] private EFadeType eFadeType = EFadeType.FadeIn;
        [SerializeField] private bool activateOnStart = false;
        [SerializeField] private float fadeTime = 1f;
        [SerializeField] private AnimationCurve curve;
        private Image _image;

        public float Alpha { get; private set; }

        public Action OnFadeFinish;
        
        private void Start()
        {
            if (!activateOnStart) return;

            switch (eFadeType)
            {
                case EFadeType.FadeIn:
                    FadeIn();
                    break;
                case EFadeType.FadeOut:
                    FadeOut();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Awake() 
        {
            _image= GetComponentInChildren<Image>();
        }

        public void SetImageAlphaValue(float value) 
        {
            Color color = _image.color;
            color.a = value;
        
            Alpha = color.a;
            _image.color = color;
        }

        public void FadeIn() => StartCoroutine(FadeCoroutine(0, 1));

        public void FadeOut() => StartCoroutine(FadeCoroutine(1, 0));

        private IEnumerator FadeCoroutine(float initial,float final) 
        {
            float timePassed = 0;

            SetImageAlphaValue(initial);
            while (timePassed < fadeTime)
            {
                SetImageAlphaValue(curve.Evaluate(Mathf.Lerp(initial, final, timePassed)));
                timePassed += (Time.deltaTime / fadeTime);

                yield return null;
            }
            SetImageAlphaValue(final);
            
            OnFadeFinish?.Invoke();
        }
    }
}