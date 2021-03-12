using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessagePrompt : MonoBehaviour
{
    public GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas.SetActive(false);
    }

    void OnTriggerEnter(Collider player)
    {
        canvas.SetActive(true);
    }

    void OnTriggerExit(Collider player)
    {
        canvas.SetActive(false);
    }
}
