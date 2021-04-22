using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundDetection : MonoBehaviour
{
    public float groundCheckDistance = 0f;
    public float groundCheckBounds = 0f;
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + new Vector3(groundCheckBounds, 0, groundCheckBounds), (transform.position + new Vector3(groundCheckBounds, 0, groundCheckBounds)) - transform.up * groundCheckDistance);
        Gizmos.DrawLine(transform.position + new Vector3(groundCheckBounds, 0, -groundCheckBounds), (transform.position + new Vector3(groundCheckBounds, 0, -groundCheckBounds)) - transform.up * groundCheckDistance);
        Gizmos.DrawLine(transform.position + new Vector3(-groundCheckBounds, 0, groundCheckBounds), (transform.position + new Vector3(-groundCheckBounds, 0, groundCheckBounds)) - transform.up * groundCheckDistance);
        Gizmos.DrawLine(transform.position + new Vector3(-groundCheckBounds, 0, -groundCheckBounds), (transform.position + new Vector3(-groundCheckBounds, 0, -groundCheckBounds)) - transform.up * groundCheckDistance);
    }

    public bool IsPlayerGrounded()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position + new Vector3(groundCheckBounds, 0, groundCheckBounds), -transform.up, out hitInfo, groundCheckDistance, -1, QueryTriggerInteraction.Ignore)
            || Physics.Raycast(transform.position + new Vector3(groundCheckBounds, 0, -groundCheckBounds), -transform.up, out hitInfo, groundCheckDistance, -1, QueryTriggerInteraction.Ignore)
            || Physics.Raycast(transform.position + new Vector3(-groundCheckBounds, 0, groundCheckBounds), -transform.up, out hitInfo, groundCheckDistance, -1, QueryTriggerInteraction.Ignore)
            || Physics.Raycast(transform.position + new Vector3(-groundCheckBounds, 0, -groundCheckBounds), -transform.up, out hitInfo, groundCheckDistance, -1, QueryTriggerInteraction.Ignore))
        {
            if (hitInfo.transform.parent != this.transform)
            {
                return true;
            }
        }

        return false;
    }
}
