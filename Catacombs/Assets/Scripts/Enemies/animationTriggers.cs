using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class animationTriggers : MonoBehaviour
{
    NewEnemyInteract newEnemyInteract;
    HealthScript healthScript;
    AudioManager audioManager;
    public AudioSource audioSource;
    [SerializeField] AudioClip[] stepSounds;
    [SerializeField] AudioClip[] spasmSounds;
    [SerializeField] AudioClip[] creakSounds;
    [SerializeField] AudioClip[] crackSounds;
    [SerializeField] AudioClip[] jawSounds;
    [SerializeField] AudioClip[] crunchSounds;
    [SerializeField] AudioClip[] mouthSounds;
    public AudioClip[] screamSounds;
    [SerializeField] LayerMask layerMask;
    GameObject Player;

    private void Awake()
    {
        newEnemyInteract = FindObjectOfType<NewEnemyInteract>();
        healthScript = FindObjectOfType<HealthScript>();
        audioManager = FindObjectOfType<AudioManager>();
        audioSource = GetComponent<AudioSource>();
        Player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(RandomMouth());
    }

    private void FixedUpdate()
    {
        if (hasLineOfSight())
        {
            audioSource.volume = 0.5f;
        }
        else { audioSource.volume = 0; }
    }

    public bool hasLineOfSight()
    {
        int mask = layerMask;
        Physics.Raycast(transform.position, Player.transform.position - transform.position, out RaycastHit hit,100 ,mask);
        if (hit.collider != null)
        {
            return hit.collider.CompareTag("Player");
        }
        return false;
    }
    private void AttackPlayer()
    {
        newEnemyInteract.Caught();
    }
    private void LeavePlayer()
    {
        healthScript.hurt();
        newEnemyInteract.escapeEnemy = true;
    }

    private void StepNoise()
    {
        audioSource.PlayOneShot(stepSounds[Random.Range(0, stepSounds.Length)]);
    }
    private void SpasmNoise()
    {
        audioSource.PlayOneShot(spasmSounds[Random.Range(0, spasmSounds.Length)]);
    }
    private void CreakNoise()
    {
        audioSource.PlayOneShot(creakSounds[Random.Range(0, creakSounds.Length)]);
    }
    private void CrackNoise()
    {
        audioSource.PlayOneShot(crackSounds[Random.Range(0, crackSounds.Length)]);
    }
    private void JawNoise()
    {
        audioSource.volume = 0.2f;
        audioSource.PlayOneShot(jawSounds[Random.Range(0, jawSounds.Length)]);
    }
    private void CrunchNoise()
    {
        audioSource.volume = 1.5f;
        audioSource.PlayOneShot(crunchSounds[Random.Range(0, crunchSounds.Length)]);
    }
    IEnumerator RandomMouth()
    {
        int x = Random.Range(5, 20);
        audioSource.PlayOneShot(mouthSounds[Random.Range(0, mouthSounds.Length)]);
        yield return new WaitForSeconds(x);
        StartCoroutine(RandomMouth());
    }
}
