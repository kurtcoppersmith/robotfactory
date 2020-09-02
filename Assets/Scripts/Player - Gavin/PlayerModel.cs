using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerModel : MonoBehaviour
{
    public float maxAttackRadius = 3f;
    public float maxAttackAngle = 30f;
    public float playerStunnedTime = 3f;
    private float maxPlayerStunnedTime;

    public PlayerMovement playerMovement { get; private set; }
    public QTEManager qteManager { get; private set; }

    public PlayerPickup playerPickup;

    public BoxCollider pickupColliderGizmo;

    public Transform carryingPosition;

    public GameObject currentPickup { get; private set; } = null;
    
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
        Carrying,
        Stunned
    }

    public PlayerState playerState = PlayerState.Moving;

    void OnDrawGizmosSelected()
    {
        DebugExtension.DebugBounds(pickupColliderGizmo.bounds, Color.red);
    }

    void OnBoxPickup(InputValue inputValue)
    {
        if (currentPickup != null)
        {
            return;
        }
        GameObject pickup = null;

        for (int i = 0; i < playerPickup.currentColliders.Count; i++)
        {
            if (playerPickup.currentColliders[i].gameObject.tag == "Pickup")
            {
                pickup = playerPickup.currentColliders[i].gameObject;
                break;
            }
        }

        if (pickup == null)
        {
            return;
        }
        else
        {
            pickup.transform.parent = this.gameObject.transform;
            currentPickup = pickup;
            pickup.transform.DOMove(carryingPosition.position, qteManager.initialQTEBuffer / 2);
            
            if (playerState == PlayerState.Moving)
            {
                ChangeState(PlayerState.Carrying);
            }
        }
    }

    public void RemoveCurrentPickup()
    {
        playerPickup.currentColliders.Remove(currentPickup.GetComponent<Collider>());

        currentPickup.SetActive(false);
        currentPickup = null;
    }

    public void ChangeState(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Moving:
                playerMovement.canMove = true;
                qteManager.enabled = false;
                break;
            case PlayerState.Carrying:
                qteManager.enabled = true;
                playerMovement.canMove = false;
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
