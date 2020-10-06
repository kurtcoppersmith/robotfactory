using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorScript : MonoBehaviour
{
    public GameObject conveyorEnd;
    public float speed;
    bool onBelt = false;
    private GameObject currentCrate;

    // Update is called once per frame
    void Update()
    {
        float moveSpeed = speed * Time.deltaTime;
        if (currentCrate != null)
        {
            if (!currentCrate.GetComponent<IdleCrate>().PickedUp())
            {
                currentCrate.transform.position = Vector3.MoveTowards(currentCrate.transform.position, conveyorEnd.transform.position, moveSpeed);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.tag == "Pickup")
        {
            currentCrate = obj;
            onBelt = true;
            Debug.Log("Enter");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.tag == "Pickup")
        {
            onBelt = false;
            currentCrate = null;
            Debug.Log("Exit");
        }
    }
}
