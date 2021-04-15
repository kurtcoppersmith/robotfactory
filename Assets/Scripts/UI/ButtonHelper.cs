using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHelper : MonoBehaviour
{
    public void PlayMenuSound()
    {
        SoundEffectsManager.Instance.Play("Menu_Select");
    }
}
