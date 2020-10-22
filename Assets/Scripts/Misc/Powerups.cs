using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerups : MonoBehaviour
{
    public enum Enhancements
    {
        Robotic_Strength,
        Robotic_Boost,
        Robotic_Chasis,
        Robotic_Enchancements,
        None
    }

    public Enhancements augments;
    private PlayerModel playerVar;

    // Start is called before the first frame update
    void Start()
    {
        augments = Enhancements.None;
        playerVar = FindObjectOfType<PlayerModel>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(augments)
        {
            case Enhancements.Robotic_Strength:
                //add time!
                playerVar.playerPowerups.SetStrengthPowerup(true);
                Debug.Log("time_looop!");
                break;
            case Enhancements.Robotic_Boost:
                //Speed up!
                playerVar.playerPowerups.SetSpeedPowerup(true);
                Debug.Log("IAMSPEED!");
                break;
            case Enhancements.Robotic_Chasis:
                //NOU!
                playerVar.playerPowerups.SetChasisPowerup(true);
                Debug.Log("PROTECC!");
                break;
            default:
                break;
        }
    }

    public Enhancements getEnhancement()
    {
        return augments;
    }

    public void setEnhancement(Enhancements effect)
    {
        augments = effect;
    }
}
