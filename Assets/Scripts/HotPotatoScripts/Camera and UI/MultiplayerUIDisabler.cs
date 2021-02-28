using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerUIDisabler : MonoBehaviour
{
    public Button continueButton;

    private void Update()
    {
        if (PlayerManager.Instance.GetCurrentPlayerCharacterNumb() > 0 && !continueButton.interactable)
        {
            continueButton.interactable = true;
        }
        else if (PlayerManager.Instance.GetCurrentPlayerCharacterNumb() == 0)
        {
            continueButton.interactable = false;
        }
    }
}
