using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

[System.Serializable]
public class Description
{
    [TextArea]
    public List<string> currentObjectivesKeyboard;
    [TextArea]
    public List<string> currentObjectivesXBox;
    [TextArea]
    public List<string> currentObjectivesPS4;
}

public class TutorialManager : SingletonMonoBehaviour<TutorialManager>
{
    public const string KeyboardSchemeName = "Keyboard";
    public const string PS4SchemeName = "PS4";
    public const string XBoxSchemeName = "XBox";

    public List<Description> descriptions;
    private int currentTextBox = 0;
    public List<GameObject> currentObjectiveColliders;

    [Header("UI Stuff")]
    public GameObject textBoxObject;
    public TextMeshProUGUI descriptionTextMesh;
    private InputKeyUI inputKeyUI;

    public int currentObjective = 0;

    private bool hasDescription = false;
    private bool hasCompletedCurrent = false;

    new void Awake()
    {
        base.Awake();
        inputKeyUI = GetComponent<InputKeyUI>();
    }

    void OnEnter(InputValue inputValue)
    {
        currentTextBox++;
    }

    void Update()
    {
        if (!hasDescription)
        {
            switch (inputKeyUI.lastSeenControlScheme)
            {
                case KeyboardSchemeName:
                    if (currentTextBox < descriptions[currentObjective].currentObjectivesKeyboard.Count)
                    {
                        descriptionTextMesh.text = descriptions[currentObjective].currentObjectivesKeyboard[currentTextBox];
                    }
                    else
                    {
                        hasDescription = true;
                        currentObjectiveColliders[currentObjective].SetActive(true);
                        textBoxObject.SetActive(false);
                    }
                    break;
                case PS4SchemeName:
                    if (currentTextBox < descriptions[currentObjective].currentObjectivesPS4.Count)
                    {
                        descriptionTextMesh.text = descriptions[currentObjective].currentObjectivesPS4[currentTextBox];
                    }
                    else
                    {
                        hasDescription = true;
                        currentObjectiveColliders[currentObjective].SetActive(true);
                        textBoxObject.SetActive(false);
                    }
                    break;
                case XBoxSchemeName:
                    if (currentTextBox < descriptions[currentObjective].currentObjectivesXBox.Count)
                    {
                        descriptionTextMesh.text = descriptions[currentObjective].currentObjectivesXBox[currentTextBox];
                    }
                    else
                    {
                        hasDescription = true;
                        currentObjectiveColliders[currentObjective].SetActive(true);
                        textBoxObject.SetActive(false);
                    }
                    break;
            }
        }

        if (hasCompletedCurrent)
        {
            currentObjective++;
            hasDescription = false;
            textBoxObject.SetActive(true);
            currentTextBox = 0;
            hasCompletedCurrent = false;
        }
    }
}
