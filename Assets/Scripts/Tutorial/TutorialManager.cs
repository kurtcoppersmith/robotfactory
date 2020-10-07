using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using DG.Tweening;

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

    [Header("UI Stuff")]
    public GameObject textBoxObject;
    public TextMeshProUGUI descriptionTextMesh;
    private InputKeyUI inputKeyUI;

    public int currentObjective = 0;

    public GameObject cratePrefab;
    public Transform spawnLocation;
    [HideInInspector]
    public int spawnedCrateAmount = 0;

    public GameObject redBelt;
    public GameObject blueBelt;
    public GameObject greenBelt;

    public bool hasDescription = false;
    public bool hasCompletedCurrent = false;

    new void Awake()
    {
        base.Awake();
        inputKeyUI = GetComponent<InputKeyUI>();
    }

    void OnEnter(InputValue inputValue)
    {
        currentTextBox++;
    }

    void OnBackspace(InputValue inputValue)
    {
        currentTextBox = 0;
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

            switch (currentObjective)
            {
                case 1:
                    Instantiate(cratePrefab, spawnLocation.position, Quaternion.identity);
                    spawnedCrateAmount++;
                    break;
                case 2:
                    redBelt.GetComponent<MeshRenderer>().material.DOColor(Color.red, 3.0f);
                    greenBelt.GetComponent<MeshRenderer>().material.DOColor(Color.green, 3.0f);
                    blueBelt.GetComponent<MeshRenderer>().material.DOColor(Color.blue, 3.0f);
                    break;
            }

            hasCompletedCurrent = false;
        }

        if (currentObjective >= 2)
        {
            if (spawnedCrateAmount < 1)
            {
                Instantiate(cratePrefab, spawnLocation.position, Quaternion.identity);
                spawnedCrateAmount++;
            }
        }
    }
}
