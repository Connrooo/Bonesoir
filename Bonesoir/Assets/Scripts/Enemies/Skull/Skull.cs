using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] GameObject skullVisual;
    [SerializeField] float skullRay = 2;
    [SerializeField] Animator skullAnim;
    //EnemyInteractions enemyInteractions;
    NewEnemyInteract newEnemyInteract;
    Vector3 lookAt;
    bool killIt;
    // Start is called before the first frame update
    private void Awake()
    {
        Player = GameObject.FindWithTag("Player");
        //enemyInteractions = Player.GetComponent<EnemyInteractions>();
        newEnemyInteract = Player.GetComponent<NewEnemyInteract>();
    }

    // Update is called once per frame
    void Update()
    {
        lookAt = Player.transform.position;
        lookAt.y = skullVisual.transform.position.y;
        skullVisual.transform.LookAt(lookAt);
        skullRaycast();
    }

    private void skullRaycast()
    {
        RaycastHit hit;
        Vector3 front = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position, front, Color.green);
        if (Physics.Raycast(transform.position, front, out hit, skullRay))
        {
            if (hit.collider.tag == "Player" && !killIt)
            {
                newEnemyInteract.skullObject = gameObject;
                killIt = true;
                StartCoroutine(skullAttack());
            }
        }
    }
    IEnumerator skullAttack()
    {
        Player.GetComponent<NewEnemyInteract>().enemyCount++;
        skullAnim.SetTrigger("attack");
        newEnemyInteract.skullAttacked = true;
        yield return new WaitForSeconds(2);
        Player.GetComponent<NewEnemyInteract>().enemyCount--;
        Destroy(gameObject);
    }
}
