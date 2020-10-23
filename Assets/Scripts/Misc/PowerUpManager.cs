using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Button b1;
    public Button b2;
    private bool isB1;
    private Sprite sprite;

    new void Awake()
    {
        base.Awake();
    }

    public void setB1(bool b)
    {
        isB1 = b;
        Debug.Log("B1?");
    }

    public void Equip(Image image)
    {
        if (isB1)
        {
            Sprite tempSprite = image.sprite;
            image.sprite = b1.image.sprite;
            b1.image.sprite = tempSprite;
        }else
        {
            Sprite tempSprite = image.sprite;
            image.sprite = b2.image.sprite;
            b2.image.sprite = tempSprite;
        }
        Debug.Log("Swap");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
