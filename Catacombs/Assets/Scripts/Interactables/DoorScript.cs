using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField] Animator doorHingeAnim;
    [SerializeField] BoxCollider doorObject;

    private void Awake()
    {
        doorHingeAnim = GetComponent<Animator>();
    }

    public void doorOpen()
    {
        doorHingeAnim.SetBool("openDoor", true);
        doorHingeAnim.SetBool("doorWait", false);
        StartCoroutine(doorTimer());
        StartCoroutine(doorHitboxTimer());
    }
    public void doorClose() 
    {
        doorHingeAnim.SetBool("doorWait", true);
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