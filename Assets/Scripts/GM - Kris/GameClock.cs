using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

static public class GameClock
{
    //Used to display the remaining float time in a traditional clock format
    public static string TimerDisplay(float clockTime)
    {
        //Three math functions to get current minutes, seconds, and milliseconds from float value
        float minutes = Mathf.FloorToInt(clockTime / 60);
        float seconds = Mathf.FloorToInt(clockTime % 60);
        float milliseconds = Mathf.FloorToInt((clockTime % 1) * 100);


        //timeText.text = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
        if (!GameManager.Instance.isPaused && !GameManager.Instance.hasEnded)
        {
            return string.Format("{0}:{1:00}:{2:00}", minutes, seconds, milliseconds);
        }
        else
        {
            return "";
        } 
    }
}
