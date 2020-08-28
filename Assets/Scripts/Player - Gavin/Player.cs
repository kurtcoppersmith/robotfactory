using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMovement playerMovement;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }
}
