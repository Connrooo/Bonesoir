using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScript : MonoBehaviour
{
    [SerializeField] List<GameObject> objectsList;
    public int invNumber;
    public bool bagFull;
    private bool speedSlower;
    bool scrolled;
    PInputManager pInputManager;
    PlayerMotion playerMotion;
    AudioManager audioManager;
    [SerializeField] bool canSwitch = true;
    [SerializeField]GameObject candleLight;
    [SerializeField] Image candleCanvasImage;
    [SerializeField] Image bagCanvasImage;
    [SerializeField] Sprite[] candleSprites;
    [SerializeField] Sprite[] bagSprites;

    private void Awake()
    {
        pInputManager = FindObjectOfType<PInputManager>();
        playerMotion= FindObjectOfType<PlayerMotion>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        if (canSwitch)
        {
            Scroll();
        }
        invChecker();
        slowSpeed();
        if (scrolled)
        {
            PlayAudio();
        }
        spriteSelection();
    }

    private void Scroll()
    {
        if (pInputManager.scrollForward)
        {
            scrolled = true;
            invNumber++;
            if (invNumber > 2)
            {
                invNumber = 0;
            }
            StartCoroutine(switchTimer());
        }
        if (pInputManager.scrollBackward)
        {
            scrolled = true;
            invNumber--;
            if (invNumber < 0)
            {
                invNumber = 2;
            }
            StartCoroutine(switchTimer());
        }
    }

    private void invChecker()
    {
        
        foreach (GameObject x in objectsList)
        {
            x.SetActive(false);
        }
        switch (invNumber)
        {
            case 0:

                break;
            case 1:
                if (bagFull)
                {
                    objectsList[2].SetActive(true);
                }
                else
                {
                    objectsList[1].SetActive(true);
                }
                break;
            case 2:
                objectsList[3].SetActive(true);
                break;
        }
    }

    IEnumerator switchTimer()
    {
        canSwitch= false;
        yield return new WaitForSeconds(.5f);
        canSwitch = true;
    }

    private void PlayAudio()
    {
        int x = Random.Range(0, 2);
        scrolled = false;
        if (invNumber == 1)
        {
            audioManager.Stop("Candle Flame");
            string sound = x == 0 ? "Bag Zip #1" : "Bag Zip #2";
            audioManager.Play(sound);
            if (bagFull)
            {
                x = Random.Range(0, 2);
                sound = x == 0 ? "Coin Pull #1" : "Coin Pull #2";
                audioManager.Play(sound);
            }
        }
        if (invNumber == 2)
        {
            audioManager.Play("Candle Pull");
            if (candleLight.activeSelf)
            {
                audioManager.Play("Candle Flame");
            }
        }
        else { audioManager.Stop("Candle Flame"); }
    }

    private void slowSpeed()
    {
        if (!speedSlower&bagFull)
        {
            speedSlower = true;
            playerMotion.treasureHeld();
        }
    }

    private void spriteSelection()
    {
        if (bagFull) { bagCanvasImage.sprite = bagSprites[1]; }
        else bagCanvasImage.sprite = bagSprites[0];
        if (candleLight.activeSelf) { candleCanvasImage.sprite = candleSprites[1]; }
        else candleCanvasImage.sprite = candleSprites[0];
        switch (invNumber)
        {
            case 0:
                break;
            case 1:
                if (bagFull)
                {
                    bagCanvasImage.sprite = bagSprites[3];
                }
                else
                {
                    bagCanvasImage.sprite = bagSprites[2];
                }
                break;
            case 2:
                if (candleLight.activeSelf)
                {
                    candleCanvasImage.sprite = candleSprites[2];
                }
                else
                {
                    candleCanvasImage.sprite = candleSprites[3];
                }
                break;
        }
    }

}
