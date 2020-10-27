using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorScript : MonoBehaviour
{
    public GameObject conveyorEnd;
    private float speed;
    private Vector3 direction;
    private List<GameObject> crates;

    private void Start()
    {
        speed = CrateManager.Instance.crateSpeed;
        crates = new List<GameObject>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveSpeed = speed * Time.fixedDeltaTime;
        for(int i = 0; i < crates.Count; i++)
        {
            MoveCrate(crates[i], moveSpeed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.tag == "Pickup" && obj.transform.parent.GetComponent<PlayerModel>() == null)
        {
            crates.Add(obj);
            direction = (conveyorEnd.transform.position - crates[crates.Count-1].transform.position).normalized;
            //Debug.Log("Enter");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.tag == "Pickup" && obj.transform.parent.GetComponent<PlayerModel>() == null)
        {
            crates.Remove(obj);
            //Debug.Log("Exit");
        }
    }

    private void MoveCrate(GameObject crate, float moveSpeed)
    {
        if (!crate.GetComponent<IdleCrate>().PickedUp() && !GameManager.Instance.hasEnded)
        {
            if ((TutorialManager.Instance != null && TutorialManager.Instance.hasDescription && TutorialManager.Instance.currentObjective > 2) || (TutorialManager.Instance == null))
            {
                Vector3 position = crate.transform.position + direction * moveSpeed;
                crate.GetComponent<Rigidbody>().MovePosition(position);
            }
        }
        else
        {
            //crate = null;
            crates.Remove(crate);

        }

        if (crate != null && !crate.activeInHierarchy)
        {
            crates.Remove(crate);
        }
    }
}
