using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MaiNull
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UI_ShowSliderValue : MonoBehaviour
    {
        [SerializeField] Slider slider;
        [SerializeField] float multiplier;
        TextMeshProUGUI text;

        private void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        void Start()
        {
            slider.onValueChanged.AddListener(ShowValue);
            ShowValue(slider.value);
        }

        void ShowValue(float value) 
        {
            float valueMultiplied = value * multiplier;

            text.text = valueMultiplied.ToString("0");
        }
    }
}
