using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void LoadNextScene(string sceneName)
    {
        Time.timeScale = 1.0f;
        if (GameManager.Instance != null)
        {
            GameManager.Instance.setTime(150);
            GameManager.Instance.setScore(0);
            GameManager.Instance.hasEnded = false;
            GameManager.Instance.isPaused = false;
        }

        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
