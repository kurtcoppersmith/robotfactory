using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaPit : MonoBehaviour
{
    public string lavaSFX;
    private void OnTriggerEnter(Collider other)
    {
        Character currentChar = null;
        currentChar = other.GetComponent<Character>();
        if (currentChar != null)
        {
            if (currentChar.currentPickup != null)
            {
                currentChar.currentPickup.GetComponent<Pickup>().Dropped();
                currentChar.Drop();
            }

            LevelManager.Instance.CheckPlayerAudioRange(other.gameObject.transform.position, lavaSFX);
        }
    }
}
