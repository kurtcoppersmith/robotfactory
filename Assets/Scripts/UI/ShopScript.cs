using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct ItemInfo
{
    public string name;
    public Text nameText;
    public int cost;
    public Text costText;
    public Sprite itemSprite;
    public Image itemImage;
}


public class ShopScript : MonoBehaviour
{

    public ItemInfo item1;
    public ItemInfo item2;
    public ItemInfo item3;
    // Start is called before the first frame update
    void Start()
    {
        //Set item names
        item1.nameText.text = item1.name;
        item2.nameText.text = item2.name;
        item3.nameText.text = item3.name;
        //set item cost text
        item1.costText.text = "Cost: " + item1.cost + " gear(s)";
        item2.costText.text = "Cost: " + item2.cost + " gear(s)";
        item3.costText.text = "Cost: " + item3.cost + " gear(s)";
        //set item image
        item1.itemImage.sprite = item1.itemSprite;
        item2.itemImage.sprite = item2.itemSprite;
        item3.itemImage.sprite = item3.itemSprite;
    }

    public void Buy(int itemNumber)
    {
        int price = 0;
        switch(itemNumber)
        {
            case 1:
                price = item1.cost;
                break;
            case 2:
                price = item1.cost;
                break;
            case 3:
                price = item1.cost;
                break;
            default:
                Debug.Log("Invalid itemNumber");
                break;
        }
        if(GameManager.Instance.returnGears() - price >= 0)
        {
            GameManager.Instance.setGears(GameManager.Instance.returnGears() - price);
            //DefaultInfo(info);
        }
        else
        {
            Debug.Log("Not enough Gears");
        }
    }

    void DefaultInfo(ItemInfo info)
    {
        info.name = "";
        info.cost = 0;
        info.itemImage = null;
    }

    
}
