using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndScoreDisplay : MonoBehaviour
{
    public Image[] gears;
    public TextMeshProUGUI scoreText;

    private int maxGears;
    public LevelManager scores;
    
    // Start is called before the first frame update
    void Start()
    {
        maxGears = gears.Length;
        scoreText.text = $"{ GameManager.Instance.returnScore()}";
        scores = FindObjectOfType<LevelManager>();

        for(int i = 0; i < maxGears; i++)
        {
            gears[i].enabled = false;
        }
    }

    //public void displayGears()
    //{
    //    int playerScore = GameManager.Instance.returnScore();
    //    if(playerScore >= scores.getGearScore(1))
    //    {
    //        gears[0].enabled = true;
    //    }
    //    if(playerScore >= scores.getGearScore(2))
    //    {
    //        gears[1].enabled = true;
    //    }
    //    if(playerScore >= scores.getGearScore(3))
    //    {
    //        gears[2].enabled = true;
    //    }
    //}
}
