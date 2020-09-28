using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public List<Collider> currentColliders { get; private set; } = new List<Collider>();

    void OnTriggerEnter(Collider other)
    {
        if (!currentColliders.Contains(other) && other.gameObject.tag == "Pickup" && other.gameObject.activeInHierarchy)
        {
            currentColliders.Add(other);
            //Tells the crate it has been picked up
            other.gameObject.GetComponent<IdleCrate>().PickUp(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (currentColliders.Contains(other))
        {
            currentColliders.Remove(other);
        }
    }
}
