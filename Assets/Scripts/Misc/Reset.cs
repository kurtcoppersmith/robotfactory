using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Pickup")
        {
            GameManager.Instance.subScore(1);
            CrateManager.Instance.Explode();
            CrateManager.Instance.spawnLocationStatus[CrateManager.Instance.currentSpawnedItems[other.gameObject]] = false;
            CrateManager.Instance.currentSpawnedItems.Remove(other.gameObject);

            other.gameObject.SetActive(false);
        }
    }
}
