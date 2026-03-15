using MaiNull.Player;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Settings : MonoBehaviour
{
    const string playerPrefVolume = "Volume";
    const string playerPrefSensibility = "sensibility";

    [SerializeField] Slider volumeSlider;
    [SerializeField] Slider sensibilitySlider;

    [SerializeField] TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;

    private void Awake()
    {
        // Volume
        volumeSlider.onValueChanged.AddListener(SetVolume);

        volumeSlider.minValue = 0;
        volumeSlider.maxValue = 1;
        volumeSlider.value = PlayerPrefs.GetFloat(playerPrefVolume, 0.25f);

        SetVolume(PlayerPrefs.GetFloat(playerPrefVolume, 0.25f));

        // Sensibility
        sensibilitySlider.onValueChanged.AddListener(SetSensibility);

        sensibilitySlider.minValue = 1;
        sensibilitySlider.maxValue = 100;
        sensibilitySlider.value = PlayerPrefs.GetFloat(playerPrefSensibility, 40);

        SetSensibility(PlayerPrefs.GetFloat(playerPrefSensibility, 40));

        // Resolution
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> resolutionsOptions = new List<string>();

        int curentResolutionindex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string resolution = resolutions[i].width + " x " + resolutions[i].height;

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
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
        PlayerPrefs.SetFloat(playerPrefVolume, value);
        PlayerPrefs.Save();
    }

    public void SetSensibility(float value)
    {
        PlayerPrefs.SetFloat(playerPrefSensibility, value);
        PlayerPrefs.Save();

        CameraMovement cameraMovement = FindAnyObjectByType<CameraMovement>();

        if (cameraMovement)
        {
            cameraMovement.Sensibility = value;
        }
    }

    public void SetResolution(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullScreen(bool b)
    {
        Screen.fullScreen = b;
    }
}
