using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelProperties : MonoBehaviour
{
    public Image gear1;
    public Image gear2;
    public Image gear3;
    public TMPro.TextMeshProUGUI highScoreMesh;

    void Start()
    {
    //    gear1.enabled = GameManager.Instance.GetGameData().scoreHolder.levelScores[UIManager.Instance.levelList.FindIndex(a => a == (transform.gameObject))].gear1;
    //    gear2.enabled = GameManager.Instance.GetGameData().scoreHolder.levelScores[UIManager.Instance.levelList.FindIndex(a => a == (transform.gameObject))].gear2;
    //    gear3.enabled = GameManager.Instance.GetGameData().scoreHolder.levelScores[UIManager.Instance.levelList.FindIndex(a => a == (transform.gameObject))].gear3;

    //    highScoreMesh.text = $"High Score: {GameManager.Instance.GetGameData().scoreHolder.levelScores[UIManager.Instance.levelList.FindIndex(a => a == (transform.gameObject))].highScore}";
    }
}
