using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpookySounds : MonoBehaviour
{
    [SerializeField] AudioSource Player;
    [SerializeField] AudioClip whisper;
    [SerializeField] AudioSource[] audioSources;
    [SerializeField] AudioClip[] _spookySounds;


    private void Awake()
    {
        StartCoroutine(playSound());
    }

    IEnumerator playSound()
    {
        yield return new WaitForSeconds(Random.Range(15, 60));
        Debug.Log("Spoopy");
        audioSources[Random.Range(0, audioSources.Length)].PlayOneShot(_spookySounds[Random.Range(0, _spookySounds.Length)]);
        yield return new WaitForSeconds(Random.Range(15, 60));
        Player.PlayOneShot(whisper);
        StartCoroutine(playSound());
    }
}
