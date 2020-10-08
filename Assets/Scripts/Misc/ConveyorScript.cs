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
            if (!currentCrate.GetComponent<IdleCrate>().PickedUp() && !GameManager.Instance.hasEnded)
            {
                if ((TutorialManager.Instance != null && TutorialManager.Instance.hasDescription && TutorialManager.Instance.currentObjective > 2) || (TutorialManager.Instance == null))
                {
                    currentCrate.transform.position = Vector3.MoveTowards(currentCrate.transform.position, conveyorEnd.transform.position, moveSpeed);
                }
            }
            else
            {
                currentCrate = null;
            }

            if (currentCrate != null && !currentCrate.activeInHierarchy)
            {
                currentCrate = null;
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
