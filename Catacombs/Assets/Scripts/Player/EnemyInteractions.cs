using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyInteractions : MonoBehaviour
{
    CameraScript CameraScript;
    PlayerMotion PlayerMotion;
    Transform cameraObject;
    PInputManager pInputManager;
    [Header("Enemy Turn")]
    [SerializeField] private float smoothTurnSpeed = 2;
    Quaternion targetRotation;
    Vector3 targetPosition;
    private bool turnStart;
    Quaternion currentRotation;
    Vector3 currentPos;
    private bool returnR;
    [Header("Snatcher")]
    [SerializeField] bool snatchGrabbed;
    [SerializeField] bool snatchPresence;
    bool snatchCanGrab;
    bool snatchEscaped;
    Animator snatchAnim;
    GameObject legs;
    [Header("Skull")]
    public bool skullAttacked;
    public GameObject skullObject;
    [Header("Candle Light")]
    [SerializeField] GameObject candleLight;
    // Start is called before the first frame update
    void Awake()
    {
        CameraScript = FindObjectOfType<CameraScript>();
        PlayerMotion = GetComponent<PlayerMotion>();
        cameraObject = Camera.main.transform;
        pInputManager = FindObjectOfType<PInputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (turnStart)
        {
            turnToEnemy();
        }
        if (returnR)
        {
            returnRotation();
        }
        if (snatchPresence)
        {
            snatcherGrab();
        }
        if (skullAttacked)
        {
            StartCoroutine(skullGrab());
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        switch (collision.transform.tag)
        {
            case "Walker":
                if (!collision.GetComponent<WalkerNav>().grabCooldown)
                {
                    CameraScript.grabbed = true;
                    PlayerMotion.grabbed = true;
                    currentPos = transform.position;
                    currentRotation = cameraObject.transform.rotation;
                    targetPosition = collision.transform.position;
                    targetPosition.y++;
                    targetRotation = Quaternion.LookRotation(targetPosition - cameraObject.transform.position);
                    targetRotation.x = 0;
                    targetRotation.z = 0;
                    StartCoroutine(enemyGrabC());
                }
                break;
            case "Crawler":
                if (!collision.GetComponent<CrawlerNav>().grabCooldown)
                {
                    CameraScript.grabbed = true;
                    PlayerMotion.grabbed = true;
                    currentPos = transform.position;
                    currentRotation = cameraObject.transform.rotation;
                    targetPosition = collision.transform.position;
                    targetPosition.y = -10f;
                    targetRotation = Quaternion.LookRotation(targetPosition - cameraObject.transform.position);
                    targetRotation.z = 0;
                    StartCoroutine(enemyGrabC());
                }
                break;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.transform.tag == "Snatcher")
        {
            snatchPresence = true;
            snatchCanGrab = collision.transform.GetComponent<Snatcher>().canGrab;
            legs = collision.transform.GetComponent<Snatcher>().LegModel;
            snatchAnim = collision.transform.GetComponent<Snatcher>().snatcherAnim;
            if (!snatchGrabbed)
            {
                currentPos = transform.position;
                targetPosition = collision.transform.position;
                targetPosition.y = -1f;
                targetRotation = Quaternion.LookRotation(targetPosition - cameraObject.transform.position);
                targetRotation.x = 0.65f;
                targetRotation.w = 0.75f;
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.transform.tag == "Snatcher")
        {
            snatchPresence = false;
        }
    }

    private void turnToEnemy()
    {
        cameraObject.transform.rotation = Quaternion.Slerp(cameraObject.transform.rotation, targetRotation, smoothTurnSpeed * Time.deltaTime);
        transform.position = currentPos;
    }
    private void returnRotation()
    {
        cameraObject.transform.rotation = Quaternion.Slerp(cameraObject.transform.rotation, currentRotation, smoothTurnSpeed * Time.deltaTime);
    }

    IEnumerator enemyGrabC()
    {
        candleLight.SetActive(false);
        turnStart = true;
        yield return new WaitForSeconds(2);
        returnR = true;
        turnStart = false;
        yield return new WaitForSeconds(smoothTurnSpeed * Time.deltaTime * 20);
        returnR = false;
        CameraScript.grabbed = false;
        PlayerMotion.grabbed = false;
    }

    IEnumerator snatchTurnTo()
    {
        turnStart = true;
        CameraScript.grabbed = true;
        PlayerMotion.grabbed = true;
        yield return new WaitForSeconds(smoothTurnSpeed * Time.deltaTime);
        StartCoroutine(snatchHurt());
    }
    IEnumerator snatchTurnAway()
    {
        turnStart = false;
        returnR = true;
        legs.SetActive(false);
        snatchAnim.SetBool("attack", false);
        yield return new WaitForSeconds(smoothTurnSpeed * Time.deltaTime * 20);
        returnR = false;
        CameraScript.grabbed = false;
        PlayerMotion.grabbed = false;
    }
    IEnumerator snatchHurt()
    {
        yield return new WaitForSeconds(2);
        if (!snatchEscaped && snatchGrabbed)
        {
            Debug.Log("Damaged");
            snatchEscaped = false;
            snatchGrabbed = false;
            StartCoroutine(snatchTurnAway());
        }
    }
    private void snatcherGrab()
    {
        if (!pInputManager.sprintInput && snatchCanGrab)
        {
            snatchEscaped = false;
            if (!snatchGrabbed)
            {
                candleLight.SetActive(false);
                currentRotation = cameraObject.transform.rotation;
                snatchGrabbed = true;
                StartCoroutine(snatchTurnTo());
            }
        }
        else if (pInputManager.sprintInput && PlayerMotion.sprintToggle)
        {
            if (snatchGrabbed)
            {
                snatchEscaped = true;
                snatchGrabbed = false;
                StartCoroutine(snatchTurnAway());
            }
        }
    }

    IEnumerator skullGrab()
    {
        candleLight.SetActive(false);
        skullAttacked = false;
        CameraScript.grabbed = true;
        PlayerMotion.grabbed = true;
        currentPos = transform.position;
        currentRotation = cameraObject.transform.rotation;
        targetPosition = skullObject.transform.position;
        targetPosition.y = 0f;
        targetRotation = Quaternion.LookRotation(targetPosition - cameraObject.transform.position);
        targetRotation.x = 0;
        targetRotation.z = 0;
        turnStart = true;
        yield return new WaitForSeconds(2);
        returnR = true;
        turnStart = false;
        yield return new WaitForSeconds(smoothTurnSpeed * Time.deltaTime * 20);
        returnR = false;
        CameraScript.grabbed = false;
        PlayerMotion.grabbed = false;
    }
}
