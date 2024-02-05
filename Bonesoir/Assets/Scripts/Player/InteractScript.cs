using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class InteractScript : MonoBehaviour
{
    Camera Cam;
    PInputManager pInputManager;
    InventoryScript inventoryScript;
    AudioManager audioManager;
    DoorScript doorScript;
    PauseFunctions pauseFunctions;
    [Header("Object Detection")]
    [SerializeField] private float rayLength = 5;
    [SerializeField] private LayerMask layerMaskInteract;
    [SerializeField] private string excludeLayerName = null;
    [SerializeField] GameObject candleLight;
    [SerializeField] Animator itemHandler;
    bool interacted;

    void Awake()
    {
        doorScript = FindObjectOfType<DoorScript>();
        pInputManager = FindObjectOfType<PInputManager>();
        inventoryScript = GetComponent<InventoryScript>();
        audioManager = FindObjectOfType<AudioManager>();
        pauseFunctions = FindObjectOfType<PauseFunctions>();
        Cam = Camera.main;
    }

    public void InteractHandler()
    {
        InteractRaycast();
    }
    private void InteractRaycast()
    {
        itemHandler.SetBool("isLighting", false);
        itemHandler.SetBool("isCollecting", false);
        RaycastHit hit;
        Vector3 front = Cam.transform.TransformDirection(Vector3.forward);
        int mask = 1 << LayerMask.NameToLayer(excludeLayerName) | layerMaskInteract.value;
        Debug.DrawRay(Cam.transform.position, front, Color.green);
        if (Physics.Raycast(Cam.transform.position, front, out hit, rayLength, mask) && pInputManager.interactInput)
        {
            switch (hit.collider.tag)
            {
                case "Torch":
                    if (inventoryScript.invNumber == 2 && candleLight.activeSelf == false)
                    {
                        itemHandler.SetBool("isLighting", true);
                    }
                    break;
                case "Money":
                    if (inventoryScript.bagFull == true)
                    {
                        doorScript.doorOpen();
                        Destroy(hit.collider.gameObject);
                    }
                    else if (inventoryScript.invNumber == 1)
                    {
                        itemHandler.SetBool("isCollecting", true);
                        if (!interacted)
                        {
                            PlayAudio();
                        }
                    }
                    break;
            }
        }
        if (!itemHandler.GetBool("isCollecting"))
        {
            audioManager.Stop("Treasure Collect");
            interacted = false;
        }
    }
    private void PlayAudio()
    {
        interacted = true;
        audioManager.Play("Treasure Collect");
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Exit")
        {
            pauseFunctions.Survived();
        }
    }

}