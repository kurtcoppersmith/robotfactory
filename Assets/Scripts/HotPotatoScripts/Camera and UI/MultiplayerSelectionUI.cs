using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MultiplayerSelectionUI : MonoBehaviour
{
    [Range(0, 3)]
    public int selectionIndex = 0;
    public TMPro.TextMeshProUGUI displayTextMesh;
    public TMPro.TMP_InputField inputField;

    private Character currentCharacter = null;
    private string basicText = "Press any button to join!";

    void Update()
    {
        if (PlayerManager.Instance.characters[selectionIndex] != null || PlayerManager.Instance.characters[selectionIndex] != currentCharacter)
        {
            if (PlayerManager.Instance.characters[selectionIndex] != currentCharacter && PlayerManager.Instance.characters[selectionIndex] == null)
            {
                displayTextMesh.text = basicText;
                currentCharacter = null;

                inputField.SetTextWithoutNotify("");
                inputField.gameObject.SetActive(false);
                return;
            }

            if (PlayerManager.Instance.characters[selectionIndex].gameObject.GetComponent<PlayerInput>() != null)
            {
                currentCharacter = PlayerManager.Instance.characters[selectionIndex];
                displayTextMesh.text = $"Player {selectionIndex + 1} joined!\n" +
                                       $"({PlayerManager.Instance.characters[selectionIndex].playerInput.currentControlScheme})";

                inputField.SetTextWithoutNotify(PlayerManager.Instance.characters[selectionIndex].characterName);
                inputField.gameObject.SetActive(true);
            }
        }
    }

    public void RemoveCurrent()
    {
        if (currentCharacter != null || PlayerManager.Instance.GetCurrentPlayerCharacterNumb() > 0)
        {
            inputField.SetTextWithoutNotify("");
            PlayerManager.Instance.characters[selectionIndex].characterName = "";

            displayTextMesh.text = basicText;
            Destroy(PlayerManager.Instance.characters[selectionIndex].gameObject);
            PlayerManager.Instance.characters[selectionIndex] = null;
            currentCharacter = null;

            inputField.gameObject.SetActive(false);

            PlayerManager.Instance.ReshuffleCharacters(selectionIndex);
        }
    }

    public void ChangeCharacterName()
    {
        PlayerManager.Instance.characters[selectionIndex].characterName = inputField.text;
    }

    public void EnableCharacterJoin()
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.inputManager.EnableJoining();
        }
    }

    public void DisableCharacterJoin()
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.inputManager.DisableJoining();
        }
    }
}
