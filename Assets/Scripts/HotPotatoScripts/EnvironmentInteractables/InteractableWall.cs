using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableWall : MonoBehaviour
{
    public Collider wallCollider;
    public bool isNavigable = true;

    public void SetNavigable(bool isNav)
    {
        if (isNav && PlayerManager.Instance.GetCurrentHolder() != null)
        {
            Physics.IgnoreCollision(wallCollider, PlayerManager.Instance.GetCurrentHolder().gameObject.GetComponent<Collider>(), true);
        }

        isNavigable = isNav;
    }
}
