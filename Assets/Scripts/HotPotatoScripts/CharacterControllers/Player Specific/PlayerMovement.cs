using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerController playerController;
    private CharacterController characterController;

    private Vector2 movementInput = Vector2.zero;
    public float speed = 0;
    public float gravity = 0;
    public float turnSpeed = 0;
    private Vector3 currentVelocity = Vector3.zero, previousVelocity = Vector3.zero;
    [ReadOnly] public bool canMove = true;

    public float normalSpeed = 0f;


    private void Awake()
    {
        normalSpeed = speed;

        characterController = GetComponent<CharacterController>();
        playerController = GetComponent<PlayerController>();
    }

    public void SetCharacterMovementVariables()
    {
        playerController.maxDashAbilityRecharge = playerController.dashAbilityRecharge;

        playerController.maxDashTime = playerController.dashingDistance / playerController.dashingSpeed;
        playerController.dashTime = playerController.maxDashTime;

        playerController.maxKnockbackTime = playerController.knockbackDistance / playerController.knockbackSpeed;
        playerController.knockbackTime = playerController.maxKnockbackTime;

        playerController.maxDazedTime = playerController.dazedTime;
    }

    void OnDrawGizmosSelected()
    {
        //Gizmos.color = Color.magenta;
        //Gizmos.DrawLine(transform.position, transform.position + transform.forward * playerController.dashingDistance);

        //Gizmos.color = Color.red;
        //Gizmos.DrawLine(transform.position, transform.position + transform.right * playerController.knockbackDistance);
    }

    void OnMovement(InputValue inputValue)
    {
        movementInput = inputValue.Get<Vector2>();
    }

    void OnDash(InputValue inputValue)
    {
        if (!playerController.isEnabled || playerController.currentPickup != null)
        {
            return;
        }

        if (playerController.groundDetection.IsPlayerGrounded())
        {
            if (canMove && playerController.canDash)
            {
                playerController.isDashing = true;
                canMove = false;

                playerController.canDash = false;
                playerController.playerUI.dashAbilityImage.gameObject.SetActive(true);
                playerController.playerUI.dashAbilityImage.fillAmount = 0;
            }
        }
    }

    public void Move()
    {
        if (playerController.isStunned)
        {
            return;
        }

        if (canMove)
        {
            currentVelocity = new Vector3(movementInput.x, 0, movementInput.y);
            currentVelocity.Normalize();
            currentVelocity *= speed;

            currentVelocity.x *= Mathf.Sign(playerController.playerCam.transform.forward.x);
            currentVelocity.z *= Mathf.Sign(playerController.playerCam.transform.forward.z);

            if (!playerController.groundDetection.IsPlayerGrounded())
            {
                currentVelocity.y = -gravity;
            }

            if (movementInput != Vector2.zero)
            {
                previousVelocity = currentVelocity;

                if (!playerController.characterAnim.GetBool("WalkBool"))
                {
                    playerController.characterAnim.SetBool("WalkBool", true);
                }
            }
            else
            {
                if (playerController.characterAnim.GetBool("WalkBool"))
                {
                    playerController.characterAnim.SetBool("WalkBool", false);
                }
            }

            Vector3 tempVelocity = previousVelocity;
            tempVelocity.y = 0;

            playerController.avatar.transform.rotation =
                Quaternion.Slerp(playerController.avatar.transform.rotation, Quaternion.LookRotation(tempVelocity, Vector3.up), turnSpeed * Time.deltaTime);

            playerController.currentVelocity = currentVelocity;

            characterController.Move(currentVelocity * Time.deltaTime);
        }
    }

    public void Knockback(Vector3 attackerPosition)
    {
        canMove = false;

        playerController.isKnockbacked = true;
        playerController.knockbackTime = playerController.maxKnockbackTime;

        Vector3 tempAttackerPos = attackerPosition, tempCharacterPos = transform.position;
        tempAttackerPos.y = 0;
        tempCharacterPos.y = 0;

        Vector3 finalDir = tempCharacterPos - tempAttackerPos;
        finalDir.Normalize();
        playerController.knockbackDir = finalDir;

        playerController.isDazed = true;
        playerController.dazedTime = playerController.maxDazedTime;
        speed = playerController.dazedSpeed;
    }

    void UpdateDash()
    {
        if (!playerController.canDash)
        {
            playerController.dashAbilityRecharge -= Time.deltaTime;

            if (playerController.currentPickup == null)
            {
                playerController.playerUI.dashAbilityImage.fillAmount =
                                    HelperUtilities.Remap(playerController.maxDashAbilityRecharge - playerController.dashAbilityRecharge, 0, playerController.maxDashAbilityRecharge, 0, 1);
            }

            if (playerController.dashAbilityRecharge <= 0)
            {
                playerController.playerUI.dashAbilityImage.fillAmount = 1;

                playerController.canDash = true;
                playerController.dashAbilityRecharge = playerController.maxDashAbilityRecharge;
                if (playerController.currentPickup == null)
                {
                    playerController.playerUI.dashAbilityImage.gameObject.SetActive(false);
                }
            }
        }

        if (playerController.isDashing)
        {
            if (playerController.dashTime >= 0)
            {
                playerController.dashTime -= Time.deltaTime;
                if (!playerController.isKnockbacked)
                {
                    if (movementInput == Vector2.zero)
                    {
                        playerController.currentVelocity = playerController.avatar.transform.forward.normalized * playerController.dashingSpeed;

                        characterController.Move(playerController.avatar.transform.forward.normalized * playerController.dashingSpeed * Time.deltaTime);
                    }
                    else
                    {
                        playerController.currentVelocity = currentVelocity.normalized * playerController.dashingSpeed;

                        characterController.Move(currentVelocity.normalized * playerController.dashingSpeed * Time.deltaTime);

                        Vector3 tempVelocity = previousVelocity;
                        tempVelocity.y = 0;
                        playerController.avatar.transform.rotation = Quaternion.LookRotation(tempVelocity, Vector3.up);
                    }
                }
            }
            else
            {
                playerController.isDashing = false;
                canMove = true;
                playerController.dashTime = playerController.maxDashTime;
            }
        }
    }

    void UpdateKnockback()
    {
        if (playerController.isKnockbacked)
        {
            playerController.knockbackTime -= Time.deltaTime;
            if (playerController.knockbackTime >= 0)
            {
                characterController.Move(playerController.knockbackDir * playerController.knockbackSpeed * Time.deltaTime);
            }
            else
            {
                canMove = true;
                playerController.isKnockbacked = false;
            }
        }
    }

    void UpdateDaze()
    {
        if (playerController.isDazed)
        {
            playerController.dazedTime -= Time.deltaTime;
            if (playerController.dazedTime <= 0)
            {
                playerController.isDazed = false;
                speed = normalSpeed;        
            }
        }
    }

    void UpdateStun()
    {
        if (playerController.isStunned)
        {
            playerController.stunnedTime -= Time.deltaTime;
            if(playerController.stunnedTime <= 0)
            {
                playerController.isStunned = false;
                playerController.stunnedTime = playerController.maxStunnedTime;
            }
        }
    }

    private void Update()
    {
        if (playerController.isEnabled)
        {
            UpdateStun();
            if (playerController.isStunned)
            {
                return;
            }

            UpdateDash();
            UpdateKnockback();
            UpdateDaze();
        }
    }
}
