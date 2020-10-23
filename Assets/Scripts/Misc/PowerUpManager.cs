using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerUps
{
    public string name;
    public int cost;
    public Sprite itemSprite;
    public bool unlocked;
}

public class PowerUpManager : SingletonMonoBehaviour<PowerUpManager>
{
    

    public int numberOfPowerUps = 3;
    public PowerUps[] powerUps;

    new void Awake()
    {
        base.Awake();
        powerUps = new PowerUps[numberOfPowerUps];
    }

    void Start()
    {
        for(int i = 0; i<numberOfPowerUps; i++)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
