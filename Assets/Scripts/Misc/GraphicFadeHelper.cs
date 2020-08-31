using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicFadeHelper : MonoBehaviour
{
    public float minOpacity = 0f;
    public float maxOpacity = 1f;

    public bool useStartingOpacityAsMin = false;
    public bool useStartingOpacityAsMax = true;

    private Graphic graphic;

    void Awake()
    {
        RefreshIfNeeded();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshIfNeeded()
    {
        if (graphic == null)
        {
            graphic = GetComponent<Graphic>();

            if (useStartingOpacityAsMin)
            {
                minOpacity = graphic.color.a;
            }

            if (useStartingOpacityAsMax)
            {
                maxOpacity = graphic.color.a;
            }
        }
    }
}
