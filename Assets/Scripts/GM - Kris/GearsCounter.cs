using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GearsCounter : MonoBehaviour
{
    public TextMeshProUGUI gearsText;

    void Start()
    {
        gearsText.text = $"Gears: { GameManager.Instance.returnGears()}";
    }

    void Update()
    {
        if (!GameManager.Instance.isPaused && !GameManager.Instance.hasEnded)
        {
            gearsText.text = $"Gears: { GameManager.Instance.returnGears()}";
        }
        else
        {
            gearsText.text = "";
        }
    }
}
