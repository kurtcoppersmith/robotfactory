using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public InputDevice lastDetectedDevice = null;

    public bool tempTankBool = false; //Temp bool for playtest

    public bool isPaused { get; set; } = false;
    public bool hasEnded { get; set; } = false;
    
    //Float used by in game timer
    public float timeRemaining = 150;
    public int lives = 3;
    public int score = 0;
    public Transform pm;

    public int gears = 0;

    public Text soundLevel;



    new void Awake()
    {
        base.Awake();

        InputSystem.onEvent += (ptr, device) => { lastDetectedDevice = device; };
    }

    void Start()
    {
        if (Instance == this)
        {
            DontDestroyOnLoad(this.gameObject);
        }
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

    // Below are the gear functions
    public void addGears(int gearsToAdd)
    {
        gears += gearsToAdd;
    }

    public int returnGears()
    {
        return gears;
    }

    public void setGears(int gearsToSet)
    {
        gears = gearsToSet;
    }

    public float[] returnPlayerPos()
    {
        pm = GameObject.FindGameObjectWithTag("Player").transform;
        float[] position = { pm.position.x, pm.position.y, pm.position.z };

        return position;
    }

    public void setPlayerPosition(float x, float y, float z)
    {
        Vector3 position; 
        position.x = x;
        position.y = y;
        position.z = z;
        GameObject.FindGameObjectWithTag("Player").transform.position = position;
    }

    public void SavePlayer()
    {
        SaveSystem.SaveData(this, "test");
    }

    public void LoadPlayer()
    {
        SaveData data = SaveSystem.LoadData("test");
        this.setScore(data.score);
        this.setTime(data.time);
      //  this.setPlayerPosition(data.position[0], data.position[1], data.position[2]);

        
    }


    public void SetVolume(float volume)
    {
        Debug.Log(volume);
        soundLevel.text = volume.ToString("F1");
    }

        
}
