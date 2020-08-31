using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerModel : MonoBehaviour
{
    public float maxAttackRadius = 3f;
    public float maxAttackAngle = 30f;
    public float playerStunnedTime = 3f;
    private float maxPlayerStunnedTime;

    public PlayerMovement playerMovement { get; private set; }
    public QTEManager qteManager { get; private set; }

    public Canvas QTECanvas;
    
    /*new void Awake()
    {
        base.Awake();
        playerMovement = GetComponent<PlayerMovement>();

        qteManager = GetComponent<QTEManager>();
        qteManager.enabled = false;
    }*/

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();

        qteManager = GetComponent<QTEManager>();
        qteManager.enabled = false;

        maxPlayerStunnedTime = playerStunnedTime;
    }

    public enum PlayerState
    {
        Moving,
        Attacking,
        Stunned
    }

    void Start()
    {
        QTECanvas.worldCamera = GameManager.Instance.mainCam;
    }

    public PlayerState playerState = PlayerState.Moving;

    void OnDrawGizmos()
    {
        DebugExtension.DebugWireSphere(transform.position, Color.red, maxAttackRadius);
    }

    void OnBoxPickup(InputValue inputValue)
    {
        if (playerState == PlayerState.Moving)
        {
            ChangeState(PlayerState.Attacking);
        }
    }

    public void ChangeState(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Moving:
                playerMovement.canMove = true;
                qteManager.enabled = false;
                break;
            case PlayerState.Attacking:
                qteManager.enabled = true;
                playerMovement.canMove = true;
                break;
            case PlayerState.Stunned:
                playerMovement.canMove = false;
                qteManager.enabled = false;
                break;
        }

        playerState = state;
    }

    void Update()
    {
        if (playerState == PlayerState.Stunned)
        {
            Stunned();
        }
    }

    void Stunned()
    {
        playerStunnedTime -= Time.deltaTime;

        if (playerStunnedTime <= 0)
        {
            ChangeState(PlayerState.Moving);
            playerStunnedTime = maxPlayerStunnedTime;
        }
    }

    void OnPauseToggle(InputValue inputValue)
    {
        print("Pause");

        if (playerState == PlayerState.Moving)
        {
            if (Time.timeScale == 0.0f)
            {
                Time.timeScale = 1.0f;
                HelperUtilities.UpdateCursorLock(false);
            }
            else
            {
                Time.timeScale = 0.0f;
                HelperUtilities.UpdateCursorLock(true);
            }
        }
    }
}
