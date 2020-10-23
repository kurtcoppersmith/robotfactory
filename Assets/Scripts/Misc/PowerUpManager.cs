using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : SingletonMonoBehaviour<PowerUpManager>
{
    public class PowerUps
    {
        public string name;
        public int cost;
        public Sprite itemSprite;
    }

    public int numberOfPowerUps = 3;

    new void Awake()
    {
        base.Awake();

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
