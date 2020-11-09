using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearJuice : MonoBehaviour
{
    public bool turnRight = true;
    public float speed = 10;

    void Update()
    {
        if (turnRight)
        {
            transform.Rotate(0, 0, -speed * Time.deltaTime);
        }
        else
        {
            transform.Rotate(0, 0, speed * Time.deltaTime);
        }
        
    }
}
