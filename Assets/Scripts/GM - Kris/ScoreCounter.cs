using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    public TextMeshProUGUI scoreTextTMP;

    void Start()
    {
        scoreTextTMP.text = $"Score: { GameManager.Instance.returnScore()}";    
    }

    void Update()
    {
        if (!GameManager.Instance.isPaused && !GameManager.Instance.hasEnded)
        {
            scoreTextTMP.text = $"Score: { GameManager.Instance.returnScore()}";
        }
        else
        {
            scoreTextTMP.text = "";
        }
    }
}
