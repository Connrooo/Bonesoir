using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerAnim : MonoBehaviour
{
    [SerializeField] GameObject crawlerController;
    CrawlerNav crawlerNav;
    [SerializeField] Animator cAnimator;
    public bool finishedJumping;
    public bool inPipe;

    void Awake()
    {
        inPipe = true;
        cAnimator= GetComponent<Animator>();
        crawlerNav = crawlerController.GetComponent<CrawlerNav>();
    }

    private void Update()
    {
        cAnimator.SetBool("chasing", crawlerNav.aChasing);
        cAnimator.SetBool("in room",crawlerNav.aInRoom);
        cAnimator.SetBool("attack",crawlerNav.aAttack);
        cAnimator.SetFloat("speed",crawlerNav.aSpeed);
    }

    public void entered()
    {
        inPipe = true;
    }

}
