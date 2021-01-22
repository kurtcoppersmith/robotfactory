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

    void OnBoxPickup(InputValue inputValue)
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
            currentPickup.transform.position = carryingPosition.position;
            pickup.transform.parent = this.gameObject.transform;
            pickup.GetComponent<IdleCrate>().PickUp(true);
            pickup.GetComponent<Rigidbody>().useGravity = false;
            pickup.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            pickup.GetComponent<Crate>().powerupIndicator.SetActive(false);
            anim.SetBool("grab", true);

            currentPickup = pickup;
            isHolding = true;

            if (playerState == PlayerState.Moving)
            {
                ChangeState(PlayerState.Carrying);
            }
        }
    }

    public void RemoveCurrentPickup()
    {
        //CrateManager.Instance.Explode();
        //CrateManager.Instance.spawnLocationStatus[CrateManager.Instance.currentSpawnedItems[currentPickup]] = false;
        //CrateManager.Instance.currentSpawnedItems.Remove(currentPickup);
        playerPickup.currentColliders.Remove(currentPickup.GetComponent<Collider>());

        //currentPickup.GetComponent<IdleCrate>().PickUp(false);
        //currentPickup.SetActive(false);
        currentPickup.transform.parent = null;//ObjectPoolerGavin.GetPooler(ObjectPoolerGavin.Key.AtomBomb).gameObject.transform;
        isHolding = false;
    }

    void DetectCollisions()
    {
        RaycastHit hit;
        Vector3 p1 = transform.position + Vector3.up * bottomOfCharacter;
        Vector3 p2 = p1 + Vector3.up * characterMovement.charController.height;

        for (int i = 0; i < 360; i += 18)
        {
            if (Physics.CapsuleCast(p1, p2, 0, new Vector3(Mathf.Cos(i), 0, Mathf.Sin(i)), out hit, characterMovement.charController.radius + collisionDetectionDistance))
            {
                PlayerModel tempPlay = null;
                tempPlay = hit.collider.gameObject.GetComponentInParent<PlayerModel>();
                if (tempPlay == null)
                {
                    Vector3 temp = (hit.point - transform.position).normalized;
                    temp.y = 0;
                    characterMovement.charController.Move((temp) * (collisionDetectionDistance - hit.distance));

                    //if (hit.collider.gameObject.tag == "Hazard" && isHolding)
                    //{
                    //    if (!playerPowerups.strengthPower)
                    //    {
                    //        Fail();
                    //    }
                    //}
                }

            }
            DebugExtension.DebugCapsule(p1 + new Vector3(Mathf.Cos(i), 0, Mathf.Sin(i)), p2 + new Vector3(Mathf.Cos(i), 0, Mathf.Sin(i)), 0);
        }
    }

    void ShowPickUpIndicator()
    {
        if (isHolding || GameManager.Instance.hasEnded || (TutorialManager.Instance != null))
        {
            pickupIndicator.SetActive(false);
            return;
        }
        bool pickup = false;

        for (int i = 0; i < playerPickup.currentColliders.Count; i++)
        {
            if (playerPickup.currentColliders[i].gameObject.tag == "Pickup")
            {
                pickup = true;
                pickupIndicator.SetActive(true);
                break;
            }
        }

        if (!pickup)
        {
            pickupIndicator.SetActive(false);
        }
    }

    public void ChangeState(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Moving:
                characterMovement.canMove = true;
                break;
            case PlayerState.Carrying:
                characterMovement.canMove = false;
                break;
        }

        playerState = state;
    }

    void Update()
    {
        //DetectCollisions();
        ShowPickUpIndicator();
    }

    void OnPauseToggle(InputValue inputValue)
    {
        if (!GameManager.Instance.hasEnded)
        {
            LevelManager.Instance.PauseToggle();
        }
    }
}
