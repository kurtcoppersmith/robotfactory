using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController charController { get; private set; }
    private PlayerModel playerModel;
    private PlayerDash playerDash;

    Vector2 movementInput;
    Vector3 movementVector;

    [Header("Normal Control Variables")]
    public float speed = 4;
    public float initialSpeed { get; private set; }
    public float rotationSpeed = 4;

    [Header("Icey Movement Variables")]
    Vector3 finalDir = Vector3.zero;
    public float iceSpeed = 5.5f;
    private float friction = 1.0f;
    private bool isIced = false;

    public float groundCheckDistance = 0f;
    public float groundCheckBounds = 0f;
    private bool isPermaIced = false;

    [Header("Slowed Movement Variables")]
    public float slowedSpeed = 4;
    private bool isSlowed = false;

    [Header("General Hazard Effect Variables")]
    public float hazardEffectDuration = 2.0f;
    private float maxHazardEffectDuration;

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
        playerModel = GetComponent<PlayerModel>();
        playerDash = GetComponent<PlayerDash>();

        HelperUtilities.UpdateCursorLock(true);

        movementVector = Vector3.zero;
        charController.detectCollisions = true;

        initialSpeed = speed;
        maxHazardEffectDuration = hazardEffectDuration;
    }

    void OnMovement(InputValue inputValue)
    {
        movementInput = inputValue.Get<Vector2>();
    }

    public void SetPlayerIced(bool option)
    {
        if(playerModel.playerPowerups.chasisPower)
        {
            return;
        }
        else 
        {
            isIced = option;

            if (option)
            {
                if (speed == initialSpeed)
                {
                    speed = iceSpeed;
                    Vector2 tempRelativeValues = FindMovementRelativeToCamera(movementInput.x, movementInput.y);
                    Vector3 tempFinalDir = new Vector3(tempRelativeValues.x, 0, tempRelativeValues.y) * speed;
                    finalDir = tempFinalDir;
                }
                
                hazardEffectDuration = maxHazardEffectDuration;
            }
            else
            {
                isIced = false;
                friction = 1.0f;
                speed = initialSpeed;
            }
        }
    }

    void IcePlayer()
    {
        hazardEffectDuration -= Time.deltaTime;

        if (hazardEffectDuration <= 0)
        {
            if (!CheckForIce())
            {
                isIced = false;
                friction = 1.0f;
                if (playerModel.playerPowerups.speedPower)
                {
                    speed = playerModel.playerPowerups.powerupSpeed;
                }
                else
                {
                    speed = initialSpeed;
                }
            }        
        }
    }

    public void SetPlayerSlowed(bool option)
    {
        if (playerModel.playerPowerups.chasisPower)
        {
            return;
        }
        else
        {
            isSlowed = option;

            if (option)
            {
                speed = slowedSpeed;
                hazardEffectDuration = maxHazardEffectDuration + playerModel.maxPlayerStunnedTime;
            }
        }
    }

    void SlowPlayer()
    {
        hazardEffectDuration -= Time.deltaTime;

        if (hazardEffectDuration <= 0)
        {
            isSlowed = false;
            speed = initialSpeed;
        }
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
        
        if(isIced || isPermaIced)
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
        float h, v;

        if (TutorialManager.Instance != null && TutorialManager.Instance.currentObjective == 0)
        {
            if (movementInput.x != 0 || movementInput.y != 0)
            {
                h = 0;
                v = 1;
            }
            else
            {
                h = 0; 
                v = 0;
            }
        }
        else
        {
            h = movementInput.x;
            v = movementInput.y;
        }

        if (!playerDash.isDashing)
        {
            NormalMove(h, v);
            
            if (!charController.isGrounded)
            {
                movementVector.y -= gravity * Time.deltaTime;
            }
            else
            {
                movementVector.y -= minimumGravity * Time.deltaTime;
            }
        }
        else
        {
            movementVector = Vector3.zero;
        }

        charController.Move(movementVector);
    }

    bool CheckForIce()
    {
        if (playerModel.playerPowerups.chasisPower)
        {
            return false;
        }

        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position + new Vector3(groundCheckBounds, 0, groundCheckBounds), -transform.up, out hitInfo, groundCheckDistance))
        {
            if (hitInfo.transform.parent != this.transform && hitInfo.transform.gameObject.tag == "IceTrap")
            {
                isPermaIced = true;

                if (speed == initialSpeed)
                {
                    speed = iceSpeed;
                    Vector2 tempRelativeValues = FindMovementRelativeToCamera(movementInput.x, movementInput.y);
                    Vector3 tempFinalDir = new Vector3(tempRelativeValues.x, 0, tempRelativeValues.y) * speed;
                    finalDir = tempFinalDir;
                }
                return true;
            }
        }
        
        if(Physics.Raycast(transform.position + new Vector3(groundCheckBounds, 0, -groundCheckBounds), -transform.up, out hitInfo, groundCheckDistance))
        {
            if (hitInfo.transform.parent != this.transform && hitInfo.transform.gameObject.tag == "IceTrap")
            {
                isPermaIced = true;

                if (speed == initialSpeed)
                {
                    speed = iceSpeed;
                    Vector2 tempRelativeValues = FindMovementRelativeToCamera(movementInput.x, movementInput.y);
                    Vector3 tempFinalDir = new Vector3(tempRelativeValues.x, 0, tempRelativeValues.y) * speed;
                    finalDir = tempFinalDir;
                }
                return true;
            }
        }
        
        if(Physics.Raycast(transform.position + new Vector3(-groundCheckBounds, 0, groundCheckBounds), -transform.up, out hitInfo, groundCheckDistance))
        {
            if (hitInfo.transform.parent != this.transform && hitInfo.transform.gameObject.tag == "IceTrap")
            {
                isPermaIced = true;

                if (speed == initialSpeed)
                {
                    speed = iceSpeed;
                    Vector2 tempRelativeValues = FindMovementRelativeToCamera(movementInput.x, movementInput.y);
                    Vector3 tempFinalDir = new Vector3(tempRelativeValues.x, 0, tempRelativeValues.y) * speed;
                    finalDir = tempFinalDir;
                }
                return true;
            }
        }
        
        if(Physics.Raycast(transform.position + new Vector3(-groundCheckBounds, 0, -groundCheckBounds), -transform.up, out hitInfo, groundCheckDistance))
        {
            if (hitInfo.transform.parent != this.transform && hitInfo.transform.gameObject.tag == "IceTrap")
            {
                isPermaIced = true;

                if (speed == initialSpeed)
                {
                    speed = iceSpeed;
                    Vector2 tempRelativeValues = FindMovementRelativeToCamera(movementInput.x, movementInput.y);
                    Vector3 tempFinalDir = new Vector3(tempRelativeValues.x, 0, tempRelativeValues.y) * speed;
                    finalDir = tempFinalDir;
                }
                return true;
            }
        }

        if (Physics.Raycast(transform.position + new Vector3(-groundCheckBounds, 0, -groundCheckBounds), -transform.up, out hitInfo, groundCheckDistance))
        {
            if (hitInfo.transform.parent != this.transform && hitInfo.transform.gameObject.tag != "IceTrap" && isPermaIced )
            {
                isPermaIced = false;

                SetPlayerIced(true);

                return false;
            }
        }

        return false;
    }

    void Update()
    {
        CheckForIce();

        if (isIced)
        {
            IcePlayer();
        }

        if (isSlowed)
        {
            SlowPlayer();
        }
    }

    void FixedUpdate()
    {
        if (canMove && !GameManager.Instance.hasEnded)
        {
            if ((TutorialManager.Instance != null && playerModel.isHolding && TutorialManager.Instance.currentObjective == 0) || (TutorialManager.Instance != null && TutorialManager.Instance.currentObjective > 0 && TutorialManager.Instance.hasDescription) || (TutorialManager.Instance == null))
            {
                MovePlayer();
            }
        }
    }
}
