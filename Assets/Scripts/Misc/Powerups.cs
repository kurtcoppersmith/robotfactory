using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerups : MonoBehaviour
{
    public enum Enhancements
    {
        Robotic_Pulse,
        Robotic_Boost,
        Robotic_Chasis,
        Robotic_Enchancements,
        None
    }

    public Enhancements augments;
    private PlayerMovement playerVar;
    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        augments = Enhancements.None;
        playerVar = FindObjectOfType<PlayerMovement>();
        gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(augments)
        {
            case Enhancements.Robotic_Pulse:
                //add time!
                gm.addTime(15f);
                Debug.Log("time_looop!");
                break;
            case Enhancements.Robotic_Boost:
                //Speed up!
                playerVar.SetPlayerSped(true);
                Debug.Log("IAMSPEED!");
                break;
            case Enhancements.Robotic_Chasis:
                //NOU!
                playerVar.SetPlayerChasis(true);
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
