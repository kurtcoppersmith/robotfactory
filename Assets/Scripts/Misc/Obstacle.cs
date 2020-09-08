using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.parent != null && collision.transform.parent.gameObject.tag == "Player")
        {
            collision.transform.parent.gameObject.GetComponent<QTEManager>().Fail();
        }
    }
}
