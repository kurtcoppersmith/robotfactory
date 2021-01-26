using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Scores
{
    public TMPro.TextMeshProUGUI scoreText;
    public Character character;
}

public class LevelManagerHP : SingletonMonoBehaviour<LevelManagerHP>
{
    public GameObject pausePanel;
    public GameObject endPanel;

    public LevelEndChecker levelEnd;
    //public EndScoreDisplay endCard;

    public Scores[] scores = new Scores[4];
    public int currentScoreIndex = 0;
    public GameObject currentHolder;
    public GameObject currentObject;

    new void Awake()
    {
        base.Awake();
        currentScoreIndex = 0;

        //levelEnd = GetComponent<LevelEndChecker>();
        //endCard = FindObjectOfType <EndScoreDisplay>();
    }

    private void Update()
    {
        for (int i = 0; i < scores.Length; i++)
        {
            scores[i].scoreText.text = $"{scores[i].character.characterName} - {scores[i].character.currentScore}";
        }
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
        //levelEnd.displayGears();


        levelEnd.CheckFinalScore();
    }

    public void CheckFinalScore()
    {
        levelEnd.CheckFinalScore();
    }
}
