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
    private PowerUpManager item;
    // Start is called before the first frame update
    void Start()
    {
        item = PowerUpManager.Instance;
        UpdateShop();
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
        if(GameManager.Instance.returnGears() - price >= 0)
        {
            GameManager.Instance.setGears(GameManager.Instance.returnGears() - price);
            item.powerUps[itemNumber].unlocked = true;
            DefaultItem(itemNumber);
            Debug.Log("Just bought " + item.powerUps[itemNumber].name);
        }
        else
        {
            Debug.Log("Not enough Gears");
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
