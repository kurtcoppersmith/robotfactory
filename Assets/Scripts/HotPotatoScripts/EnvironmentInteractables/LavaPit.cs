using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaPit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Character currentChar = null;
        currentChar = other.GetComponent<Character>();
        if (currentChar != null)
        {
            if (currentChar.currentPickup != null)
            {
                currentChar.currentPickup.GetComponent<Pickup>().Dropped();
                currentChar.Drop();
            }
        }
    }
}
