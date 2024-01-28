using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerPreferences : MonoBehaviour
{
    AudioManager audioManager;
    SettingsMenu settingsMenu;
    public const string SOUNDS_KEY = "SoundsKey";
    public const string MIXER_SOUNDS = "SoundsVolume";


    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        settingsMenu = FindObjectOfType<SettingsMenu>();
    }


    public void LoadVolumes()
    {
        float soundsVolume = PlayerPrefs.GetFloat(SOUNDS_KEY, 1f);
        audioManager.audioMixer.SetFloat(MIXER_SOUNDS, Mathf.Log10(soundsVolume) * 20);
    }
}
