using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PInputManager PInputManager;
    CameraScript CameraScript;
    PlayerMotion PlayerMotion;
    InteractScript InteractScript;

    private void Awake()
    {
        PInputManager = GetComponent<PInputManager>();
        CameraScript = FindObjectOfType<CameraScript>();
        PlayerMotion = GetComponent<PlayerMotion>();
        InteractScript = GetComponent<InteractScript>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        PInputManager.InputHandler();
        PlayerMotion.MovementHandler();
        InteractScript.InteractHandler();
    }

    private void LateUpdate()
    {
        CameraScript.CameraFunction();
    }
}
