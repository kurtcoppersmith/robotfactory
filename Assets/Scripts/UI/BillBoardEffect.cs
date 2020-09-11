using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoardEffect : MonoBehaviour
{
    public Transform camTransform;

    private Quaternion originalRotation;
    private Canvas canvas;

    void Start()
    {
        originalRotation = transform.rotation;
        camTransform = LevelManager.Instance.mainCam.transform;

        canvas = GetComponent<Canvas>();
        canvas.worldCamera = LevelManager.Instance.mainCam;
    }

    void Update()
    {
        transform.rotation = camTransform.rotation * originalRotation;
    }
}
