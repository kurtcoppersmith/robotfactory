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
        item1.SetActive(GameManager.Instance.GetGameData().boughtPowerups.strengthPowerup);
        item2.SetActive(GameManager.Instance.GetGameData().boughtPowerups.speedPowerup);
        item3.SetActive(GameManager.Instance.GetGameData().boughtPowerups.chasisPowerup);
    }
}
