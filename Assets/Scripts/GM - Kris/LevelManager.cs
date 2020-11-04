using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonMonoBehaviour<LevelManager>
{
    public Camera mainCam;

    public GameObject pausePanel;
    public GameObject endPanel;

    public LevelEndChecker levelEnd;

    new void Awake()
    {
        base.Awake();

        levelEnd = GetComponent<LevelEndChecker>();
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

        levelEnd.CheckFinalScore();
    }

    public void CheckFinalScore()
    {
        levelEnd.CheckFinalScore();
    }
}
