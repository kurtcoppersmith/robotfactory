using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct ItemInfo
{
    public Text nameText;
    public Text costText;
    public Image itemImage;
    public int itemNumber;
}


public class ShopScript : MonoBehaviour
{

    public ItemInfo item1;
    public ItemInfo item2;
    public ItemInfo item3;
    public GameObject itemHolder1;
    public GameObject itemHolder2;
    public GameObject itemHolder3;
    private PowerUpManager item;

    public TMPro.TextMeshProUGUI gearsMesh;
    // Start is called before the first frame update
    void Start()
    {
        item = PowerUpManager.Instance;
        UpdateShop();
    }

    void OnEnable()
    {
        if (itemHolder1.activeInHierarchy)
        {
            DisableBoughtItems(item1.itemNumber);
        }

        DisableBoughtItems(item2.itemNumber);
        DisableBoughtItems(item3.itemNumber);
    }

    void Update()
    {
        gearsMesh.text = $"Gears: {GameManager.Instance.GetGameData().gears}";
    }

    public void UpdateShop()
    {
        //Set item names
        item1.nameText.text = item.powerUps[item1.itemNumber].name;
        item2.nameText.text = item.powerUps[item2.itemNumber].name;
        item3.nameText.text = item.powerUps[item3.itemNumber].name;
        //set item cost text
        item1.costText.text = "Cost: " + item.powerUps[item1.itemNumber].cost + " gear(s)";
        item2.costText.text = "Cost: " + item.powerUps[item2.itemNumber].cost + " gear(s)";
        item3.costText.text = "Cost: " + item.powerUps[item3.itemNumber].cost + " gear(s)";
        //set item image
        item1.itemImage.sprite = item.powerUps[item1.itemNumber].itemSprite;
        item2.itemImage.sprite = item.powerUps[item2.itemNumber].itemSprite;
        item3.itemImage.sprite = item.powerUps[item3.itemNumber].itemSprite;
    }

    public void Buy(int itemNumber)
    {
        int price = 0;
        switch(itemNumber)
        {
            case 0:
                price = item.powerUps[item1.itemNumber].cost;
                break;
            case 1:
                price = item.powerUps[item2.itemNumber].cost;
                break;
            case 2:
                price = item.powerUps[item3.itemNumber].cost;
                break;
            default:
                Debug.Log("Invalid itemNumber");
                break;
        }
        if(GameManager.Instance.GetGameData().gears - price >= 0)
        {
            GameManager.Instance.subGears(price);
            item.powerUps[itemNumber].unlocked = true;

            switch (item.powerUps[itemNumber].name)
            {
                case "Strength":
                    GameManager.Instance.GetGameData().boughtPowerups.strengthPowerup = true;
                    break;
                case "Chasis":
                    GameManager.Instance.GetGameData().boughtPowerups.chasisPowerup = true;
                    break;
                case "Speed":
                    GameManager.Instance.GetGameData().boughtPowerups.speedPowerup = true;
                    break;
            }

            GameManager.Instance.SaveGameData();

            //swich to disable item after you buy
            DisableBoughtItems(itemNumber);
        }
        else
        {
            Debug.Log("Not enough Gears");
        }
    }

    public void DisableBoughtItems(int itemNumber)
    {
        switch (PowerUpManager.Instance.powerUps[itemNumber].name)
        {
            case "Strength":
                switch (itemNumber)
                {
                    case 0:
                        itemHolder1.SetActive(!GameManager.Instance.GetGameData().boughtPowerups.strengthPowerup);
                        break;
                    case 1:
                        itemHolder2.SetActive(!GameManager.Instance.GetGameData().boughtPowerups.strengthPowerup);
                        break;
                    case 2:
                        itemHolder3.SetActive(!GameManager.Instance.GetGameData().boughtPowerups.strengthPowerup);
                        break;
                }
                break;
            case "Chasis":
                switch (itemNumber)
                {
                    case 0:
                        itemHolder1.SetActive(!GameManager.Instance.GetGameData().boughtPowerups.chasisPowerup);
                        break;
                    case 1:
                        itemHolder2.SetActive(!GameManager.Instance.GetGameData().boughtPowerups.chasisPowerup);
                        break;
                    case 2:
                        itemHolder3.SetActive(!GameManager.Instance.GetGameData().boughtPowerups.chasisPowerup);
                        break;
                }
                break;
            case "Speed":
                switch (itemNumber)
                {
                    case 0:
                        itemHolder1.SetActive(!GameManager.Instance.GetGameData().boughtPowerups.speedPowerup);
                        break;
                    case 1:
                        itemHolder2.SetActive(!GameManager.Instance.GetGameData().boughtPowerups.speedPowerup);
                        break;
                    case 2:
                        itemHolder3.SetActive(!GameManager.Instance.GetGameData().boughtPowerups.speedPowerup);
                        break;
                }
                break;
        }
    }

    public void DefaultItem(int num)
    {
        item.powerUps[num].cost = 0;
        item.powerUps[num].name = "";
        item.powerUps[num].itemSprite = null;
        UpdateShop();
    }
    
}
