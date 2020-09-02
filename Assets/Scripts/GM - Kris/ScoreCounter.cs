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
        scoreTextTMP.text = $"Score: { GameManager.Instance.returnScore()}";
    }
}
