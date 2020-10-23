using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupsHelper : MonoBehaviour
{
    private Powerups powerup;

    void Start()
    {
        powerup = FindObjectOfType<Powerups>();
    }

    public void setPowerUp()
    {
        int rng = Random.Range(0, 3);
        switch (rng)
        {
            case 0:
                powerup.setEnhancement(Powerups.Enhancements.None);
                break;
            case 1:
                powerup.setEnhancement(Powerups.Enhancements.Robotic_Strength);
                break;
            case 2:
                powerup.setEnhancement(Powerups.Enhancements.Robotic_Boost);
                break;
            case 3:
                powerup.setEnhancement(Powerups.Enhancements.Robotic_Chasis);
                break;
            default:
                powerup.setEnhancement(Powerups.Enhancements.None);
                break;
        }
    }
}
