using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateEndScore : MonoBehaviour
{
    private TextMeshProUGUI endScoreTextTMP;

    void Awake()
    {
        endScoreTextTMP = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        endScoreTextTMP.text = $"Score: { GameManager.Instance.returnScore() }";
    }
}
