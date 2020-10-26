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
        if (TutorialManager.Instance == null)
        {
            scoreTextTMP.text = $"{ GameManager.Instance.returnScore()}";
        }
    }

    void Update()
    {
        if (!GameManager.Instance.isPaused && !GameManager.Instance.hasEnded && TutorialManager.Instance == null)
        {
            scoreTextTMP.text = $"{ GameManager.Instance.returnScore()}";
        }
        else
        {
            scoreTextTMP.text = "";
        }
    }
}
