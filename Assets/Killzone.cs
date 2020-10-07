using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour
{
    public GameObject origin;

    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = origin.transform.position;
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.position = origin.transform.position;
    }
}
