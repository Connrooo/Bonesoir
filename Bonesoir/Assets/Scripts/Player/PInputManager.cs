using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PInputManager : MonoBehaviour
{
    [Header("Input Start Up")]
    ISInputSystem PlayerInput;

    [Header("Movement Controls")]
    [SerializeField] Vector2 movementInput;
    [Header("Camera Controls")]
    [SerializeField] Vector2 cameraInput;
    [Header("Camera Values")]
    public float cameraInputX;
    public float cameraInputY;
    [Header("Interact Button")]
    public bool interactInput;
    [Header("Sprint Button")]
    public bool sprintInput;
    [Header("Crouch Button")]
    public bool crouchInput;

    [Header("Vert/Hor Input")]
    public float vertInput;
    public float horInput;

    [Header("Scroll Inputs")]
    public bool scrollForward;
    public bool scrollBackward;

    [Header("Pause Menu Controls")]
    public bool pauseButton;
    public bool unpauseButton;

    private void OnEnable()
    {
        if (PlayerInput == null)
        {
            PlayerInput = new ISInputSystem();
            PlayerInput.Main.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            PlayerInput.Main.Turn.performed += i => cameraInput = i.ReadValue<Vector2>();
            PlayerInput.Main.Interact.performed += i => interactInput = true;
            PlayerInput.Main.Interact.canceled += i => interactInput = false;
            PlayerInput.Main.Sprint.performed += i => sprintInput = true;
            PlayerInput.Main.Sprint.canceled += i => sprintInput = false;
            PlayerInput.Main.Crouch.performed += i => crouchInput = true;
            PlayerInput.Main.Crouch.canceled += i => crouchInput = false;
            PlayerInput.Main.InvScrollForward.performed += i => scrollForward = true;
            PlayerInput.Main.InvScrollForward.canceled += i => scrollForward = false;
            PlayerInput.Main.InvScrollBackward.performed += i => scrollBackward = true;
            PlayerInput.Main.InvScrollBackward.canceled += i => scrollBackward = false;
            PlayerInput.Main.Pause.performed += i => pauseButton = true;
            PlayerInput.Main.Unpause.performed += i => unpauseButton = true;
        }
        PlayerInput.Enable();
    }
    private void OnDisable()
    {
        PlayerInput.Disable();
    }
    public void InputHandler()
    {
        MoveInputHandler();
    }
    private void MoveInputHandler()
    {
        vertInput = movementInput.y;
        horInput = movementInput.x;
        cameraInputY = cameraInput.y;
        cameraInputX = cameraInput.x;
    }
}
