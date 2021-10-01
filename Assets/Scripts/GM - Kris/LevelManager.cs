using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnvironmentInteract
{
    public EnvironmentInteractable currentInteractable;
    public Transform interactableWaypoint;
}

[System.Serializable]
public class SpawnGroup
{
    public Transform[] characterSpawnLocations = new Transform[4];
    public Transform pickupStartLocation;
}

public class LevelManager : SingletonMonoBehaviour<LevelManager>
{
    public bool isLevelActive = false;

    [Header("Player Spawn Points")]
    public List<SpawnGroup> spawnGroups = new List<SpawnGroup>();

    [Header("AI Waypoints")]
    public List<Transform> AIWaypoints = new List<Transform>();

    [Header("Environment Interactables and Waypoints")]
    public List<EnvironmentInteract> levelInteractableWaypoints = new List<EnvironmentInteract>();

    [Header("Interactable Walls")]
    public List<InteractableWall> interactableWalls = new List<InteractableWall>();

    [Header("Winner Positions")]
    public Transform[] winnerPositions = new Transform[4];

    [Header("Various Variables")]
    public SplitScreenManager splitScreenManager;
    public GameObject levelPickup;
    public Camera levelCamera;
    public GameObject mainMenuButton;
    [HideInInspector] public int currentSpawnGroup = -1;

    private bool levelEnded = false;
    private float lastHolderDivider = 0;

    private void Start()
    {
        SetUpLevel();
    }

    void SetUpLevel()
    {
        PlayerManager.Instance.SetPickup(levelPickup);
        lastHolderDivider = PlayerManager.Instance.lastHolderDivider;

        int currentPlayersAdded = PlayerManager.Instance.GetCurrentPlayerCharacterNumb();
        for (int i = 0; i < currentPlayersAdded; i++)
        {
            PlayerManager.Instance.characters[i].playerCam.rect = splitScreenManager.wrappedRects[currentPlayersAdded - 1].viewportRects[i];
        }

        int randLocaleNumb = Random.Range(0, spawnGroups.Count);
        currentSpawnGroup = randLocaleNumb;
        levelPickup.transform.position = spawnGroups[randLocaleNumb].pickupStartLocation.position;

        for (int i = 0; i < PlayerManager.Instance.characters.Length; i++)
        {
            Vector3 currentCharPos = spawnGroups[randLocaleNumb].characterSpawnLocations[i].position, 
                    tempPickupLocation = levelPickup.transform.position;
            currentCharPos.y = 0;
            tempPickupLocation.y = 0;
            Vector3 lookDir = tempPickupLocation - currentCharPos;
            Quaternion lookRotation = Quaternion.LookRotation(lookDir, Vector3.up);

            PlayerManager.Instance.characters[i].characterController.enabled = false;
            PlayerManager.Instance.characters[i].Spawn(spawnGroups[randLocaleNumb].characterSpawnLocations[i].position, lookRotation);
            PlayerManager.Instance.characters[i].characterController.enabled = true;
            PlayerManager.Instance.characters[i].EnableObj();
        }
    }

    void ApplyLastHolderBonus()
    {
        int currentMax = PlayerManager.Instance.characters[0].characterScore;
        for (int i = 1; i < PlayerManager.Instance.characters.Length; i++)
        {
            if (currentMax < PlayerManager.Instance.characters[i].characterScore)
            {
                currentMax = PlayerManager.Instance.characters[i].characterScore;
            }
        }

        if (currentMax != -1 && PlayerManager.Instance.GetCurrentHolder() != null)
        {
            PlayerManager.Instance.GetCurrentHolder().characterScore += Mathf.RoundToInt(currentMax / lastHolderDivider);
            Debug.Log("Last holder was: " + PlayerManager.Instance.GetCurrentHolder().characterName);
        }
    }

    void FindWinner()
    {
        List<Character> tempCharacters = new List<Character>();

        for (int i = 0; i < PlayerManager.Instance.characters.Length; i++)
        {
            tempCharacters.Add(PlayerManager.Instance.characters[i]);
        }

        for (int i = 0; i < PlayerManager.Instance.characters.Length; i++)
        {
            int currentMax = tempCharacters[0].characterScore;
            Character winner = tempCharacters[0];
            for (int j = 1; j < tempCharacters.Count; j++)
            {
                if (currentMax < tempCharacters[j].characterScore)
                {
                    winner = tempCharacters[j];
                    currentMax = tempCharacters[j].characterScore;
                }
            }

            Debug.Log(winner.characterName + " is in " + i + " place!");

            winner.gameObject.transform.position = winnerPositions[i].position;
            Vector3 tempWinnerPos = winner.gameObject.transform.position, tempCameraPos = levelCamera.transform.position;
            tempWinnerPos.y = 0;
            tempCameraPos.y = 0;
            Vector3 lookDir = tempCameraPos - tempWinnerPos;
            winner.avatar.transform.rotation = Quaternion.LookRotation(lookDir, Vector3.up);
            winner.winCanvas.transform.rotation = Quaternion.LookRotation(-winnerPositions[i].forward, Vector3.up);
            winner.EnableWinCanvas();

            tempCharacters.Remove(winner);
        }
        
        //return winner;
    }

    public void CheckPlayerAudioRange(Vector3 position, string audioName)
    {
        Vector3 tempAudPos = position;
        tempAudPos.y = 0;

        int currentPlayersAdded = PlayerManager.Instance.GetCurrentPlayerCharacterNumb();
        for (int i = 0; i < currentPlayersAdded; i++)
        {
            Vector3 tempChar = PlayerManager.Instance.characters[i].gameObject.transform.position;
            tempChar.y = 0;

            if (Vector3.Distance(tempAudPos, tempChar) < 11)
            {
                SoundEffectsManager.Instance.Play(audioName);
                return;
            }
        }
    }

    private void Update()
    {
        if (!GameManager.Instance.hasEnded)
        {
            GameManager.Instance.subtractTime();
        }
        else
        {
            if (!levelEnded)
            {
                ApplyLastHolderBonus();
                FindWinner();

                mainMenuButton.SetActive(true);
                levelEnded = true;
            }
        }
    }

    public void DestroyPlayerManager()
    {
        PlayerManager.Instance.SelfDestruct();
    }

    private void OnDestroy()
    {
        GameManager.Instance.setTime(GameManager.Instance.maxTime);
        GameManager.Instance.hasEnded = false;
    }
}
