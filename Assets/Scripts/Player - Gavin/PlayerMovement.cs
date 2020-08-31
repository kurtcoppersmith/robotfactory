using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    CharacterController charController;

    Vector2 movementInput;
    Vector3 movementVector;

    public float speed = 5;
    public float rotationSpeed = 4;
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
    }

    void OnMovement(InputValue inputValue)
    {
        movementInput = inputValue.Get<Vector2>();
    }

    void MovePlayer()
    {
        float h = movementInput.x;
        float v = movementInput.y;
        movementVector = new Vector3(h, 0, v);
        if (movementVector != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(transform.forward + movementVector, transform.up), rotationSpeed * Time.deltaTime);
        }

        if (!charController.isGrounded)
        {
            movementVector.y -= gravity * Time.deltaTime;
        }
        else
        {
            movementVector.y -= minimumGravity * Time.deltaTime;
        }

        charController.Move(movementVector * speed * Time.deltaTime);
    }

    void Update()
    {
        if (canMove)
        {
            MovePlayer();
        }
    }
}
