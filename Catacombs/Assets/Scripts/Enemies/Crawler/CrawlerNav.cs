using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class CrawlerNav : MonoBehaviour
{
    [SerializeField] Transform Player;
    [SerializeField] NavMeshAgent Agent;
    [SerializeField] CrawlerAnim cAnimScript;
    bool inHole = true;
    [Header("Detection Range")]
    [SerializeField] Transform detectionPoint;
    [SerializeField] Quaternion crawlerStartRot;
    [SerializeField] float sphereSize = 10;
    [SerializeField] bool playerDetected;
    [Header("Build Up")]
    public float attackBuildUp;
    [SerializeField] float lastBuild;
    private bool turnToP;
    [Header("Speed")]
    [SerializeField] float crawlerSpeed = 3f;
    [Header("Follow Path")]
    [SerializeField] bool atPlayer;
    [SerializeField] bool chasing;
    [SerializeField] bool justChased;
    float chaseTimer;
    [SerializeField] Transform pipeHole;
    [Header("Grabbed Player")]
    [SerializeField] bool grabbedPlayer;
    public bool grabCooldown;
    [SerializeField] GameObject Legs;
    public GameObject LegModel;
    Quaternion inPos;
    [Header("Animation Values")]
    public bool aChasing;
    public bool aInRoom;
    public bool aAttack;
    public float aSpeed;
    private bool turningBack;

    void Start()
    {
        crawlerStartRot = transform.rotation;
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        Agent = GetComponent<NavMeshAgent>();
        Agent.speed = crawlerSpeed;
        inPos = Legs.transform.rotation;
        LegModel.SetActive(false);
    }
    private void Update()
    {
        stateCheck();
        if (turningBack)
        {
            rotToPipe();
        }
    }

    private void stateCheck()
    {
        if (chasing)
        {
            if (atPlayer)
            {
                freezeChaser();
            }
            else
            {
                chasingPlayer();
            }
        }
        else
        {
            if (justChased)
            {
                notChasing();
            }
            else
            {
                grabCooldown = true;
                lastBuild = attackBuildUp;
                largeSphereDetect();
                crawlerBuild();
            }
        }
    }

    private void largeSphereDetect()
    {
        Collider[] hitColliders = Physics.OverlapSphere(detectionPoint.position, sphereSize);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Player" && cAnimScript.inPipe)
            {
                attackBuildUp += Time.deltaTime;
                aSpeed = 1;
                turningBack = false;
                aInRoom = true;
            }
        }

    }
    private void crawlerBuild()
    {
        if (lastBuild==attackBuildUp)
        {
            attackBuildUp -= Time.deltaTime;
            aSpeed = -1f;
            if (attackBuildUp <= 0)
            {
                attackBuildUp = 0;
                aInRoom = false;
                aSpeed = 1f;
            }
        }
        if (attackBuildUp >= 10f)
        {
            grabCooldown = false;
            chasing = true;
            attackBuildUp = 0;
            chaseTimer = 0;
            StartCoroutine(finishedJump());
        }
    }

    IEnumerator finishedJump()
    {
        turnToP = true;
        cAnimScript.inPipe = false;
        yield return new WaitForSeconds(2 / 3f);
        turnToP = false;
    }

    private void chasingPlayer()
    {
        aInRoom = false;
        aChasing = true;
        inHole = false;
        if (!turnToP)
        {
            transform.LookAt(Player.transform.position);
        }
        Agent.destination = Player.transform.position;
        chaseTimer += Time.deltaTime;
        if (chaseTimer > 20) 
        {
            justChased = true;
            chasing = false;
        }
    }
    private void notChasing()
    {
        Agent.destination = pipeHole.position;
    }

    private void OnTriggerEnter(Collider col)
    {
        switch (col.tag)
        {
            case "pipeHole":
                aChasing = false;
                justChased = false;
                atPlayer = false;
                inHole = true;
                turningBack = true;
                break;
            case "Player":
                if (!inHole)
                {
                    chasing = true;
                    atPlayer = true;
                    StartCoroutine(playerSecured());
                }
                break;
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.tag == "pipeHole")
        {
            if (justChased)
            {
                aChasing = false;
                justChased = false;
                atPlayer = false;
                inHole = true;
                turningBack = true;
            }
        }
    }
    private void freezeChaser()
    {
        Agent.destination = transform.position;
    }

    private void rotToPipe()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, crawlerStartRot, 2.5f * Time.fixedDeltaTime);
    }
    private void LegsRot()
    {
        Vector3 lookPos = Player.transform.position;
        lookPos.y -= 2f;
        Legs.transform.LookAt(lookPos);
    }

    IEnumerator playerSecured()
    {
        //LegsRot();
        LegModel.SetActive(true);
        transform.LookAt(Player.transform.position);
        aAttack = true;
        yield return new WaitForSeconds(2);
        LegModel.SetActive(false);
        aAttack = false;
        Debug.Log("Damaged");
        justChased = true;
        chasing = false;
    }
}
