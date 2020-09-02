using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public InputDevice lastDetectedDevice = null;
    public Camera mainCam;
    public GameObject pausePanel;
    public GameObject endPanel;
    public bool isPaused { get; private set; } = false;
    public bool hasEnded { get; set; } = false;
    
    //Float used by in game timer
    public float timeRemaining = 600;
    public int lives = 3;
    public int score = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        InputSystem.onEvent += (ptr, device) => { lastDetectedDevice = device; };
    }


    // Below are the four functions involving the in game timer
    
    public void subtractTime()
    {
        if(timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            timeRemaining = 0;
            EnableEndScreen();
        }
    }

    public void addTime(float timeToAdd)
    {
        timeRemaining += timeToAdd;
    }

    public void setTime(float timeToSet)
    {
        timeRemaining = timeToSet;
    }

    public float returnTime()
    {
        return timeRemaining;
    }



    //Below are the four functions involving lives



    public void subLives(int livesToSub)
    {
        lives -= livesToSub;
        if(lives <= 0)
        {
            // call end game scenario for running out of lives
        }
    }
    
    public void addLives(int livesToAdd)
    {
        lives += livesToAdd;
    }

    public int livesRemaining()
    {
        return lives;
    }

    public void setLives(int setLives)
    {
        lives = setLives;
    }



    public void subScore(int scoreToSub)
    {
        score -= scoreToSub;
        
        /*if (score <= 0)
        {
            score = 0;
        }*/ //Commented out because I wanna see what people think about the ability to get a negative score. - Gavin
    }

    // Below are the score functions
    public void addScore(int scoreToAdd)
    {
        score += scoreToAdd;
    }

    public int returnScore()
    {
        return score;
    }

    public void setScore(int scoreToSet)
    {
        score = scoreToSet;
    }

    public void PauseToggle()
    {
        isPaused = !isPaused;
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
        hasEnded = true;
        HelperUtilities.UpdateCursorLock(false);
        endPanel.SetActive(true);
    }
}
