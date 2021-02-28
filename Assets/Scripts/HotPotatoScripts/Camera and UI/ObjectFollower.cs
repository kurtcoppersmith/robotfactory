using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollower : MonoBehaviour
{
    public GameObject objectToFollow;

    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - objectToFollow.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = objectToFollow.transform.position + offset;
    }
}
