﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JuiceCrate : MonoBehaviour
{
    public List<Transform> movementPath = new List<Transform>();
    public List<float> movementSpeeds = new List<float>();
    public float startTime = 0f;
    bool shouldCountDown = true;

    private void Update()
    {
        if (shouldCountDown)
        {
            startTime -= Time.deltaTime;
            if (startTime <= 0)
            {
                Vector3 startingPos = transform.position;

                Sequence moveSequence = DOTween.Sequence();

                for (int i = 0; i < movementPath.Count; i++)
                {
                    moveSequence.Append(transform.DOMove(movementPath[i].transform.position, movementSpeeds[i]));
                }

                if (movementPath.Count > 0)
                {
                    moveSequence.Append(transform.DOMove(startingPos, movementSpeeds[0]));
                }

                moveSequence.SetLoops(-1);
                moveSequence.Play();

                shouldCountDown = false;
            }
        }
    }
}