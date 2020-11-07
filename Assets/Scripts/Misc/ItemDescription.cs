using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemDescription : MonoBehaviour
{
    private static ItemDescription instance;

    public Camera camera;

    private TextMeshProUGUI text;
    private RectTransform background;



    private void Awake()
    {
        instance = this;
        background = transform.Find("Background").GetComponent<RectTransform>();
        text = transform.Find("Description").GetComponent<TextMeshProUGUI>();
        HideDecription_Static();
    }

    private void Update()
    {

    }

    private void ShowDecription(Vector2 position, string description)
    {
        gameObject.SetActive(true);
        float textPadding = 8f;
        float x = text.GetPreferredValues(description).x*3/4;
        float y = text.GetPreferredValues(description).y + (textPadding * 2);
        text.SetText(description);
        Vector2 backgroundSize = new Vector2(x, y);
        background.sizeDelta = backgroundSize;
        transform.position = position;
        
    }

    private void HideDecription()
    {
        gameObject.SetActive(false);
    }

    public static void ShowDecription_Static(Vector2 position, string description)
    {
        instance.ShowDecription(position, description);
    }

    public static void HideDecription_Static()
    {
        instance.HideDecription();
    }

}
