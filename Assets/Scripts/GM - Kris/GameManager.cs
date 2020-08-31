using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public InputDevice lastDetectedDevice = null;
    public Camera mainCam;
    
    //Float used by in game timer
    private float timeRemaining = 600;
    private int lives = 600;

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
        if(timeRemaining >= 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            timeRemaining = 0;
            //write the end-game scenario for running out of time here
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
}
