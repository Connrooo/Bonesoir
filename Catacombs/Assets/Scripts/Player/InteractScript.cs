using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class InteractScript : MonoBehaviour
{
    Camera Cam;
    PlayerMotion playerMotion;
    PInputManager pInputManager;
    InventoryScript inventoryScript;
    [Header("Object Detection")]
    [SerializeField] private float rayLength = 5;
    [SerializeField] private LayerMask layerMaskInteract;
    [SerializeField] private string excludeLayerName = null;
    float deltaRep;
    [Header("Torch")]
    Vector3 candleAngle;
    float cAngleAmount = 10;
    [SerializeField] GameObject candle;
    [SerializeField] GameObject candleLight;
    [SerializeField] float lightingTime = 3;
    [SerializeField] float lightingCharge;
    int lightCheck;
    [Header("Money")]
    Vector3 moneyBagAngle;
    float mBAngleAmount = 10;
    [SerializeField] GameObject moneyBagEmpty;
    [SerializeField] GameObject moneyBagFull;
    [SerializeField] float collectTime = 5;
    [SerializeField] float collectCharge;
    int moneyCheck;
    bool stopRot;
    int x;

    void Awake()
    {
        pInputManager = FindObjectOfType<PInputManager>();
        inventoryScript = GetComponent<InventoryScript>();
        Cam = Camera.main;
    }

    public void InteractHandler()
    {
        InteractRaycast();
        candleFunction();
        collectFunction();
        animCount();
    }
    private void InteractRaycast()
    {
        moneyCheck = -1;
        lightCheck = -1;
        RaycastHit hit;
        Vector3 front = Cam.transform.TransformDirection(Vector3.forward);
        int mask = 1 << LayerMask.NameToLayer(excludeLayerName) | layerMaskInteract.value;
        Debug.DrawRay(Cam.transform.position, front, Color.green);
        if (Physics.Raycast(Cam.transform.position, front, out hit, rayLength, mask) && pInputManager.interactInput)
        {
            if (hit.collider.tag == "Door")
            {
                DoorScript doorScript = hit.collider.GetComponentInParent<DoorScript>();
                doorScript.doorOpen();
            }
            else if (hit.collider.tag == "Torch")
            {
                if (inventoryScript.invNumber == 2)
                {
                    lightCheck = 1;
                    if (lightingCharge >= lightingTime)
                    {
                        lightingCharge = lightingTime;
                        candleLight.SetActive(true);
                    }
                }
            }
            else if (hit.collider.tag == "Money")
            {

                if (inventoryScript.invNumber == 1)
                {
                    moneyCheck = 1;
                    if (collectCharge >= collectTime)
                    {
                        Destroy(hit.collider.gameObject);
                        inventoryScript.bagFull = true;
                    }
                }
            }
        }
    }
    private void candleFunction()
    {
        lightingCharge += Time.deltaTime * lightCheck;
        if (lightingCharge <= 0)
        {
            lightingCharge = 0;
        }
        candleAngle.x = lightingCharge * cAngleAmount;
        candleAngle.y = lightingCharge * -cAngleAmount;
    }

    private void collectFunction()
    {
        collectCharge += Time.deltaTime * moneyCheck;
        if (collectCharge <= 0)
        {
            collectCharge = 0;
        }
        moneyBagAngle.x = collectCharge * mBAngleAmount;
        moneyBagAngle.y = collectCharge * -mBAngleAmount;
        
    }

    private void animCount()
    {
        if (!stopRot)
        {
            moneyBagEmpty.transform.localRotation = Quaternion.Euler(moneyBagAngle);
            moneyBagFull.transform.localRotation = Quaternion.Euler(moneyBagAngle);
            candle.transform.localRotation = Quaternion.Euler(candleAngle);
            StartCoroutine(frameStop());
        }
    }

    IEnumerator frameStop()
    {
        stopRot = true;
        yield return new WaitForSeconds(1/12f);
        stopRot= false;
    }
}