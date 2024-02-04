using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    [SerializeField] Image[] heartSprites;
    [SerializeField] Sprite brokenHeartSprite;
    AudioSource audioSource;
    [SerializeField] AudioClip[] hurtSounds;
    public int health = 5;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void hurt()
    {
        heartSprites[--health].sprite = brokenHeartSprite;
        audioSource.PlayOneShot(hurtSounds[Random.Range(0, hurtSounds.Length)]);
        if (health<=0)
        {
            Debug.Log("dead");
        }
    }
}
