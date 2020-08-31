using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public InputDevice lastDetectedDevice = null;
    public Camera mainCam;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        InputSystem.onEvent += (ptr, device) => { lastDetectedDevice = device; };
    }
}
