using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonMonoBehaviour<LevelManager>
{
    [Header("Player Spawn Points")]
    public Transform[] characterSpawnLocations = new Transform[4];

    [Header("AI Waypoints")]
    public List<Transform> AIWaypoints = new List<Transform>();

    [Header("Winner Positions")]
    public Transform[] winnerPositions = new Transform[4];

    [Header("Various Variables")]
    public SplitScreenManager splitScreenManager;
    public GameObject levelPickup;
    public Camera levelCamera;
    public GameObject mainMenuButton;

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

        for (int i = 0; i < PlayerManager.Instance.characters.Length; i++)
        {
            Vector3 currentCharPos = characterSpawnLocations[i].position, tempPickupLocation = levelPickup.transform.position;
            currentCharPos.y = 0;
            tempPickupLocation.y = 0;
            Vector3 lookDir = tempPickupLocation - currentCharPos;
            Quaternion lookRotation = Quaternion.LookRotation(lookDir, Vector3.up);

            PlayerManager.Instance.characters[i].Spawn(characterSpawnLocations[i].position, lookRotation);
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

    private void OnDestroy()
    {
        GameManager.Instance.setTime(GameManager.Instance.maxTime);
        GameManager.Instance.hasEnded = false;
        PlayerManager.Instance.SelfDestruct();
    }
}
