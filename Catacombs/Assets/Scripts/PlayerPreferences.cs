using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerPreferences : MonoBehaviour
{
    AudioManager audioManager;
    public AudioMixer audioMixer;
    SettingsMenu settingsMenu;
    [SerializeField] GameObject settings;
    public const string _audioKey = "AudioKey";
    public const string _sensKey = "SensKey";
    public const string MIXER = "MIXER";


    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        settingsMenu = settings.GetComponent<SettingsMenu>();
        
        LoadSensitivity();
        LoadVolume();
    }


    public void LoadSensitivity()
    {
        float _sensitivity = PlayerPrefs.GetFloat(_sensKey, 1f);
        settingsMenu.sensitivitySlider.value = _sensitivity * 0.2f;
        SettingsMenu.sensMult = _sensitivity;
    }
    public void LoadVolume()
    {
        float temp = PlayerPrefs.GetFloat(_audioKey, 1f);
        audioMixer.SetFloat(MIXER, Mathf.Log10(temp) * 20);
        settingsMenu.soundsSlider.value = temp;
    }
}
