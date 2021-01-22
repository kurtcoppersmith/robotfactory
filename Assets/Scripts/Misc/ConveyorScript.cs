using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorScript : MonoBehaviour
{
    public GameObject conveyorEnd;
    public Transform beginningOfConveyor;
    private float speed = 0;
    private Vector3 direction;
    private List<GameObject> crates;

    private void Start()
    {
        //speed = CrateManager.Instance.crateSpeed;
        crates = new List<GameObject>();

        direction = (conveyorEnd.transform.position - beginningOfConveyor.position).normalized;
        direction.y = 0;
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
            obj.transform.rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
            //direction = (conveyorEnd.transform.position - obj.transform.position).normalized;
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
