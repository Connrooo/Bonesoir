using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class PlayerMotion : MonoBehaviour
{
    PInputManager PInputManager;
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
    

    private void Awake()
    {
        PInputManager = GetComponent<PInputManager>();
        pRB = GetComponent<Rigidbody>();
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
        sprintIf();
    }
    private void Sprint()
    {
        if (PInputManager.sprintInput & sprintToggle)
        {
            movementSpeed = defaultSpeed * sprintMultiplier;
            isSprinting= true;
        }
        else
        {
            movementSpeed = defaultSpeed;
            isSprinting = false;
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
}
