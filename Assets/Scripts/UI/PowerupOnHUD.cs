using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupOnHUD : MonoBehaviour
{
    public bool isPowerup1 = false;
    private Image HUDImage;

    public Sprite strengthSprite;
    public Sprite speedSprite;
    public Sprite chasisSprite;

    void Awake()
    {
        HUDImage = GetComponent<Image>();
        HUDImage.enabled = true;
    }


    void Start()
    {
        if (isPowerup1)
        {
            switch (GameManager.Instance.item1)
            {
                case "Strength":
                    HUDImage.sprite = strengthSprite;
                    break;
                case "Speed":
                    HUDImage.sprite = speedSprite;
                    break;
                case "Chasis":
                    HUDImage.sprite = chasisSprite;
                    break;
                default:
                    HUDImage.enabled = false;
                    break;
            }
        }
        else
        {
            switch (GameManager.Instance.item2)
            {
                case "Strength":
                    HUDImage.sprite = strengthSprite;
                    break;
                case "Speed":
                    HUDImage.sprite = speedSprite;
                    break;
                case "Chasis":
                    HUDImage.sprite = chasisSprite;
                    break;
                default:
                    HUDImage.enabled = false;
                    break;
            }
        }
    }
}
