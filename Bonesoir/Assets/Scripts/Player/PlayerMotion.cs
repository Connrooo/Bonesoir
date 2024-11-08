using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Rendering;

public class PlayerMotion : MonoBehaviour
{
    PInputManager PInputManager;
    AudioManager audioManager;
    public Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody pRB;
    [Header("Player Speed")]
    [SerializeField] float defaultSpeed;
    [SerializeField] float sprintMultiplier;
    [SerializeField] float crouchMultiplier = .5f;
    float movementSpeed;
    [Header("Sprinting")]
    [SerializeField] float sprintCooldown; //The number of seconds the player can sprint for (and the cooldown until the player can sprint again)
    public bool sprintToggle = true; //Allows the player to sprint
    public bool isSprinting = false; //Checks if the player is currently Sprinting
    [SerializeField] Image sprintProgress; //Progress bar for sprinting
    float sprintTimer;
    bool spUpdate = false;
    [Header("Crouch Values")]
    public bool isCrouching;
    [SerializeField] CapsuleCollider playerCollider;
    [SerializeField] float crouchValue;

    [SerializeField] AudioClip[] stepSounds;
    [SerializeField] AudioClip[] runSounds;
    AudioSource playerAudioSource;
    [SerializeField] AudioSource sprintAudioSource;
    bool stopStepSound;
    float stepTime;
    private float startTime;


    private void Awake()
    {
        PInputManager = GetComponent<PInputManager>();
        audioManager = FindObjectOfType<AudioManager>();
        pRB = GetComponent<Rigidbody>();
        playerAudioSource= GetComponent<AudioSource>();
        cameraObject = Camera.main.transform;
        playerCollider = GetComponent<CapsuleCollider>();
        StartCoroutine(spProAnim());
    }
    public void MovementHandler()
    {
        if (!NewEnemyInteract.grabbed)
        {
            Crouch();
            Move();
        }
        if (NewEnemyInteract.grabbed)
        {
            SprintCooldown();
            sprintIf();
            isSprinting = false;
        }
    }
    private void Move()
    {
        moveDirection = cameraObject.forward * PInputManager.vertInput;
        moveDirection = moveDirection + cameraObject.right * PInputManager.horInput;
        moveDirection.Normalize();
        moveDirection.y = crouchValue;
        moveDirection = moveDirection * movementSpeed;

        Vector3 movementVelocity = moveDirection;
        pRB.velocity = movementVelocity;
        Sprint();
        SprintCooldown();
        if (PInputManager.vertInput == 0 & PInputManager.horInput == 0)
        {
            isSprinting = false;
        }
        else { PlayAudio(); }
        sprintIf();
    }
    private void Sprint()
    {
        if (PInputManager.sprintInput & sprintToggle)
        {
            movementSpeed = defaultSpeed * sprintMultiplier;
            isSprinting= true;
            if (!isSprintPlaying())
            {
                sprintAudioSource.clip = runSounds[Random.Range(0, runSounds.Length)];
                sprintAudioSource.Play();
                startTime = Time.time;
            }
        }
        else
        {
            movementSpeed = defaultSpeed;
            isSprinting = false;
            startTime = Time.time-4;
            sprintAudioSource.Stop();
        }
    }

    private void SprintCooldown()
    {
        if (spUpdate)
        {
            sprintProgress.fillAmount = 1 - (sprintTimer / sprintCooldown);
            spUpdate = false;
            StartCoroutine(spProAnim());
        }
        if (sprintTimer == sprintCooldown && PInputManager.sprintInput)
        {
            if (sprintToggle)
            {
            }
            sprintToggle = false;
        }
        if (!sprintToggle && sprintTimer==0 && !PInputManager.sprintInput)
        {
            sprintToggle= true;
        }
    }

    IEnumerator spProAnim()
    {
        yield return new WaitForSeconds(1/8f);
        spUpdate = true;
    }

    private void sprintIf()
    {
        if (isSprinting)
        {
            sprintTimer += Time.deltaTime;
        }
        if (!isSprinting)
        {
            sprintTimer -= Time.deltaTime;
        }
        sprintTimer = Mathf.Clamp(sprintTimer, 0, sprintCooldown);
    }
    private void Crouch()
    {
        if (PInputManager.crouchInput)
        {
            isCrouching= true;
            sprintToggle = false;
            movementSpeed = defaultSpeed * crouchMultiplier;
            playerCollider.height = 1f;
            RaycastHit hit;
            if (Physics.Raycast(transform.position,-transform.up, out hit, 0.51f))
            {
                crouchValue = 0f;
            }
            else
            {
                crouchValue = -1f;
            }
        }
        else
        {
            isCrouching= false;
            playerCollider.height = 2f;
            crouchValue = 0f;
        } 
    }
    public void treasureHeld()
    {
        defaultSpeed = defaultSpeed * .8f;
    }

    private void PlayAudio()
    {
        if (!stopStepSound)
        {
            if (isCrouching)
            {
                stepTime = 1.5f;
            }
            else if (isSprinting)
            {
                stepTime = .3f;
            }
            else
            {
                stepTime = .75f;
            }
            StartCoroutine(playStep());
        }
    }

    IEnumerator playStep()
    {
        stopStepSound= true;
        int x = Random.Range(0, stepSounds.Length);
        playerAudioSource.PlayOneShot(stepSounds[x]);
        yield return new WaitForSeconds(stepTime);
        stopStepSound = false;
    }

    public bool isSprintPlaying()
    {
        if ((Time.time - startTime) >= 3.1)
        {
            return false;
        }
        return true;
    }
}
