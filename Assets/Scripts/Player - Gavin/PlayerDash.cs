using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerDash : MonoBehaviour
{
    public bool isDashing = false;
    public float dashingDistance = 0f;
    public float dashingSpeed = 0f;

    private float maxDashTime = 0f;
    private float dashTime = 0f;

    public float groundCheckDistance = 0f;
    public float groundCheckBounds = 0f;

    PlayerModel playerMod;

    void Awake()
    {
        maxDashTime = dashingDistance / dashingSpeed;
        dashTime = maxDashTime;

        playerMod = GetComponent<PlayerModel>();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * dashingDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + new Vector3(groundCheckBounds, 0, groundCheckBounds), (transform.position + new Vector3(groundCheckBounds, 0, groundCheckBounds)) - transform.up * groundCheckDistance);
        Gizmos.DrawLine(transform.position + new Vector3(groundCheckBounds, 0, -groundCheckBounds), (transform.position + new Vector3(groundCheckBounds, 0, -groundCheckBounds)) - transform.up * groundCheckDistance);
        Gizmos.DrawLine(transform.position + new Vector3(-groundCheckBounds, 0, groundCheckBounds), (transform.position + new Vector3(-groundCheckBounds, 0, groundCheckBounds)) - transform.up * groundCheckDistance);
        Gizmos.DrawLine(transform.position + new Vector3(-groundCheckBounds, 0, -groundCheckBounds), (transform.position + new Vector3(-groundCheckBounds, 0, -groundCheckBounds)) - transform.up * groundCheckDistance);
    }

    void OnScoot(InputValue inputValue)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position + new Vector3(groundCheckBounds, 0, groundCheckBounds), -transform.up, out hitInfo, groundCheckDistance)
            || Physics.Raycast(transform.position + new Vector3(groundCheckBounds, 0, -groundCheckBounds), -transform.up, out hitInfo, groundCheckDistance)
            || Physics.Raycast(transform.position + new Vector3(-groundCheckBounds, 0, groundCheckBounds), -transform.up, out hitInfo, groundCheckDistance)
            || Physics.Raycast(transform.position + new Vector3(-groundCheckBounds, 0, -groundCheckBounds), -transform.up, out hitInfo, groundCheckDistance))
        {
            if(hitInfo.transform.parent != this.transform)
            {
                if (!isDashing && playerMod.playerState != PlayerModel.PlayerState.Stunned && playerMod.playerMovement.canMove && !GameManager.Instance.hasEnded && !GameManager.Instance.isPaused)
                {
                    if ((TutorialManager.Instance != null && TutorialManager.Instance.hasDescription && TutorialManager.Instance.currentObjective >= 2) || (TutorialManager.Instance == null))
                    {
                        isDashing = true;
                    }
                }
            }
        }
    }

    void Update()
    {
        if (isDashing)
        {
            if(dashTime >= 0)
            {
                dashTime -= Time.deltaTime;
                playerMod.playerMovement.charController.Move(transform.forward.normalized * dashingSpeed * Time.deltaTime);
            }
            else
            {
                isDashing = false;
                dashTime = maxDashTime;
            }
        }
    }
}
