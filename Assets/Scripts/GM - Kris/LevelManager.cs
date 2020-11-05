using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonMonoBehaviour<LevelManager>
{
    public Camera mainCam;

    public GameObject pausePanel;
    public GameObject endPanel;

    public LevelEndChecker levelEnd;
    public EndScoreDisplay endCard;

    public int score1 = 10;
    public int score2 = 20;
    public int score3 = 30;

    new void Awake()
    {
        base.Awake();

        levelEnd = GetComponent<LevelEndChecker>();
        //endCard = FindObjectOfType <EndScoreDisplay>();
    }

    public void PauseToggle()
    {
        GameManager.Instance.isPaused = !GameManager.Instance.isPaused;
        if (Time.timeScale == 0.0f)
        {
            Time.timeScale = 1.0f;
            HelperUtilities.UpdateCursorLock(true);
            pausePanel.SetActive(false);
        }
        else
        {
            Time.timeScale = 0.0f;
            HelperUtilities.UpdateCursorLock(false);
            pausePanel.SetActive(true);
        }
    }

    public void EnableEndScreen()
    {
        GameManager.Instance.hasEnded = true;
        HelperUtilities.UpdateCursorLock(false);
        endPanel.SetActive(true);
        endCard.displayGears();
        

        levelEnd.CheckFinalScore();
    }

    public void CheckFinalScore()
    {
        levelEnd.CheckFinalScore();
    }

    public int getGearScore(int index)
    {
        switch (index)
        {
            case 1:
                return score1;
            case 2:
                return score2;
            case 3:
                return score3;
            default:
                return score1;
        }
    }
}
