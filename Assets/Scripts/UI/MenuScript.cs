using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public UIManager levelInt;
    public string tutorialLevel = "MainTutorial";
    public string factoryLevel = "FactoryLevel";
    public string lavaLevel = "LavaLevelOne";
    public string iceLevel = "IceLevelOne";
    public string mainMenu = "fixedMainMenu";
    public string levelSelect = "LevelSelect";
    private string curLevel;

    public void LoadNextScene()
    {
        Time.timeScale = 1.0f;
        if (GameManager.Instance != null)
        {
            GameManager.Instance.setTime(150);
            GameManager.Instance.setScore(0);
            GameManager.Instance.hasEnded = false;
            GameManager.Instance.isPaused = false;
        }

        checkLevel();
        SceneManager.LoadScene(curLevel);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenu);
    }

    public void LevelSelectMenu()
    {
        SceneManager.LoadScene(levelSelect);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ButtonClickSound(string audioClipName)
    {
        SoundEffectsManager.Instance.Play(audioClipName);
    }

    private void checkLevel()
    {
        switch(levelInt.curLevel)
        {
            case 0:
                curLevel = tutorialLevel;
                break;
            case 1:
                curLevel = factoryLevel;
                break;
            case 2:
                curLevel = lavaLevel;
                break;
            case 3:
                curLevel = iceLevel;
                break;
            default:
                curLevel = tutorialLevel;
                break;
        }
    }
}
