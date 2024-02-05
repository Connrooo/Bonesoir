using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class candleFacingScript : MonoBehaviour
{
    [SerializeField] GameObject Player;
    private void Update()
    {
        Quaternion lookRot;
        lookRot = Quaternion.LookRotation(transform.position - Player.transform.position);
        transform.rotation = lookRot;
    }
    private void Awake()
    {
        Player = GameObject.FindWithTag("Player");
    }
}
