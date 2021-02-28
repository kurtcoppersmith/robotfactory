using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.UI;

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
    public Text dummyUIText;
    public float textBoxTimer = 0.5f;
    private float maxTextBoxTimer;
    private bool finishedCurrentDialogue = false;
    private bool textBoxUnderscore = false;
    private InputKeyUI inputKeyUI;

    [Header("Input Variables")]
    public InputActionAsset inputActionAsset;
    public string inputActionMapName;
    private InputActionMap inputActionMap = null;

    [Header("Objective Counter")]
    public int currentObjective = 0;

    [Header("Spawning Stuff")]
    public GameObject atomBomb;
    public GameObject fuseBomb;
    public GameObject tntBomb;
    public Transform spawnLocation;
    public Transform movingSpawnLocation;
    [HideInInspector]
    public int spawnedCrateAmount = 0;

    [Header("Second Objective")]
    public GameObject atomSign;
    public GameObject fuseSign;
    public GameObject tntSign;

    [Header("Third Objective")]
    //public bool hasDashed = false;

    [Header("Fourth Objective")]
    public float hazardSpawnTimer = 0;
    private float maxHazardSpawnTimer = 0;
    public bool hitWire = false;
    public bool hitOil = false;

    public float finalCompleteTextTimer = 0;
    private float maxFinalCompleteTextTimer = 0;

    public bool hasDescription = false;
    public bool hasCompletedCurrent = false;

    new void Awake()
    {
        base.Awake();
        inputKeyUI = GetComponent<InputKeyUI>();

        maxHazardSpawnTimer = hazardSpawnTimer;
        maxFinalCompleteTextTimer = finalCompleteTextTimer;

        maxTextBoxTimer = textBoxTimer;
    }

    void Start()
    {
        Invoke("InitScreen", 1);
    }

    void InitScreen()
    {
        dummyUIText.text = "";

        switch (inputKeyUI.lastSeenControlScheme)
        {
            case KeyboardSchemeName:
                dummyUIText.DOText(descriptions[currentObjective].currentObjectivesKeyboard[currentTextBox], 4.5f, true);
                break;
            case PS4SchemeName:
                dummyUIText.DOText(descriptions[currentObjective].currentObjectivesPS4[currentTextBox], 4.5f, true);
                break;
            case XBoxSchemeName:
                dummyUIText.DOText(descriptions[currentObjective].currentObjectivesXBox[currentTextBox], 4.5f, true);
                break;
            default:
                Debug.Log("Broken.");
                break;
        }
    }

    public void StartOverDialogue()
    {
        finishedCurrentDialogue = false;

        if (currentTextBox < descriptions[currentObjective].currentObjectivesKeyboard.Count)
        {
            dummyUIText.text = "";
            dummyUIText.DOKill();
            descriptionTextMesh.text = "";
            switch (inputKeyUI.lastSeenControlScheme)
            {
                case KeyboardSchemeName:
                    dummyUIText.DOText(descriptions[currentObjective].currentObjectivesKeyboard[currentTextBox], 4.5f, true);
                    break;
                case PS4SchemeName:
                    dummyUIText.DOText(descriptions[currentObjective].currentObjectivesPS4[currentTextBox], 4.5f, true);
                    break;
                case XBoxSchemeName:
                    dummyUIText.DOText(descriptions[currentObjective].currentObjectivesXBox[currentTextBox], 4.5f, true);
                    break;
            }
        }
    }

    void OnEnter(InputValue inputValue)
    {
        if (currentTextBox < descriptions[currentObjective].currentObjectivesKeyboard.Count)
        {
            if (descriptionTextMesh.text != descriptions[currentObjective].currentObjectivesKeyboard[currentTextBox] && !finishedCurrentDialogue)
            {
                dummyUIText.DOKill();
                descriptionTextMesh.text = descriptions[currentObjective].currentObjectivesKeyboard[currentTextBox];

                finishedCurrentDialogue = true;
            }
            else
            {
                currentTextBox++;
                finishedCurrentDialogue = false;

                if (currentTextBox < descriptions[currentObjective].currentObjectivesKeyboard.Count)
                {
                    dummyUIText.text = "";
                    dummyUIText.DOKill();
                    descriptionTextMesh.text = "";
                    switch (inputKeyUI.lastSeenControlScheme)
                    {
                        case KeyboardSchemeName:
                            dummyUIText.DOText(descriptions[currentObjective].currentObjectivesKeyboard[currentTextBox], 4.5f, true);
                            break;
                        case PS4SchemeName:
                            dummyUIText.DOText(descriptions[currentObjective].currentObjectivesPS4[currentTextBox], 4.5f, true);
                            break;
                        case XBoxSchemeName:
                            dummyUIText.DOText(descriptions[currentObjective].currentObjectivesXBox[currentTextBox], 4.5f, true);
                            break;
                    }
                }
            }
        }
    }

    void OnBackspace(InputValue inputValue)
    {
        currentTextBox = 0;
        finishedCurrentDialogue = false;

        dummyUIText.text = "";
        dummyUIText.DOKill();
        descriptionTextMesh.text = "";
        switch (inputKeyUI.lastSeenControlScheme)
        {
            case KeyboardSchemeName:
                dummyUIText.DOText(descriptions[currentObjective].currentObjectivesKeyboard[currentTextBox], 4.5f, true);
                break;
            case PS4SchemeName:
                dummyUIText.DOText(descriptions[currentObjective].currentObjectivesPS4[currentTextBox], 4.5f, true);
                break;
            case XBoxSchemeName:
                dummyUIText.DOText(descriptions[currentObjective].currentObjectivesXBox[currentTextBox], 4.5f, true);
                break;
        }
    }

    void Update()
    {
        Debug.Log(inputKeyUI.lastSeenControlScheme);

        if (!hasDescription)
        {
            switch (inputKeyUI.lastSeenControlScheme)
            {
                case KeyboardSchemeName:
                    if (currentTextBox < descriptions[currentObjective].currentObjectivesKeyboard.Count)
                    {
                        if (descriptionTextMesh.text != descriptions[currentObjective].currentObjectivesKeyboard[currentTextBox] && !finishedCurrentDialogue)
                        {
                            descriptionTextMesh.text = dummyUIText.text;
                        }
                        else
                        {
                            finishedCurrentDialogue = true;
                            textBoxTimer -= Time.deltaTime;
                            if (textBoxTimer <= 0)
                            {
                                textBoxUnderscore = !textBoxUnderscore;
                                textBoxTimer = maxTextBoxTimer;
                            }

                            if (textBoxUnderscore)
                            {
                                descriptionTextMesh.text = descriptions[currentObjective].currentObjectivesKeyboard[currentTextBox];
                                descriptionTextMesh.text = descriptionTextMesh.text + "_";
                            }
                            else
                            {
                                descriptionTextMesh.text = descriptions[currentObjective].currentObjectivesKeyboard[currentTextBox];
                            }
                        }
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
                        if (descriptionTextMesh.text != descriptions[currentObjective].currentObjectivesPS4[currentTextBox] && !finishedCurrentDialogue)
                        {
                            descriptionTextMesh.text = dummyUIText.text;
                        }
                        else
                        {
                            finishedCurrentDialogue = true;
                            textBoxTimer -= Time.deltaTime;
                            if (textBoxTimer <= 0)
                            {
                                textBoxUnderscore = !textBoxUnderscore;
                                textBoxTimer = maxTextBoxTimer;
                            }

                            if (textBoxUnderscore)
                            {
                                descriptionTextMesh.text = descriptions[currentObjective].currentObjectivesPS4[currentTextBox];
                                descriptionTextMesh.text = descriptionTextMesh.text + "_";
                            }
                            else
                            {
                                descriptionTextMesh.text = descriptions[currentObjective].currentObjectivesPS4[currentTextBox];
                            }
                        }
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
                        if (descriptionTextMesh.text != descriptions[currentObjective].currentObjectivesXBox[currentTextBox] && !finishedCurrentDialogue)
                        {
                            descriptionTextMesh.text = dummyUIText.text;
                        }
                        else
                        {
                            finishedCurrentDialogue = true;
                            textBoxTimer -= Time.deltaTime;
                            if (textBoxTimer <= 0)
                            {
                                textBoxUnderscore = !textBoxUnderscore;
                                textBoxTimer = maxTextBoxTimer;
                            }

                            if (textBoxUnderscore)
                            {
                                descriptionTextMesh.text = descriptions[currentObjective].currentObjectivesXBox[currentTextBox];
                                descriptionTextMesh.text = descriptionTextMesh.text + "_";
                            }
                            else
                            {
                                descriptionTextMesh.text = descriptions[currentObjective].currentObjectivesXBox[currentTextBox];
                            }
                        }
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
            finishedCurrentDialogue = false;

            dummyUIText.text = "";
            dummyUIText.DOKill();
            descriptionTextMesh.text = "";

            switch (inputKeyUI.lastSeenControlScheme)
            {
                case KeyboardSchemeName:
                    dummyUIText.DOText(descriptions[currentObjective].currentObjectivesKeyboard[currentTextBox], 4.5f, true);
                    break;
                case PS4SchemeName:
                    dummyUIText.DOText(descriptions[currentObjective].currentObjectivesPS4[currentTextBox], 4.5f, true);
                    break;
                case XBoxSchemeName:
                    dummyUIText.DOText(descriptions[currentObjective].currentObjectivesXBox[currentTextBox], 4.5f, true);
                    break;
            }

            switch (currentObjective)
            {
                case 1:
                    atomSign.transform.DOLocalMoveY(1.90f, 3.0f);
                    fuseSign.transform.DOLocalMoveY(1.90f, 3.0f);
                    tntSign.transform.DOLocalMoveY(1.90f, 3.0f);

                    spawnedCrateAmount++;
                    break;
                case 3:
                    HazardManager.Instance.TutorialSpawnHazards();
                    break;
                case 4:
                    //LevelManager.Instance.levelEnd.TutorialGearAdder();
                    break;
            }

            hasCompletedCurrent = false;
        }

        if (currentObjective >= 1)
        {
            if (spawnedCrateAmount < 1)
            {
                int randNumb = Random.Range(0, 3);

                switch (randNumb)
                {
                    case 0:
                        Instantiate(atomBomb, spawnLocation.position, Quaternion.identity);
                        break;
                    case 1:
                        Instantiate(fuseBomb, spawnLocation.position, Quaternion.identity);
                        break;
                    case 2:
                        Instantiate(tntBomb, spawnLocation.position, Quaternion.identity);
                        break;
                }
                
                spawnedCrateAmount++;
            }
        }

        if (currentObjective >= 3)
        {
            hazardSpawnTimer -= Time.deltaTime;

            if (hazardSpawnTimer <= 0)
            {
                HazardManager.Instance.TutorialSpawnHazards();
                hazardSpawnTimer = maxHazardSpawnTimer;
            }
        }

        if (hitWire && hitOil && currentObjective == 3)
        {
            finalCompleteTextTimer -= Time.deltaTime;

            if (finalCompleteTextTimer <= 0)
            {
                hasCompletedCurrent = true;
            }
        }
    }
}
