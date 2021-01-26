using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Character : MonoBehaviour
{
    [Header("Player General Variables")]
    //public float playerStunnedTime = 3f;
    ///Dash Variable
    [Range(0, 1)]
    public float bottomOfCharacter = 0.25f;
    public float collisionDetectionDistance = 0.2f;
    //public float maxPlayerStunnedTime { get; private set; }

    //public float boxPickUpTime = 1f;
    //public float maxBoxPickUpTime = 0f;

    public CharacterMovement characterMovement { get; private set; }
    //public PlayerDash playerDash { get; private set; }
    //public PlayerPowerups playerPowerups { get; private set; }

    [Header("Player Debug Gizmos and Transforms")]
    public GameObject avatar;

    public PlayerPickup playerPickup;

    public BoxCollider pickupColliderGizmo;

    public Transform carryingPosition;
    //public Transform strengthCarryingPosition;

    [Header("Player Particle Effects")]
    public GameObject sparksParticleEffect;

    public ParticleSystem sparksParticleSystem;
    private float sparksParticleDuration;

    [Header("Player Audio")]
    public string explosionSound;
    public string deliverySound;

    [Header("Player Animator")]
    public Animator anim;

    [Header("Misc?")]
    public GameObject pickupIndicator;

    public GameObject currentPickup { get; private set; } = null;
    public bool isHolding { get; set; } = false;

    private float scoreTimer = 0;
    public int currentScore = 0;
    public string characterName = "";

    public Vector3 currentVelocity = Vector3.zero;

    void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
        HelperUtilities.UpdateCursorLock(true);
        //playerDash = GetComponent<PlayerDash>();
        //playerPowerups = GetComponent<PlayerPowerups>();

        //maxPlayerStunnedTime = playerStunnedTime;
        //maxBoxPickUpTime = boxPickUpTime;
    }

    public enum PlayerState
    {
        Moving,
        Carrying//,
        //Stunned
    }

    public PlayerState playerState = PlayerState.Moving;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pickupColliderGizmo.transform.position, pickupColliderGizmo.size);
    }

    public void BoxPickUp()
    {
        if (isHolding || GameManager.Instance.hasEnded)
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
            if (!pickup.GetComponent<ObjectHP>().canBeHeld)
            {
                return;
            }

            if (LevelManagerHP.Instance.currentHolder != null)
            {
                LevelManagerHP.Instance.currentHolder.GetComponent<Character>().RemoveCurrentPickup();
            }

            pickup.transform.parent = this.gameObject.transform;
            pickup.GetComponent<Rigidbody>().useGravity = false;
            pickup.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            pickup.GetComponent<Collider>().isTrigger = true;

            currentPickup = pickup;
            LevelManagerHP.Instance.currentHolder = this.gameObject;
            currentPickup.transform.position = carryingPosition.position;
            currentPickup.transform.rotation = Quaternion.LookRotation(carryingPosition.transform.right, Vector3.up);
            isHolding = true;
            pickup.GetComponent<ObjectHP>().SetHoldFalse();

            ChangeState(PlayerState.Carrying);
        }
    }

    public void RemoveCurrentPickup()
    {
        
        playerPickup.currentColliders.Remove(currentPickup.GetComponent<Collider>());
        currentPickup.transform.parent = null;
        isHolding = false;
        ChangeState(PlayerState.Moving);
    }

    public void ChangeState(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Moving:
                break;
            case PlayerState.Carrying:
                break;
        }

        playerState = state;
    }

    void Update()
    {
        if (isHolding && !GameManager.Instance.hasEnded)
        {
            scoreTimer -= Time.deltaTime;
            if (scoreTimer <= 0)
            {
                currentScore++;
                scoreTimer = 1;
            }
        }
        else
        {
            scoreTimer = 0;
        }
    }

    void OnPauseToggle(InputValue inputValue)
    {
        if (!GameManager.Instance.hasEnded)
        {
            LevelManager.Instance.PauseToggle();
        }
    }
}
