using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSelecter : MonoBehaviour
{
    public GameObject item1;
    public GameObject item2;
    public GameObject item3;

    // Update is called once per frame
    void Update()
    {
        item1.SetActive(PowerUpManager.Instance.powerUps[0].unlocked);
        item2.SetActive(PowerUpManager.Instance.powerUps[1].unlocked);
        item3.SetActive(PowerUpManager.Instance.powerUps[2].unlocked);
    }
}
