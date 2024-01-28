using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0, 1)]
    public float volume = 1;
    [Range(.0f, 3)]
    public float pitch = 1;
    public bool loop;
    public bool playOnAwake;

    [HideInInspector] public AudioSource audioSource;
    public AudioMixerGroup mixerGroup;
}