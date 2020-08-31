using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGoal : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<QTEManager>().Passed();
        }
    }
}
