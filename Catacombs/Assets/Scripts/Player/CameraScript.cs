using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CameraScript : MonoBehaviour
{
    PInputManager PInputManager;
    private float pivotSmoothVelocity;
    [Header("Camera Sensitivity")]
    [SerializeField] private float cameraLookSpeed = 0.2f;
    [SerializeField] private float cameraPivotSpeed = 0.2f;
    [Header("")]
    [SerializeField] private float lookDampSpeed;
    [SerializeField] private float upMin;
    [SerializeField] private float downMin;
    [SerializeField] private float upMax;
    [SerializeField] private float downMax;
    [SerializeField] private float lookAngle;
    [SerializeField] private float pivotAngle;
    [SerializeField] private Transform player;
    [Header("Grabbed")]
    public bool grabbed = false;

    private void Awake()
    {
        PInputManager = GetComponentInParent<PInputManager>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void CameraFunction()
    {
        if (!grabbed)
        {
            CameraMovement();
        }
    }
    private void CameraMovement()
    {
        Vector3 rotation;
        Quaternion targetRotation;

        lookAngle = lookAngle + (PInputManager.cameraInputX * cameraLookSpeed* SettingsMenu.sensMult);
        pivotAngle = pivotAngle - (PInputManager.cameraInputY * cameraPivotSpeed* SettingsMenu.sensMult);
        pivotAngle = Mathf.Clamp(pivotAngle, downMax, upMax);
        if (pivotAngle > upMin)
        {
            pivotAngle = Mathf.SmoothDamp(pivotAngle, upMin, ref pivotSmoothVelocity, lookDampSpeed);
        }
        if (pivotAngle < downMin)
        {
            pivotAngle = Mathf.SmoothDamp(pivotAngle, downMin, ref pivotSmoothVelocity, lookDampSpeed);
        }
        rotation = Vector3.zero;
        rotation.y = lookAngle;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        transform.localRotation = targetRotation;
    }
}
