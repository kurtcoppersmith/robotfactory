using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public CrateManager crateManager;

    private void Start()
    {
        crateManager = FindObjectOfType<CrateManager>();
    }
    

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hello");

        if(collision.transform.parent.gameObject.tag == "Player")
        {
            collision.transform.parent.gameObject.GetComponent<QTEManager>().Fail();
            crateManager.GetComponent<CrateManager>().Explode();
        }
    }
}
