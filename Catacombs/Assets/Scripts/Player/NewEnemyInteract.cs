using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NewEnemyInteract : MonoBehaviour
{
    AudioManager audioManager;
    PlayerMotion playerMotion;
    PInputManager pInputManager;
    Transform cameraObject;
    [Header("Grabbed")]
    public static bool grabbed;
    public bool escapeEnemy;
    [SerializeField] private float smoothTurnSpeed = 8;
    [Header("Captured Positions")]
    Vector3 currentPosition;
    Vector3 targetPosition;
    Quaternion currentRotation;
    Quaternion targetRotation;
    [Header("Candle Light")]
    [SerializeField] GameObject candleLight;

    [Header("Skull")]
    public GameObject skullObject;
    public bool skullAttacked;

    [Header("Snatcher")]
    bool isSnatcher;
    bool snatcherCanGrab;
    bool snatcherEscaped;
    Animator snatchAnim;
    GameObject legs;

    bool canJumpscare;
    // Start is called before the first frame update
    void Awake()
    {
        cameraObject = Camera.main.transform;
        playerMotion = FindObjectOfType<PlayerMotion>();
        pInputManager = FindObjectOfType<PInputManager>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (grabbed)
        {
            if (!escapeEnemy)
            {
                turnToEnemy();
            }
            else
            {
                returnRotation();
            }
        }
        if (skullAttacked)
        {
            grabbed = true;
            targetPosition = skullObject.transform.position;
            skullAttacked= false;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        switch (collision.transform.tag)
        {
            case "Walker":
                targetPosition = collision.transform.position;
                targetPosition.y = 2;
                break;
            case "Crawler":
                targetPosition = collision.transform.position;
                break;
        }
    }
    private void OnTriggerStay(Collider collision)
    {
        if (collision.transform.tag == "Snatcher")
        {
            targetPosition = collision.transform.position;
            snatcherCanGrab = collision.transform.GetComponent<Snatcher>().canGrab;
            legs = collision.transform.GetComponent<Snatcher>().LegModel;
            snatchAnim = collision.transform.GetComponent<Snatcher>().snatcherAnim;
            if (snatcherCanGrab)
            {
                snatcherGrab();
            }
        }
    }
    public void Caught()
    {
        currentRotation = cameraObject.transform.rotation;
        currentPosition = transform.position;
        targetRotation = Quaternion.LookRotation(targetPosition - cameraObject.transform.position);
        grabbed = true;
    }

    private void turnToEnemy()
    {
        if (!canJumpscare)
        {
            canJumpscare = true;
            audioManager.Play("Jumpscare");
        }
        cameraObject.transform.rotation = Quaternion.Slerp(cameraObject.transform.rotation, targetRotation, smoothTurnSpeed * Time.deltaTime);
        transform.position = currentPosition;
        if (isSnatcher)
        {
            SnatcherHeld();
        }

    }
    private void returnRotation()
    {
        canJumpscare = false;
        if (candleLight.activeSelf)
        {
            audioManager.Play("Candle Blow Out");
            candleLight.SetActive(false);
        }
        isSnatcher = false;
        snatcherEscaped = false;
        cameraObject.transform.rotation = Quaternion.Slerp(cameraObject.transform.rotation, currentRotation, smoothTurnSpeed * Time.deltaTime);
        if (cameraObject.transform.rotation == currentRotation)
        {
            grabbed = false;
            escapeEnemy = false;
        }
    }

    private void snatcherGrab()
    {
        if (!pInputManager.sprintInput&&snatcherCanGrab)
        {
            Caught();
            grabbed = true;
            isSnatcher = true;
        }
    }


    private void SnatcherHeld()
    {
        if (pInputManager.sprintInput && playerMotion.sprintToggle)
        {
            snatcherEscaped = true;
            escapeEnemy = true;
            if (snatcherEscaped)
            {
                legs.SetActive(false);
                snatchAnim.SetBool("attack", false);
            }
        }
    }
}
