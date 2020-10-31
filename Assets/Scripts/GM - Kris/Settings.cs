using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

// If you want the option to be saved across in the save data you'll need to create a public function here and have it pass to a variable in the GM
// See getVolume in the Gamemanager for reference

public class Settings
{
    public Text soundLevel;

    public float volume;

    public void SetVolume(float volVal)
    {
        // Remove the debug.log message and connect to volume manager
        volume = volVal;
        Debug.Log(volume);
        soundLevel.text = volVal.ToString("F1");
    }
    
    public float ReturnVolume()
    {
        return volume;
    }
}
