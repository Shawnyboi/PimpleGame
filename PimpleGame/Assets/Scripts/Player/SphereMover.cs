﻿using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(SphereSticker))]
public class SphereMover : MonoBehaviour
{
    Rigidbody body = null;
    SphereSticker sticker = null;
    [SerializeField] float acceleration = 1f;
    [SerializeField] float topSpeed = 10f;

    Vector2 cachedInputMovement = Vector2.up;

    [SerializeField] Transform desiredLook = null;
    [SerializeField] Transform actualLook = null;
    [SerializeField] Transform inputOrientation = null;

    [SerializeField] float dampening = 0.8f;

    Vector3? forcedDirection = null;

    bool allowInput = true;
    bool inputPaused = false;

    float topSpeedModifier = 1;

    bool wasMoving = false;

    [SerializeField] UnityEvent onMoveStart = null;
    [SerializeField] UnityEvent onMoveEnd = null;

    private void OnDestroy()
    {
        GamePauser.OnGamePause -= PausePlayerMovement;
        GamePauser.OnGameUnpause -= UnPausePlayerMovement;
    }

    private void Awake()
    {
        GamePauser.OnGamePause += PausePlayerMovement;
        GamePauser.OnGameUnpause += UnPausePlayerMovement;
        body = GetComponent<Rigidbody>();
        sticker = GetComponent<SphereSticker>();
        cachedInputMovement = new Vector2(desiredLook.forward.x, desiredLook.forward.z);
    }

    private void FixedUpdate()
    {
        Vector2 inputMovement = Vector2.zero;
        if (allowInput && !inputPaused)
        {
            inputMovement.x = Input.GetAxis("Horizontal");
            inputMovement.y = Input.GetAxis("Vertical");
        }

        if (forcedDirection != null)
        {
            var forcedOnNormal = Vector3.Project(forcedDirection.Value, actualLook.up);
            var forcedOnPlane = forcedDirection.Value - forcedOnNormal;

            var newRight = Vector3.Cross(actualLook.up, forcedOnPlane.normalized);
            actualLook.LookAt(actualLook.transform.position + Vector3.Cross(newRight, actualLook.up), actualLook.up);
            body.velocity = forcedOnPlane * topSpeed * topSpeedModifier;
        }

        if (inputMovement.sqrMagnitude > Helper.Epsilon)
        {
            if (!wasMoving)
            {
                wasMoving = true;
                onMoveStart.Invoke();
            }

            inputMovement.Normalize();

            Vector3 force = (inputOrientation.right * inputMovement.x) + (inputOrientation.forward * inputMovement.y);
            force = force.normalized * acceleration;
            var targetForward = (force - Vector3.Project(force, actualLook.up)).normalized;


            var cos = Mathf.Clamp(Vector3.Dot(actualLook.forward, targetForward), -1, 1);

            var angle = Mathf.Acos(cos) * Mathf.Rad2Deg;
            if (Vector3.Dot(actualLook.right, force) < 0)
            {
                angle *= -1;
            }
            actualLook.Rotate(new Vector3(0, angle, 0), Space.Self);

            var velocity = body.velocity + (force * Time.deltaTime);
            if (velocity.sqrMagnitude > topSpeed)
            {
                velocity = velocity.normalized * topSpeed;
            }

            // TODO when hit disable input, blink with i-frames, give some knockback
            body.velocity = velocity;

        }
        else
        {
            if (wasMoving)
            {
                wasMoving = false;
                onMoveEnd.Invoke();
            }
            body.velocity *= dampening;
        }

        sticker.StickToSphere();
    }

    public void AllowInput(bool allow)
    {
        allowInput = allow;
    }

    public void ForceMovement(Vector3 forcedDirection)
    {
        allowInput = false;
        topSpeedModifier = forcedDirection.magnitude;
        this.forcedDirection = forcedDirection / topSpeedModifier;
    }

    public void FreeMovement()
    {
        allowInput = true;
        topSpeedModifier = 1;
        forcedDirection = null;
    }

    public void Stop()
    {
        allowInput = false;
        topSpeedModifier = 0;
        body.velocity = Vector3.zero;
    }

    private void PausePlayerMovement()
    {
        inputPaused = true;
    }

    private void UnPausePlayerMovement()
    {
        inputPaused = false;
    }

}
