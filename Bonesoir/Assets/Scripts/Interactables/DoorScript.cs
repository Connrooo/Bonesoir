using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField] Animator doorHingeAnim;
    [SerializeField] BoxCollider doorObject;
    AudioSource doorAudio;
    [SerializeField] AudioClip[] doorAudioClips;

    private void Awake()
    {
        doorHingeAnim = GetComponent<Animator>();
        doorAudio = GetComponent<AudioSource>();
    }

    public void doorOpen()
    {
        doorHingeAnim.SetBool("openDoor", true);
        doorHingeAnim.SetBool("doorWait", false);
        doorAudio.PlayOneShot(doorAudioClips[Random.Range(0, 4)]);
    }
}