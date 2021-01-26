using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHP : MonoBehaviour
{
    public bool canBeHeld = true;
    public float HoldBuffer = 1.5f;
    private float maxHoldBuffer = 0;

    private void Awake()
    {
        maxHoldBuffer = HoldBuffer;
    }

    private void Update()
    {
        if (!canBeHeld)
        {
            HoldBuffer -= Time.deltaTime;

            if (HoldBuffer <= 0)
            {
                canBeHeld = true;
                HoldBuffer = maxHoldBuffer;
            }
        }
    }

    public void SetHoldFalse()
    {
        canBeHeld = false;
    }
}
