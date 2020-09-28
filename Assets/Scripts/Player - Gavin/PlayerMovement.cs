﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController charController { get; private set; }

    Vector2 movementInput;
    Vector3 movementVector;

    public bool tankControls = false;

    [Header("Tank Control Variables")]
    public float topForwardSpeed = 2;
    public float topReverseSpeed = 1;
    public float acceleration = 3;

    public float tankRotationSpeed = 4;
    public float stoppedTankRotationSpeed = 5;

    [Header("Normal Control Variables")]
    public float speed = 4;
    private float initialSpeed;
    public float rotationSpeed = 4;

    [Header("Icey Movement Variables")]
    Vector3 finalDir = Vector3.zero;
    public float iceSpeed = 5.5f;
    private float friction = 1.0f;
    private bool isIced = false;

    [Header("General Hazard Effect Variables")]
    public float hazardEffectDuration = 2.0f;
    private float maxHazardEffectDuration;

    private bool isSlowed = false;

    private float currentSpeed;
    private float currentTopSpeed;
    private bool isAccel = false;

    [Header("Additional Variables")]
    public float gravity = 10;
    [Range(0.01f, 1f)]
    public float minimumGravity = 0.01f;

    [HideInInspector]
    public bool canMove = true;

    void Awake()
    {
        charController = GetComponent<CharacterController>();

        HelperUtilities.UpdateCursorLock(true);

        movementVector = Vector3.zero;
        currentTopSpeed = topForwardSpeed;
        charController.detectCollisions = true;

        initialSpeed = speed;
        maxHazardEffectDuration = hazardEffectDuration;
    }

    void Start()
    {
        tankControls = GameManager.Instance.tempTankBool;
    }

    void OnMovement(InputValue inputValue)
    {
        movementInput = inputValue.Get<Vector2>();
    }

    public void SetPlayerIced(bool option)
    {
        isIced = option;

        if (option)
        {
            speed = iceSpeed;
            hazardEffectDuration = maxHazardEffectDuration;

            Vector2 tempRelativeValues = FindMovementRelativeToCamera(movementInput.x, movementInput.y);
            Debug.Log(tempRelativeValues);
            Vector3 tempFinalDir = new Vector3(tempRelativeValues.x, 0, tempRelativeValues.y) * speed;
            finalDir = tempFinalDir;
        }
    }

    public void IcePlayer()
    {
        hazardEffectDuration -= Time.deltaTime;

        if (hazardEffectDuration <= 0)
        {
            isIced = false;
            friction = 1.0f;
            speed = initialSpeed;
        }
    }

    void TankMove(float h, float v)
    {
        float currentTurnRate = Mathf.Lerp(tankRotationSpeed, stoppedTankRotationSpeed, 1 - (currentSpeed / currentTopSpeed));
        Vector3 angles = transform.eulerAngles;
        angles.y += (h * currentTurnRate);
        transform.eulerAngles = angles;

        if (v > 0)
        {
            movementVector = transform.forward * v;
            currentTopSpeed = topForwardSpeed;
            isAccel = true;
        }
        else if (v < 0)
        {
            movementVector = transform.forward * v;
            currentTopSpeed = topReverseSpeed;
            isAccel = true;
        }
        else
        {
            movementVector = Vector3.zero;
            isAccel = false;
        }

        if (isAccel)
        {
            if (currentSpeed < currentTopSpeed)
            {
                //currentSpeed = currentSpeed + (acceleration * Time.deltaTime);
                currentSpeed += acceleration;
            }
        }
        else
        {
            if (currentSpeed > 0)
            {
                //currentSpeed = currentSpeed - (acceleration * Time.deltaTime);
                currentSpeed -= acceleration;
            }
        }

        if (!(currentSpeed > 0) && movementVector == Vector3.zero)
        {
            currentSpeed = 0;
            isAccel = false;
        }
        else
        {
            if (currentSpeed > currentTopSpeed)
            {
                currentSpeed = currentTopSpeed;
            }
        }
        
        movementVector.x *= currentSpeed * Time.deltaTime;
        movementVector.z *= currentSpeed * Time.deltaTime;
    }

    Vector2 FindMovementRelativeToCamera(float h, float v)
    {
        Vector3 VertVector = LevelManager.Instance.mainCam.transform.forward;
        VertVector.x = Mathf.Abs(VertVector.x);
        VertVector.z = Mathf.Abs(VertVector.z);

        if ((int)VertVector.x != 0 || !(VertVector.x < 0.1 && VertVector.x > -0.1))
            VertVector.x /= VertVector.x;

        if ((int)VertVector.z != 0 || !(VertVector.z < 0.1 && VertVector.z > -0.1))
            VertVector.z /= VertVector.z;

        VertVector *= v;

        Vector3 HorVector = LevelManager.Instance.mainCam.transform.right;
        HorVector.x = Mathf.Abs(HorVector.x);
        HorVector.z = Mathf.Abs(HorVector.z);

        if ((int)HorVector.x != 0 || !(HorVector.x < 0.1 && HorVector.x > -0.1))
            HorVector.x /= HorVector.x;

        if ((int)HorVector.z != 0 || !(HorVector.z < 0.1 && HorVector.x > -0.1))
            HorVector.z /= HorVector.z;

        HorVector *= -h;

        Vector3 tempVec = VertVector + HorVector;

        return new Vector2(tempVec.x, tempVec.z);
    }

    void NormalMove(float h, float v)
    {
        Vector2 relativeMoveValues = FindMovementRelativeToCamera(h, v);
        
        
        Vector3 moveDirection = new Vector3(relativeMoveValues.x, 0, relativeMoveValues.y);
        
        if(isIced)
        {
            Vector3 directionVelocity = moveDirection * speed;
            finalDir = Vector3.Lerp(finalDir, directionVelocity, friction * Time.deltaTime);
            movementVector = finalDir;

            movementVector.x *= Time.deltaTime;
            movementVector.z *= Time.deltaTime;
        }
        else
        {
            movementVector = moveDirection;

            movementVector.x *= speed * Time.deltaTime;
            movementVector.z *= speed * Time.deltaTime;
        }
        
        if (movementVector != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(transform.forward + moveDirection, transform.up), rotationSpeed * Time.deltaTime);
        }
    }

    void MovePlayer()
    {
        float h = movementInput.x;
        float v = movementInput.y;

        if (tankControls)
        {
            TankMove(h, v);
        }
        else
        {
            NormalMove(h, v);
        }

        if (!charController.isGrounded)
        {
            movementVector.y -= gravity * Time.deltaTime;
        }
        else
        {
            movementVector.y -= minimumGravity * Time.deltaTime;
        }

        charController.Move(movementVector);
    }

    void Update()
    {
        if (isIced)
        {
            IcePlayer();
        }
    }

    void FixedUpdate()
    {
        if (canMove && !GameManager.Instance.hasEnded)
        {
            MovePlayer();
        }
    }
}
