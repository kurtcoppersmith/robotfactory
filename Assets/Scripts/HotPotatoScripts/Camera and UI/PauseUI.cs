using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : SingletonMonoBehaviour<PauseUI>
{
    public bool canPause = false;
    public bool isPaused = false;
    public GameObject pausePanel;

    public void CanPause(bool toggle)
    {
        canPause = toggle;
    }

    public void TogglePause()
    {
        if (!canPause)
        {
            return;
        }

        isPaused = !isPaused;

        pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0.0f : 1.0f;
    }
}
