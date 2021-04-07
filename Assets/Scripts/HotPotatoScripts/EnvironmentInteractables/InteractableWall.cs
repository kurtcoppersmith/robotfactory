using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableWall : MonoBehaviour
{
    public Collider colliderTrigger;
    public Collider wallCollider;
    public UnityEngine.AI.NavMeshObstacle navObst;
    public bool isNavigable = true;

    private bool hasPassedThrough = false;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, colliderTrigger.bounds.size);
    }

    public void SetNavigable(bool isNav)
    {
        if (isNav && PlayerManager.Instance.GetCurrentHolder() != null)
        {
            Physics.IgnoreCollision(wallCollider, PlayerManager.Instance.GetCurrentHolder().gameObject.GetComponent<Collider>(), true);
        }
        else if (!isNav && PlayerManager.Instance.GetCurrentHolder() != null)
        {
            Physics.IgnoreCollision(wallCollider, PlayerManager.Instance.GetCurrentHolder().gameObject.GetComponent<Collider>(), false);
        }

        isNavigable = isNav;
        navObst.enabled = !isNav;
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something has entered");
        Character otherCharacter = null;
        otherCharacter = other.gameObject.GetComponent<Character>();
        if (otherCharacter != null)
        {
            hasPassedThrough = true;
            Debug.Log("Character has entered");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Somethign has exited.");
        if (hasPassedThrough && isNavigable)
        {
            Debug.Log("Parameters met.");
            Character otherCharacter = null;
            otherCharacter = other.gameObject.GetComponent<Character>();
            if (otherCharacter != null)
            {
                Debug.Log("Character has exited");
                SetNavigable(false);
            }
        }
    }
}
