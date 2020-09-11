using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public InputDevice lastDetectedDevice = null;
    public Camera mainCam;
    
    public bool isPaused { get; set; } = false;
    public bool hasEnded { get; set; } = false;
    
    //Float used by in game timer
    public float timeRemaining = 600;
    public int lives = 3;
    public int score = 0;

    new void Awake()
    {
        base.Awake();

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
            LevelManager.Instance.EnableEndScreen();
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
        
        if (score <= 0)
        {
            score = 0;
        }
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
}
