﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PowerUps
{
    public string name;
    public int cost;
    public Sprite itemSprite;
    public string description;
    public bool unlocked;
    public bool equipped1;
    public bool equipped2;
}

public class PowerUpManager : SingletonMonoBehaviour<PowerUpManager>
{
    public int numberOfPowerUps = 3;
    public PowerUps[] powerUps;
    public Button b1;
    public Button b2;
    private bool isB1;
    private Sprite sprite;
    private GameManager gm;

    new void Awake()
    {
        base.Awake();
        gm = GameManager.Instance;
    }

    void Start()
    {
        b1.image.sprite = null;
        b2.image.sprite = null;
        for (int i = 0; i < powerUps.Length; i++)
        {
            if (powerUps[i].name == GameManager.Instance.item1)
            {
                isB1 = true;
                Equip(i);
            }

            if (powerUps[i].name == GameManager.Instance.item2)
            {
                isB1 = false;
                Equip(i);
            }
        }
        if (b1.image.sprite == null)
            b1.transform.GetChild(0).gameObject.SetActive(true);
        else
            b1.transform.GetChild(0).gameObject.SetActive(false);
        if (b2.image.sprite == null)
            b2.transform.GetChild(0).gameObject.SetActive(true);
        else
            b2.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void setB1(bool b)
    {
        isB1 = b;
    }

    public void Equip(int item)
    {
        if (isB1)
        {
            for(int i=0; i<powerUps.Length; i++)
            {
                if (i != item)
                    powerUps[i].equipped1 = false;
            }
            powerUps[item].equipped1 = !powerUps[item].equipped1;
            if (powerUps[item].equipped1)
            {
                if (powerUps[item].equipped2)
                {
                    b2.image.sprite = null;
                    powerUps[item].equipped2 = false;
                }
                b1.image.sprite = powerUps[item].itemSprite;
            }
            else
                b1.image.sprite = null;
            setPower(b1.image.sprite);
        }
        else
        {
            for (int i = 0; i < powerUps.Length; i++)
            {
                if (i != item)
                    powerUps[i].equipped2 = false;
            }
            powerUps[item].equipped2 = !powerUps[item].equipped2;
            if (powerUps[item].equipped2)
            {
                if (powerUps[item].equipped1)
                {
                    b1.image.sprite = null;
                    powerUps[item].equipped1 = false;
                }
            b2.image.sprite = powerUps[item].itemSprite;
            }
            else
                b2.image.sprite = null; 
            setPower(b2.image.sprite);
        }
        if (b1.image.sprite == null)
            b1.transform.GetChild(0).gameObject.SetActive(true);
        else
            b1.transform.GetChild(0).gameObject.SetActive(false);
        if (b2.image.sprite == null)
            b2.transform.GetChild(0).gameObject.SetActive(true);
        else
            b2.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void setPower(Sprite s)
    {
        for (int i=0; i<powerUps.Length;i++)
        {
            if(s == powerUps[i].itemSprite)
            {
                if (isB1)
                    gm.item1 = powerUps[i].name;
                else
                    gm.item2 = powerUps[i].name;

            }
        }
    }
}
