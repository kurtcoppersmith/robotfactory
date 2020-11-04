using System.Collections;
using System.Collections.Generic;
using BasicTools.ButtonInspector;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

[System.Serializable]
public class LevelScores
{
    public float highScore;
    public bool gear1;
    public bool gear2;
    public bool gear3;
}

[System.Serializable]
public class ScoreHolder {
    public List<LevelScores> levelScores = new List<LevelScores>();
}

[System.Serializable]
public class GameData
{
    public ScoreHolder scoreHolder;
    public int gears;
    public bool hasCompletedTutorial;
}

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public InputDevice lastDetectedDevice = null;

    
    public GameData gameData { get; private set; }
    [Header("Save System Variables")]
    public string saveFile = "randFile.gav";
    public int amountOfLevels = 4;

    [SerializeField]
    [Button("Reset Game Data", "ResetGameData")]
    private bool _btnResetGameData;

    public bool isPaused { get; set; } = false;
    public bool hasEnded { get; set; } = false;
    
    [Header("Important Internal Variables")]
    //Float used by in game timer
    public float timeRemaining = 150;
    public int lives = 3;
    public int score = 0;
    public Transform pm;
    public int gears = 0;

    [Header("Powerup Variables")]
    //Strings to hold name of current powerups
    public string item1;
    public string item2;

    public Settings settingsFile;
    public float volume;


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

        gameData = SaveSystem.LoadData<GameData>(saveFile);

        if (gameData == null)
        {
            ResetGameData();
        }
    }

    void SaveGameData()
    {
        SaveSystem.SaveData<GameData>(gameData, saveFile);
    }

    void ResetGameData()
    {
        GameData tempGameData = new GameData();
        tempGameData.scoreHolder = new ScoreHolder();

        for (int i = 0; i < amountOfLevels; i++)
        {
            LevelScores levelScores = new LevelScores
            {
                highScore = 0,
                gear1 = false,
                gear2 = false,
                gear3 = false,
            };

            tempGameData.scoreHolder.levelScores.Add(levelScores);
        }
        tempGameData.gears = 0;
        tempGameData.hasCompletedTutorial = false;

        gameData = tempGameData;

        SaveGameData();
    }

    public GameData GetGameData()
    {
        return gameData;
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

    public void SetScoreAndGears(int currentLevelIndex, float currentScore, bool gear1, bool gear2, bool gear3)
    {
        if (gameData.scoreHolder.levelScores[currentLevelIndex].highScore < currentScore)
        {
            gameData.scoreHolder.levelScores[currentLevelIndex].highScore = currentScore;

            gameData.scoreHolder.levelScores[currentLevelIndex].gear1 = gear1;
            gameData.scoreHolder.levelScores[currentLevelIndex].gear2 = gear2;
            gameData.scoreHolder.levelScores[currentLevelIndex].gear3 = gear3;

            SaveGameData();
        }
    }

    public void HasCompletedTutorial(bool status)
    {
        gameData.hasCompletedTutorial = status;

        SaveGameData();
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
        gameData.gears = gears;
        SaveGameData();
    }

    public void subGears(int gearsToSub)
    {
        gears -= gearsToSub;
        gameData.gears = gears;
        SaveGameData();
    }

    public int returnGears()
    {
        return gears;
    }

    public void setGears(int gearsToSet)
    {
        gears = gearsToSet;
    }

    public void GetVolume()
    {
        volume = settingsFile.ReturnVolume();
    }
}
