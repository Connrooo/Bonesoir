using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class WalkerNav : MonoBehaviour
{
    [SerializeField] Transform Player;
    [SerializeField] NavMeshAgent Agent;
    [SerializeField] float sphereSize = 10;
    [SerializeField] float rayLength = 50;
    [SerializeField] bool chasing;
    [SerializeField] float walkingSpeed = 1f;
    [SerializeField] float runningSpeed = 6f;
    [Header ("Default Path")]
    [SerializeField] bool toA;
    [SerializeField] bool toP;
    [SerializeField] bool pOnce;
    [SerializeField] Transform pathA;
    [SerializeField] Transform pathB;
    [Header("Grabbed Player")]
    [SerializeField] bool grabbedPlayer;
    public bool grabCooldown;
    Quaternion preGrabRot;
    [SerializeField] GameObject body;
    [Header("Animator")]
    [SerializeField] Animator walkerAnim;
    [SerializeField] animationTriggers animTriggers;
    bool playerAttacking;

    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        audioSource= GetComponent<AudioSource>();
        Agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!grabbedPlayer)
        {
            chasing = false;
            rayDetect();
            largeSphereDetect();
            smallSphereDetect();
            if (!chasing)
            {
                if (toP)
                {
                    pChase();
                }
                if (!toP)
                    notChasing();
            }
        }
        else
        {
            chasing = false;
            if (grabCooldown)
            {
                notChasing();
            }
            else
            {
                freezeWalker();
            }
        }
    }


    IEnumerator chaseCooldown()
    {
        yield return new WaitForSeconds(5);
        toP = false;
    }

    private void pChase()
    {
        if (pOnce)
        {
            pOnce = false;
            StartCoroutine(chaseCooldown());
        }
        else
        {
            walkerAnim.SetBool("Running", true);
            Agent.speed = runningSpeed;
            Agent.destination = Player.transform.position;
        }
    }

    private void notChasing()
    {
        walkerAnim.SetBool("Running", false);
        Agent.speed = walkingSpeed;
        if (toA)
        {
            Agent.destination = pathA.position;
        }
        if (!toA)
        {
            Agent.destination = pathB.position;
        }
    }

    private void chasingPlayer()
    {
        walkerAnim.SetBool("Running", true);
        Agent.speed = runningSpeed;
        Agent.destination = Player.transform.position;
        chasing = true;
        toP = true;
        pOnce = true;
    }
    private void rayDetect()
    {
        RaycastHit hit;
        Vector3 front = transform.TransformDirection(Vector3.forward);
        if (Physics.SphereCast(transform.position, 1, front, out hit, rayLength) && hit.transform.tag == "Player" && hasLineOfSight())
        {
            if (!Player.GetComponent<PlayerMotion>().isCrouching || chasing)
            {
                chasingPlayer();
            }
        }
    }

    private bool hasLineOfSight()
    {
        Physics.Raycast(transform.position, Player.transform.position - transform.position, out RaycastHit hit);
        if (hit.collider != null)
        {
            return hit.collider.CompareTag("Player");
        }
        return false;
    }

    private void largeSphereDetect()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sphereSize);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Player" && Player.GetComponent<PlayerMotion>().isSprinting)
            {
                chasingPlayer();
            }
        }
    }
    private void smallSphereDetect()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Player")
            {
                chasingPlayer();
            }
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        switch (col.tag)
        {
            case "pathA":
                toA = false;
                break;
            case "pathB":
                toA = true;
                break;
            case "Player":
                playerAttacking = true;
                if (!grabCooldown)
                {
                    grabbedPlayer = true;
                    toP = false;
                    StartCoroutine(playerSecured());
                }
                break;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            playerAttacking = false;
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player"&& playerAttacking == true)
        {
            if (!grabCooldown)
            {
                grabbedPlayer = true;
                toP = false;
                StartCoroutine(playerSecured());
            }
        }
    }

    private void freezeWalker()
    {
        Agent.destination = transform.position;
        transform.LookAt(Player.transform);
    }

    IEnumerator playerSecured()
    {
        audioSource.PlayOneShot(animTriggers.screamSounds[Random.Range(0, animTriggers.screamSounds.Length)]);
        playerAttacking = false;
        transform.position += .25f * Vector3.forward;
        walkerAnim.SetBool("Running", false);
        walkerAnim.SetBool("Attacking", true);
        yield return new WaitForSeconds(2);
        audioSource.Stop();
        transform.position -= .25f * Vector3.forward;
        grabCooldown = true;
        walkerAnim.SetBool("Attacking", false);
        yield return new WaitForSeconds(5);
        grabbedPlayer = false;
        grabCooldown = false;
    }

}
