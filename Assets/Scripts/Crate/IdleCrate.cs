using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleCrate : MonoBehaviour
{
    bool pickedUp = false;
    public float speed = 10;
    public GameObject reset;

    public void PickUp(bool status)
    {
        pickedUp = status;
    }

    public bool PickedUp()
    {
        return pickedUp;
    }

    // Update is called once per frame
    void Update()
    {
        float moveSpeed = speed * Time.deltaTime;
        if(!pickedUp)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, reset.transform.position, moveSpeed);
        }
    }
}
