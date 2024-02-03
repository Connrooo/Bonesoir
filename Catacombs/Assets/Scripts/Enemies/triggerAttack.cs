using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerAttack : MonoBehaviour
{
    NewEnemyInteract newEnemyInteract;
    HealthScript healthScript;

    private void Awake()
    {
        newEnemyInteract = FindObjectOfType<NewEnemyInteract>();
        healthScript = FindObjectOfType<HealthScript>();
    }
    private void AttackPlayer()
    {
        newEnemyInteract.Caught();
    }
    private void LeavePlayer()
    {
        Debug.Log("!");
        healthScript.hurt();
        newEnemyInteract.escapeEnemy = true;
    }
}
