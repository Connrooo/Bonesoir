using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemsAnimationFinished : MonoBehaviour
{
    InventoryScript inventoryScript;
    AudioManager audioManager;
    [SerializeField] GameObject candleLight;
    private void Awake()
    {
        inventoryScript = FindObjectOfType<InventoryScript>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void torchFinished()
    {
        candleLight.SetActive(true);
        audioManager.Play("Candle Flame");
    }
    public void bagFinished()
    {
        inventoryScript.bagFull = true;
    }
}
