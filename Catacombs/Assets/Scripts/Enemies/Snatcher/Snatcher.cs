using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class Snatcher : MonoBehaviour
{
    PlayerMotion PlayerMotion;
    GameObject Player;
    [SerializeField] bool grabbed = false;
    public Animator snatcherAnim;
    [SerializeField] GameObject Legs;
    public GameObject LegModel;
    public bool canGrab = true;
    Quaternion inPos;

    float coolDownTimer = 0;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerMotion = Player.GetComponent<PlayerMotion>();
        inPos = Legs.transform.rotation;
        LegModel.SetActive(false);
    }

    private void Update()
    {
        coolCheck();
        if (grabbed)
        {
            grabbed = false;
            StartCoroutine(playerSecured());
        }
    }
    private void OnTriggerStay(Collider col)
    {
        if (col.transform.tag == "Player" &&canGrab)
        {
            grabbed= true;
        }
    }

    private void LegsRot()
    {
        Vector3 lookPos = Player.transform.position;
        lookPos.y -= 1.8f;
        Legs.transform.LookAt(lookPos);
    }

    private void coolCheck()
    {
        if (coolDownTimer <= 0)
        {
            coolDownTimer = 0;
            if (!PlayerMotion.isSprinting)
            {
                canGrab = true;
            }
            else
            {
                canGrab= false;
            }
        }
        if (coolDownTimer > 0)
        {
            coolDownTimer -= Time.deltaTime;
        }
    }

    IEnumerator playerSecured()
    {
        LegsRot();
        LegModel.SetActive(true);
        snatcherAnim.SetBool("attack",true);
        canGrab = false;
        coolDownTimer = 6;
        yield return new WaitForSeconds(2);
        LegModel.SetActive(false);
        Legs.transform.rotation = inPos;
        snatcherAnim.SetBool("attack", false);
    }
}
