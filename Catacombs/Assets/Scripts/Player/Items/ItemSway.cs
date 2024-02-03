using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemSway : MonoBehaviour
{
    PInputManager pInputManager;
    PlayerMotion playerMotion;
    [SerializeField] GameObject itemHolder;
    [SerializeField] private float smoothSwing = 1f;
    [SerializeField] private float swayMultiplier = 1f;
    bool stopRot;
    Quaternion targetRotation;

    private void Awake()
    {
        pInputManager = FindObjectOfType<PInputManager>();
        playerMotion = GetComponent<PlayerMotion>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!NewEnemyInteract.grabbed)
        {
            Quaternion rotationX = Quaternion.AngleAxis(pInputManager.cameraInputY * swayMultiplier, Vector3.right);
            Quaternion rotationY = Quaternion.AngleAxis(-pInputManager.cameraInputX * swayMultiplier, Vector3.up);
            targetRotation = rotationX * rotationY;
        }
    }
    void FixedUpdate()
    {
        if (!stopRot)
        {
            itemHolder.transform.localRotation = Quaternion.Slerp(itemHolder.transform.localRotation, targetRotation, smoothSwing * Time.deltaTime);
            StartCoroutine(frameStop());
        }
    }

    IEnumerator frameStop()
    {
        stopRot= true;
        yield return new WaitForSeconds(1/12f);
        stopRot = false;
    }
}
