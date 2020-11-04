using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndChecker : MonoBehaviour
{
    public int currentLevelIndex = 0;
    public int gearThreshold1 = 0;
    public int gearThreshold2 = 0;
    public int gearThreshold3 = 0;

    public GameObject firstGearIcon;
    public GameObject secondGearIcon;
    public GameObject thirdGearIcon;

    public GameObject tutorialGearIcon;

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
            firstGearIcon.SetActive(true);
        }

        if (GameManager.Instance.GetGameData().scoreHolder.levelScores[currentLevelIndex].gear2)
        {
            secondGearIcon.SetActive(true);
        }

        if (GameManager.Instance.GetGameData().scoreHolder.levelScores[currentLevelIndex].gear3)
        {
            thirdGearIcon.SetActive(true);
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
                GameManager.Instance.addGears(1);
            }

            if(currentScore > gearThreshold2)
            {
                gear2 = true;
                if (!GameManager.Instance.gameData.scoreHolder.levelScores[currentLevelIndex].gear2)
                {
                    GameManager.Instance.addGears(1);
                }

                if (currentScore > gearThreshold3)
                {
                    gear3 = true;
                    if (!GameManager.Instance.gameData.scoreHolder.levelScores[currentLevelIndex].gear3)
                    {
                        GameManager.Instance.addGears(1);
                    }
                }
            }
        }

        GameManager.Instance.SetScoreAndGears(currentLevelIndex, currentScore, gear1, gear2, gear3);
    }
}
