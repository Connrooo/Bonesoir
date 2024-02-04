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
        StartCoroutine(doorTimer());
        StartCoroutine(doorHitboxTimer());
    }
    public void doorClose() 
    {
        doorHingeAnim.SetBool("doorWait", true);
        doorAudio.PlayOneShot(doorAudioClips[Random.Range(4, doorAudioClips.Length)]);
        StartCoroutine(doorHitboxTimer());
    }

    IEnumerator doorHitboxTimer()
    {
        doorObject.enabled = false;
        yield return new WaitForSeconds(1);
        doorObject.enabled = true;
    }

    IEnumerator doorTimer()
    {
        yield return new WaitForSeconds(5);
        doorHingeAnim.SetBool("openDoor", false);
        doorClose();
    }
}