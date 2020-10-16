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

    PlayerMovement playerMovement;

    void Awake()
    {
        maxDashTime = dashingDistance / dashingSpeed;
        dashTime = maxDashTime;

        playerMovement = GetComponent<PlayerMovement>();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * dashingDistance);
    }

    void OnScoot(InputValue inputValue)
    {
        if (!isDashing)
        {
            isDashing = true;
        }
    }

    void Update()
    {
        if (isDashing)
        {
            if(dashTime >= 0)
            {
                dashTime -= Time.deltaTime;
                playerMovement.charController.Move(transform.forward.normalized * dashingSpeed * Time.deltaTime);
            }
            else
            {
                isDashing = false;
                dashTime = maxDashTime;
            }
        }
    }
}
