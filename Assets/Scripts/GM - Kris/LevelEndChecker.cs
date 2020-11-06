using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelEndChecker : MonoBehaviour
{
    public int currentLevelIndex = 0;
    public int gearThreshold1 = 0;
    public int gearThreshold2 = 0;
    public int gearThreshold3 = 0;

    public GameObject tutorialGearIcon;
    public GameObject firstGearIcon;
    public GameObject secondGearIcon;
    public GameObject thirdGearIcon;

    public Image[] gears;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highscoreText;
    public LevelManager scores;

    private int maxGears;

    public void TutorialGearAdder()
    {
        GameManager.Instance.addGears(1);
        GameManager.Instance.SetScoreAndGears(0, 100, false, true, false);

        tutorialGearIcon.SetActive(true);
        GameManager.Instance.HasCompletedTutorial(true);
    }

    void Start()
    {
        if (GameManager.Instance.GetGameData().scoreHolder.levelScores[currentLevelIndex].gear1)
        {
            gears[0].enabled = true;
            firstGearIcon.SetActive(true);
        }

        if (GameManager.Instance.GetGameData().scoreHolder.levelScores[currentLevelIndex].gear2)
        {
            gears[1].enabled = true;
            secondGearIcon.SetActive(true);
        }

        if (GameManager.Instance.GetGameData().scoreHolder.levelScores[currentLevelIndex].gear3)
        {
            gears[2].enabled = true;
            thirdGearIcon.SetActive(true);
        }

        maxGears = gears.Length;
        scores = FindObjectOfType<LevelManager>();
    }

    public void displayGears()
    {
        int playerScore = GameManager.Instance.returnScore();
        if (playerScore >= scores.getGearScore(1))
        {
            gears[0].enabled = true;
        }
        if (playerScore >= scores.getGearScore(2))
        {
            gears[1].enabled = true;
        }
        if (playerScore >= scores.getGearScore(3))
        {
            gears[2].enabled = true;
        }
    }

    public void CheckFinalScore()
    {
        bool gear1 = false;
        bool gear2 = false;
        bool gear3 = false;
        float currentScore = GameManager.Instance.returnScore();

        if (currentScore > gearThreshold1)
        {
            gear1 = true;
            if (!GameManager.Instance.gameData.scoreHolder.levelScores[currentLevelIndex].gear1)
            {
                gears[0].enabled = true;
                firstGearIcon.SetActive(true);
                GameManager.Instance.addGears(1);
            }

            if(currentScore > gearThreshold2)
            {
                gear2 = true;
                if (!GameManager.Instance.gameData.scoreHolder.levelScores[currentLevelIndex].gear2)
                {
                    gears[1].enabled = true;
                    secondGearIcon.SetActive(true);
                    GameManager.Instance.addGears(1);
                }

                if (currentScore > gearThreshold3)
                {
                    gear3 = true;
                    if (!GameManager.Instance.gameData.scoreHolder.levelScores[currentLevelIndex].gear3)
                    {
                        gears[2].enabled = true;
                        thirdGearIcon.SetActive(true);
                        GameManager.Instance.addGears(1);
                    }
                }
            }
        }

        GameManager.Instance.SetScoreAndGears(currentLevelIndex, currentScore, gear1, gear2, gear3);

        highscoreText.text = $"High Score: {GameManager.Instance.GetGameData().scoreHolder.levelScores[currentLevelIndex].highScore}";
        scoreText.text = $"Score: {currentScore}";
    }
}
