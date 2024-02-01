using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    CameraScript cameraScript;
    [SerializeField] TMP_Dropdown dropdown;
    public Slider sensitivitySlider;
    public Slider soundsSlider;
    public AudioMixer audioMixer;
    Resolution[] resolutions;
    List<Resolution> filteredResolutions;
    RefreshRate currentRefreshRate;
    int currentResIndex = 0;
    public static float sensMult;


    void Start()
    {
        cameraScript=FindObjectOfType<CameraScript>();
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        dropdown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRateRatio;

        //Debug.Log("RefreshRate: " + currentRefreshRate);

        for (int i = 0; i < resolutions.Length; i++)
        {
            //Debug.Log("Resolutions: " + resolutions[i]);
            if (resolutions[i].refreshRateRatio.value == currentRefreshRate.value)
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }

        List<string> options = new List<string>();
        for (int i = 0; i< filteredResolutions.Count; i++)
        {
            string resolutionOption = filteredResolutions[i].width + "x" + filteredResolutions[i].height;
            options.Add(resolutionOption);
            if (i == filteredResolutions.Count-1)
            {
                currentResIndex = i;
            }
        }
        dropdown.AddOptions(options);
        dropdown.value= currentResIndex;
        dropdown.RefreshShownValue();

    }
    public void setResolution()
    {
        Resolution resolution = filteredResolutions[dropdown.value];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool value)
    {
        Screen.fullScreen = value;
    }
    private void OnEnable()
    {
        soundsSlider.onValueChanged.AddListener(SetMixerVolume);
        sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
    }

    private void OnDisable()
    {
        soundsSlider.onValueChanged.RemoveListener(SetMixerVolume);
        sensitivitySlider.onValueChanged.RemoveListener(SetSensitivity);
    }

    void SetSensitivity(float value)
    {
        sensMult = value * 5;
        if (sensMult<0.05)
        {
            sensMult = 0.05f;
        }
        PlayerPrefs.SetFloat("SensKey", sensMult);
    }

    void SetMixerVolume(float value)
    {
        if (value <= 0f)
        {
            audioMixer.SetFloat(PlayerPreferences.MIXER, -80);
        }
        else
        {
            audioMixer.SetFloat(PlayerPreferences.MIXER, Mathf.Log10(value) * 20);
        }
        PlayerPrefs.SetFloat("AudioKey", value);
    }

}
