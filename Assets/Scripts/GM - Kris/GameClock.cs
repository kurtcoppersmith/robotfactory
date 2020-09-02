using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameClock : MonoBehaviour
{
    //Gamemanager which holds time variable
    public GameManager GM;

    //Three variables used for displaying the time
    float minutes;
    float seconds;
    float milliseconds;

    //In game UI Text which displays the current timer
    //public Text timeText;
    public TextMeshProUGUI timeTextTMP;

    // Start is called before the first frame update
    void Start()
    {
        //Attach current Gamemanager to the timer script
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //call GM SubtractTime script and TimerDisplay Script
        GM.subtractTime();
        TimerDisplay(GM.returnTime());
    }

    //Used to display the remaining float time in a traditional clock format
    void TimerDisplay(float clockTime)
    {
        //Three math functions to get current minutes, seconds, and milliseconds from float value
        minutes = Mathf.FloorToInt(clockTime / 60);
        seconds = Mathf.FloorToInt(clockTime % 60);
        milliseconds = (clockTime % 1) * 100;


        //timeText.text = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
        if (!GameManager.Instance.isPaused && !GameManager.Instance.hasEnded)
        {
            timeTextTMP.text = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
        }
        else
        {
            timeTextTMP.text = "";
        } 
    }
}
