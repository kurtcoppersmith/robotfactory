using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int itemNumber;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector2 offSet = new Vector2(50, 0);
        Vector2 pos = (Vector2)this.gameObject.transform.position + offSet;
        string description = PowerUpManager.Instance.powerUps[itemNumber].description;
        ItemDescription.ShowDecription_Static(pos,description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemDescription.HideDecription_Static();
    }
}
