using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempToggleOptions : MonoBehaviour
{
    public UnityEngine.UI.Toggle tankOptionToggle;

    void Awake()
    {
        tankOptionToggle = GetComponent<UnityEngine.UI.Toggle>();

        bool temp = GameManager.Instance.tempTankBool;
        tankOptionToggle.isOn = temp;
        GameManager.Instance.tempTankBool = temp;
    }

    public void ToggleTankOptions()
    {
        GameManager.Instance.tempTankBool = !GameManager.Instance.tempTankBool;
    }
}
