using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorScript : MonoBehaviour
{
    public GameObject conveyorEnd;
    public float speed;
    private bool onBelt = false;
    private Vector3 direction;
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
                    Vector3 position  = currentCrate.transform.position + direction * moveSpeed;
                    currentCrate.GetComponent<Rigidbody>().MovePosition(position);
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
        if (obj.tag == "Pickup" && obj.transform.parent.GetComponent<PlayerModel>() == null)
        {
            currentCrate = obj;
            onBelt = true;
            direction = (conveyorEnd.transform.position - currentCrate.transform.position).normalized;
            Debug.Log("Enter");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.tag == "Pickup" && obj.transform.parent.GetComponent<PlayerModel>() == null)
        {
            onBelt = false;
            currentCrate = null;
            Debug.Log("Exit");
        }
    }
}
