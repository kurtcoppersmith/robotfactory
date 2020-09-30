using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{

    public int score;
    public float time;
    public float[] position;


    public SaveData(GameManager gm)
    {
        score = gm.returnScore();
        time = gm.returnTime();
      //  position = gm.returnPlayerPos();
    }
}
