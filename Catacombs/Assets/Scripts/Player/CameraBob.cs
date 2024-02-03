using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBob : MonoBehaviour
{
    [SerializeField, Range(0,0.1f)] private float amplitude = 0.015f;
    [SerializeField, Range(0, 30)] private float frequency = 10;

    PInputManager pInputManager;
    PlayerMotion playerMotion;
    [SerializeField] private Transform mainCamera = null;

    private float toggleSpeed = 3;
    private Vector3 startPos;

    private void Awake()
    {
        pInputManager = GetComponent<PInputManager>();
        playerMotion = GetComponent<PlayerMotion>();
        startPos = mainCamera.localPosition;
    }

    private void Update()
    {
        if (!NewEnemyInteract.grabbed)
        {
            CheckMotion();
            ResetPosition();
        }
    }

    private void CheckMotion()
    {
        float speed = new Vector3(playerMotion.moveDirection.z, 0, playerMotion.moveDirection.x).magnitude;
        if (speed < toggleSpeed) return;
        PlayMotion(BobMotion());
    }

    private Vector3 BobMotion()
    {
        Vector3 pos = Vector3.zero;
        if (playerMotion.isSprinting)
        {
            pos.y = Mathf.Sin(Time.time * frequency * 2) * amplitude*2;
            pos.x = Mathf.Cos(Time.time * frequency) * amplitude * 4;
        }
        else if (playerMotion.isCrouching)
        {
            pos.y = Mathf.Sin(Time.time * frequency/2) * amplitude/2;
            pos.x = Mathf.Cos(Time.time * frequency / 4) * amplitude;
        }
        else
        {
            pos.y = Mathf.Sin(Time.time * frequency) * amplitude;
            pos.x = Mathf.Cos(Time.time * frequency /2) * amplitude*2;
        }
        
        return pos;
    }

    private void PlayMotion(Vector3 motion)
    {
        mainCamera.localPosition += motion;
    }

    private void ResetPosition()
    {
        if (mainCamera.localPosition == startPos) return;
        mainCamera.localPosition = Vector3.Lerp(mainCamera.localPosition, startPos, 1 * Time.deltaTime);
    }
}
