using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoardEffect : MonoBehaviour
{
    public Transform camTransform;

    private Quaternion originalRotation;

    void Start()
    {
        originalRotation = transform.rotation;
        camTransform = GameManager.Instance.mainCam.transform;
    }

    void Update()
    {
        transform.rotation = camTransform.rotation * originalRotation;
    }
}
