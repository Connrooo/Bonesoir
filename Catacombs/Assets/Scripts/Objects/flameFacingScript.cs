using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flameFacingScript : MonoBehaviour
{
    [SerializeField] GameObject Player;
    private void Update()
    {
        Quaternion lookRot;
        lookRot = Quaternion.LookRotation(transform.position-Player.transform.position);
        lookRot.x = 0;
        lookRot.z = 0;
        transform.rotation = lookRot;
    }
    private void Awake()
    {
        Player = GameObject.FindWithTag("MainCamera");
    }
}
