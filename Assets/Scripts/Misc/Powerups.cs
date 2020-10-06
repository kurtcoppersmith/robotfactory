using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerups : MonoBehaviour
{
    public enum Enhancements
    {
        Robotic_Strength,
        Robotic_Speed,
        Robotic_Shield,
        None
    }

    public Enhancements augments;
    private PlayerMovement playerVar;

    // Start is called before the first frame update
    void Start()
    {
        augments = Enhancements.None;
        playerVar = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(augments)
        {
            case Enhancements.Robotic_Strength:
                //double pick up!
                Debug.Log("stronk!");
                break;
            case Enhancements.Robotic_Speed:
                //Speed up!
                Debug.Log("IAMSPEED!");
                break;
            case Enhancements.Robotic_Shield:
                //NOU!
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
}
