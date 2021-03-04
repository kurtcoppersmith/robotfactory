using BasicTools.ButtonInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
{
    [Header("Main Manager Variables")]
    public PlayerInputManager inputManager;
    public Character[] characters = new Character[4];
    public Color[] playerColors = new Color[4];
    public GameObject characterAI;

    [Header("External Variables")]
    public float lastHolderDivider = 0;

    ///Private Variables
    private GameObject currentPickup;
    private GameObject currentPickupIcon;
    private Character currentPickupHolder = null;

    private int currentPlayerCharNumb = 0;

    new void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this);
    }

    public int GetCurrentPlayerCharacterNumb()
    {
        return currentPlayerCharNumb;
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i] == null)
            {
                characters[i] = playerInput.gameObject.GetComponentInChildren<Character>();
                characters[i].playerIndex = i;

                currentPlayerCharNumb++;

                break;
            }
        }

        playerInput.gameObject.transform.parent = this.transform;
    }

    public void ReshuffleCharacters(int removalIndex)
    {
        for (int i = 1; i < characters.Length; i++)
        {
            if (characters[i - 1] == null && characters[i] != null)
            {
                characters[i - 1] = characters[i];
                characters[i - 1].playerIndex = i - 1;

                characters[i] = null;
            }
        }

        if (currentPlayerCharNumb - 1 >= 0)
        {
            currentPlayerCharNumb--;
        }
    }

    string RandomNameGenerator()
    {
        string name = "Unit #";

        for (int i = 0; i < 5; i++)
        {
            name += Random.Range(0, 10).ToString();
        }

        return name;
    }

    public void SetPickup(GameObject pickup)
    {
        currentPickup = pickup;
        currentPickupIcon = currentPickup.GetComponent<Pickup>().minimapIcon;
    }

    public GameObject GetCurrentPickup()
    {
        return currentPickup;
    }

    public GameObject GetCurrentPickupIcon()
    {
        return currentPickupIcon;
    }

    public void SetCurrentHolder(Character character)
    {
        currentPickupHolder = character;
    }

    public void RemoveCurrentHolder()
    {
        currentPickupHolder = null;
    }

    public Character GetCurrentHolder()
    {
        return currentPickupHolder;
    }

    public void FillRemainingSpots()
    {
        inputManager.DisableJoining();

        for (int i = currentPlayerCharNumb; i < characters.Length; i++)
        {
            GameObject obj = Instantiate(characterAI);
            characters[i] = obj.GetComponentInChildren<Character>();
            characters[i].playerIndex = i;
            obj.transform.parent = this.transform;
        }

        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].minimapIcon.material.color = playerColors[i];
            characters[i].characterRenderer.material.color = playerColors[i];

            if (characters[i].characterName == "")
            {
                characters[i].characterName = RandomNameGenerator();
            }
        }
    }

    public void SelfDestruct()
    {
        Destroy(this.gameObject);
    }
}
