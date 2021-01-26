using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class CharacterMovement : MonoBehaviour
{
    public CharacterController charController { get; private set; }
    private Character character;
    //private PlayerDash playerDash;

    Vector2 movementInput;
    Vector3 movementVector;

    [Header("Normal Control Variables")]
    public float speed = 4;
    public float initialSpeed { get; private set; }
    public float rotationSpeed = 4;

    //[Header("Icey Movement Variables")]
    Vector3 finalDir = Vector3.zero;
    //public float iceSpeed = 5.5f;
    //public float stopIcyTime = 1f;
    private float friction = 1.0f;
    //private bool isIced = false;

    //public float groundCheckDistance = 0f;
    //public float groundCheckBounds = 0f;
    //private bool isPermaIced = false;

    //[Header("Slowed Movement Variables")]
    //public float slowedSpeed = 4;
    //private bool isSlowed = false;

    //[Header("General Hazard Effect Variables")]
    //public float hazardEffectDuration = 2.0f;
    //private float maxHazardEffectDuration;

    //private float currentSpeed;
    //private float currentTopSpeed;
    //private bool isAccel = false;

    [Header("Additional Variables")]
    public float gravity = 10;
    [Range(0.01f, 1f)]
    public float minimumGravity = 0.01f;

    [HideInInspector]
    public bool canMove = true;
    private float movementLerper = 0;

    void Awake()
    {
        charController = GetComponent<CharacterController>();
        character = GetComponent<Character>();
        //playerDash = GetComponent<PlayerDash>();

        movementVector = Vector3.zero;
        character.currentVelocity = movementVector;
        //charController.detectCollisions = true;

        initialSpeed = speed;
        //maxHazardEffectDuration = hazardEffectDuration;
    }

    void OnMovement(InputValue inputValue)
    {
        movementInput = inputValue.Get<Vector2>();
    }

    void OnBoxPickup(InputValue inputValue)
    {
        character.BoxPickUp();
    }

    void ShowPickUpIndicator()
    {
        if (character.isHolding || GameManager.Instance.hasEnded)
        {
            character.pickupIndicator.SetActive(false);
            return;
        }
        bool pickup = false;

        for (int i = 0; i < character.playerPickup.currentColliders.Count; i++)
        {
            if (character.playerPickup.currentColliders[i].gameObject.tag == "Pickup")
            {   
                if (!character.playerPickup.currentColliders[i].gameObject.GetComponent<ObjectHP>().canBeHeld)
                {
                    return;
                }

                pickup = true;
                character.pickupIndicator.SetActive(true);
                break;
            }
        }

        if (!pickup)
        {
            character.pickupIndicator.SetActive(false);
        }
    }

    void Update()
    {
        ShowPickUpIndicator();
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

        //if (isIced || isPermaIced)
        //{
            //Vector3 directionVelocity = moveDirection * speed;
           // finalDir = Vector3.Lerp(finalDir, directionVelocity, friction * Time.deltaTime);
           // movementVector = finalDir;

            //movementVector.x *= Time.deltaTime;
            //movementVector.z *= Time.deltaTime;
       // }
       // else
        //{
            Vector3 directionVelocity = moveDirection * speed;
            finalDir = Vector3.Lerp(finalDir, directionVelocity, friction * Time.deltaTime);

            movementVector = Vector3.Lerp(moveDirection * speed, finalDir, movementLerper);

            movementVector.x *= Time.deltaTime;
            movementVector.z *= Time.deltaTime;
        //}

        if (movementVector != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(transform.forward + moveDirection, transform.up), rotationSpeed * Time.deltaTime);
        }
    }

    void MovePlayer()
    {
        float h, v;

        h = movementInput.x;
        v = movementInput.y;

        NormalMove(h, v);

        if (!charController.isGrounded)
        {
            movementVector.y -= gravity * Time.deltaTime;
        }
        else
        {
            movementVector.y -= minimumGravity * Time.deltaTime;
        }

        charController.Move(movementVector);
        character.currentVelocity = new Vector3(movementVector.x, 0, movementVector.z);
    }

    void FixedUpdate()
    {
        if (canMove && !GameManager.Instance.hasEnded)
        {
            MovePlayer();
        }
    }
}
