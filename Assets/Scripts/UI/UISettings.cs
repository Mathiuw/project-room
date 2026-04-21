using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MaiNull.UI
{
    public class UISettings : MonoBehaviour
    {
        private const string PlayerPrefVolume = "Volume";
        private const string PlayerPrefSensibility = "sensibility";

        [SerializeField] private Slider volumeSlider;
        [SerializeField] private Slider sensibilitySlider;

        [SerializeField] private TMP_Dropdown resolutionDropdown;
        private Resolution[] _resolutions;

        private void Awake()
        {
            // Volume
            volumeSlider.onValueChanged.AddListener(SetVolume);

            volumeSlider.minValue = 0;
            volumeSlider.maxValue = 1;
            volumeSlider.value = PlayerPrefs.GetFloat(PlayerPrefVolume, 0.25f);

            SetVolume(PlayerPrefs.GetFloat(PlayerPrefVolume, 0.25f));

            // Sensibility
            sensibilitySlider.onValueChanged.AddListener(SetSensibility);

            sensibilitySlider.minValue = 1;
            sensibilitySlider.maxValue = 100;
            sensibilitySlider.value = PlayerPrefs.GetFloat(PlayerPrefSensibility, 40);

            SetSensibility(PlayerPrefs.GetFloat(PlayerPrefSensibility, 40));

            // Resolution
            _resolutions = Screen.resolutions;

            resolutionDropdown.ClearOptions();

            List<string> resolutionsOptions = new List<string>();

            int curentResolutionindex = 0;

            for (int i = 0; i < _resolutions.Length; i++)
            {
                string resolution = _resolutions[i].width + " x " + _resolutions[i].height;

                if (_resolutions[i].width == Screen.currentResolution.width && _resolutions[i].height == Screen.currentResolution.height)
                {
                    curentResolutionindex = i;
                }

                resolutionsOptions.Add(resolution);
            }

            resolutionDropdown.AddOptions(resolutionsOptions);

            resolutionDropdown.value = curentResolutionindex;
            resolutionDropdown.RefreshShownValue();
            SetResolution(curentResolutionindex);
        }

        public void SetVolume(float value)
        {
            AudioListener.volume = value;
            PlayerPrefs.SetFloat(PlayerPrefVolume, value);
            PlayerPrefs.Save();
        }

        public void SetSensibility(float value)
        {
            PlayerPrefs.SetFloat(PlayerPrefSensibility, value);
            PlayerPrefs.Save();

            CameraMovement cameraMovement = FindAnyObjectByType<CameraMovement>();

            if (cameraMovement)
            {
                cameraMovement.Sensibility = value;
            }
        }

        public void SetResolution(int index)
        {
            Resolution resolution = _resolutions[index];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        public void SetFullScreen(bool b)
        {
            Screen.fullScreen = b;
        }
    }
}