using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonMonoBehaviour<LevelManager>
{
    [Header("Player Spawn Points")]
    public Transform[] characterSpawnLocations = new Transform[4];

    [Header("AI Waypoints")]
    public List<Transform> AIWaypoints = new List<Transform>();

    [Header("Various Variables")]
    public SplitScreenManager splitScreenManager;
    public GameObject levelPickup;

    private void Start()
    {
        PlayerManager.Instance.SetPickup(levelPickup);

        int currentPlayersAdded = PlayerManager.Instance.GetCurrentPlayerCharacterNumb();
        for (int i = 0; i < currentPlayersAdded; i++)
        {
            PlayerManager.Instance.characters[i].playerCam.rect = splitScreenManager.wrappedRects[currentPlayersAdded - 1].viewportRects[i];
        }

        for (int i = 0; i < PlayerManager.Instance.characters.Length; i++)
        {
            PlayerManager.Instance.characters[i].Spawn(characterSpawnLocations[i].position);
            PlayerManager.Instance.characters[i].EnableObj();
        }
    }
}
