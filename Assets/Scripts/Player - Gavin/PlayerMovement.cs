using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public float rotationSpeed = 4;

    public float gravity = 10;

    private float currentSpeed;
    private float currentTopSpeed;
    private bool isAccel = false;

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
    }

    void Start()
    {
        tankControls = GameManager.Instance.tempTankBool;
    }

    void OnMovement(InputValue inputValue)
    {
        movementInput = inputValue.Get<Vector2>();
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

    void NormalMove(float h, float v)
    {
        movementVector = new Vector3(h, 0, v);
        if (movementVector != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(transform.forward + movementVector, transform.up), rotationSpeed * Time.deltaTime);
        }

        movementVector.x *= speed * Time.deltaTime;
        movementVector.z *= speed * Time.deltaTime;
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

    void FixedUpdate()
    {
        if (canMove && !GameManager.Instance.hasEnded)
        {
            MovePlayer();
        }
    }
}
