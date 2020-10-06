using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleCrate : MonoBehaviour
{
    bool pickedUp = false;

    public void PickUp(bool status)
    {
        pickedUp = status;
    }

    public bool PickedUp()
    {
        return pickedUp;
    }
}
