using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    [SerializeField] List<GameObject> objectsList;
    public int invNumber;
    public bool bagFull;
    private bool speedSlower;
    PInputManager pInputManager;
    PlayerMotion playerMotion;
    [SerializeField] bool canSwitch = true;

    private void Awake()
    {
        pInputManager = FindObjectOfType<PInputManager>();
        playerMotion= FindObjectOfType<PlayerMotion>();
    }

    private void Update()
    {
        if (canSwitch)
        {
            Scroll();
        }
        invChecker();
        slowSpeed();
    }

    private void Scroll()
    {
        if (pInputManager.scrollForward)
        {
            invNumber++;
            if (invNumber > 2)
            {
                invNumber = 0;
            }
            StartCoroutine(switchTimer());
        }
        if (pInputManager.scrollBackward)
        {
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

    private void slowSpeed()
    {
        if (!speedSlower&bagFull)
        {
            speedSlower = true;
            playerMotion.treasureHeld();
        }
    }
}
