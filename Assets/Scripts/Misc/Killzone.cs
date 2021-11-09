﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Character currentChar = null;
        currentChar = other.GetComponent<Character>();
        if (currentChar != null)
        {
            if (currentChar.currentPickup != null)
            {
                currentChar.Drop();
                LevelManager.Instance.levelPickup.GetComponent<Pickup>().Dropped();
            }

            currentChar.characterController.enabled = false;
            currentChar.Spawn(LevelManager.Instance.spawnGroups[LevelManager.Instance.currentSpawnGroup].characterSpawnLocations[currentChar.playerIndex].position, Quaternion.identity);
            currentChar.characterController.enabled = true;
        }
    }
}
