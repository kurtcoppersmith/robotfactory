﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoardEffect : MonoBehaviour
{
    public Transform camTransform;

    private Quaternion originalRotation;
    private Canvas canvas;

    void Start()
    {
        originalRotation = transform.localRotation;
        camTransform = LevelManager.Instance.mainCam.transform;

        canvas = GetComponent<Canvas>();
        canvas.worldCamera = LevelManager.Instance.mainCam;
    }

    void Update()
    {
        transform.rotation = LevelManager.Instance.mainCam.transform.rotation * originalRotation;
    }
}
