using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class PlayerPreferences : MonoBehaviour
{
    AudioManager audioManager;
    public AudioMixer audioMixer;
    SettingsMenu settingsMenu;
    [SerializeField] GameObject settings;
    public const string _audioKey = "AudioKey";
    public const string _sensKey = "SensKey";
    public const string MIXER = "MIXER";
    float _sensitivity;


    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        settingsMenu = settings.GetComponent<SettingsMenu>();

        LoadSensitivity();
        LoadVolume();
    }


    public void LoadSensitivity()
    {
        if (PlayerPrefs.HasKey("SensKey"))
        {
            _sensitivity = PlayerPrefs.GetFloat(_sensKey, 1f);
            settingsMenu.sensitivitySlider.value = _sensitivity * 0.2f;
            SettingsMenu.sensMult = PlayerPrefs.GetFloat("SensKey");
        }
        else
        {
            PlayerPrefs.SetFloat("SensKey", 0.2f);
            _sensitivity = PlayerPrefs.GetFloat(_sensKey, 1f);
            settingsMenu.sensitivitySlider.value = _sensitivity*0.2f;
            SettingsMenu.sensMult = PlayerPrefs.GetFloat("SensKey");
        }
    }
    public void LoadVolume()
    {
        if (PlayerPrefs.HasKey("AudioKey"))
        {
            float volume = PlayerPrefs.GetFloat("AudioKey", 1f);
            audioMixer.SetFloat(MIXER, Mathf.Log10(volume) * 20);
            settingsMenu.soundsSlider.value = PlayerPrefs.GetFloat(_audioKey, 1f);
        }
        else
        {
            PlayerPrefs.SetFloat("AudioKey", 1f);
            float volume = PlayerPrefs.GetFloat("AudioKey", 1f);
            audioMixer.SetFloat(MIXER, Mathf.Log10(volume) * 20);
            settingsMenu.soundsSlider.value = PlayerPrefs.GetFloat(_audioKey, 1f);
        }
    }
}
