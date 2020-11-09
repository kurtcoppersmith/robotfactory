using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerModel : MonoBehaviour
{
    [Header("Player General Variables")]
    public float playerStunnedTime = 3f;
    [Range(0,1)]
    public float bottomOfCharacter = 0.25f;
    public float collisionDetectionDistance = 0.2f;
    public float maxPlayerStunnedTime { get; private set; }

    public float boxPickUpTime = 1f;
    public float maxBoxPickUpTime = 0f;

    public PlayerMovement playerMovement { get; private set; }
    public PlayerDash playerDash { get; private set; }
    public PlayerPowerups playerPowerups { get; private set; }

    [Header("Player Debug Gizmos and Transforms")]
    public GameObject avatar;

    public PlayerPickup playerPickup;

    public BoxCollider pickupColliderGizmo;

    public Transform carryingPosition;
    public Transform strengthCarryingPosition;

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
        playerDash = GetComponent<PlayerDash>();
        playerPowerups = GetComponent<PlayerPowerups>();

        maxPlayerStunnedTime = playerStunnedTime;
        maxBoxPickUpTime = boxPickUpTime;
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pickupColliderGizmo.transform.position, pickupColliderGizmo.size);
    }

    void OnBoxPickup(InputValue inputValue)
    {
        if (isHolding || GameManager.Instance.hasEnded || (TutorialManager.Instance != null && !TutorialManager.Instance.hasDescription))
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
            

            boxPickUpTime = maxBoxPickUpTime;

            pickup.transform.parent = this.gameObject.transform;
            pickup.GetComponent<IdleCrate>().PickUp(true);
            pickup.GetComponent<Rigidbody>().useGravity = false;
            pickup.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            pickup.GetComponent<Crate>().powerupIndicator.SetActive(false);
            anim.SetBool("grab", true);

            if (TutorialManager.Instance != null && TutorialManager.Instance.currentObjective == 0)
            {
                TutorialManager.Instance.hasCompletedCurrent = true;
                pickup.GetComponent<Crate>().SetTutorialTimer(15);
            }

            currentPickup = pickup;
            isHolding = true;

            if (playerPowerups.strengthPower)
            {
                pickup.transform.DOMove(strengthCarryingPosition.position, maxBoxPickUpTime);
            }
            else
            {
                pickup.transform.DOMove(carryingPosition.position, maxBoxPickUpTime);
            }
            
            if (playerState == PlayerState.Moving)
            {
                ChangeState(PlayerState.Carrying);
            }
        }
    }

    public void RemoveCurrentPickup()
    {
        CrateManager.Instance.Explode();
        CrateManager.Instance.spawnLocationStatus[CrateManager.Instance.currentSpawnedItems[currentPickup]] = false;
        CrateManager.Instance.currentSpawnedItems.Remove(currentPickup);
        playerPickup.currentColliders.Remove(currentPickup.GetComponent<Collider>());

        currentPickup.GetComponent<IdleCrate>().PickUp(false);
        currentPickup.SetActive(false);
        currentPickup.transform.parent = ObjectPoolerGavin.GetPooler(ObjectPoolerGavin.Key.AtomBomb).gameObject.transform;
        isHolding = false;
    }

    void DetectCollisions()
    {
        RaycastHit hit;
        Vector3 p1 = transform.position + Vector3.up * bottomOfCharacter;
        Vector3 p2 = p1 + Vector3.up * playerMovement.charController.height;

        for (int i = 0; i < 360; i += 18)
        {
            if (Physics.CapsuleCast(p1, p2, 0, new Vector3(Mathf.Cos(i), 0, Mathf.Sin(i)), out hit, playerMovement.charController.radius + collisionDetectionDistance))
            {
                PlayerModel tempPlay = null;
                tempPlay = hit.collider.gameObject.GetComponentInParent<PlayerModel>();
                if (tempPlay == null)
                {
                    Vector3 temp = (hit.point - transform.position).normalized;
                    temp.y = 0;
                    playerMovement.charController.Move((temp) * (collisionDetectionDistance - hit.distance));

                    if (hit.collider.gameObject.tag == "Hazard" && isHolding)
                    {
                        if (!playerPowerups.strengthPower)
                        {
                            Fail();
                        }
                    }
                }

            }
            DebugExtension.DebugCapsule(p1 + new Vector3(Mathf.Cos(i), 0, Mathf.Sin(i)), p2 + new Vector3(Mathf.Cos(i), 0, Mathf.Sin(i)), 0);
        }
    }

    public void TutorialPassed()
    {
        if (playerState != PlayerState.Carrying)
        {
            return;
        }

        isHolding = false;
        ChangeState(PlayerState.Moving);
        playerPickup.currentColliders.Remove(currentPickup.GetComponent<Collider>());
        SoundEffectsManager.Instance.Play(deliverySound);
        Destroy(currentPickup);

        TutorialManager.Instance.spawnedCrateAmount--;
        if (TutorialManager.Instance.spawnedCrateAmount < 0)
        {
            TutorialManager.Instance.spawnedCrateAmount = 0;
        }
    }

    //If we still need QTE, make sure to add the qte fails into this function.
    public void Passed()
    {
        if (!GameManager.Instance.hasEnded)
        {
            if (playerState != PlayerState.Carrying)
            {
                return;
            }

            GameManager.Instance.addScore(5);
            SoundEffectsManager.Instance.Play(deliverySound);
            anim.SetBool("grab", false);

            if (playerPowerups.strengthPower)
            {
                playerPowerups.SetStrengthPowerup(false);
            }

            if(currentPickup.GetComponent<Crate>().power != Crate.PowerUp.None)
            {
                switch (currentPickup.GetComponent<Crate>().power)
                {
                    case Crate.PowerUp.Strength:
                        playerPowerups.SetStrengthPowerup(true);
                        Debug.Log("Player Strength");
                        break;
                    case Crate.PowerUp.Speed:
                        playerPowerups.SetSpeedPowerup(true);
                        Debug.Log("Player Speed");
                        break;
                    case Crate.PowerUp.Chasis:
                        Debug.Log("Player Chasis");
                        playerPowerups.SetChasisPowerup(true);
                        break;
                }
            }

            RemoveCurrentPickup();
            ChangeState(PlayerState.Moving);
        }
    }

    public void TutorialFail()
    {
        isHolding = false;
        ChangeState(PlayerState.Stunned);
        playerPickup.currentColliders.Remove(currentPickup.GetComponent<Collider>());
        SoundEffectsManager.Instance.Play(explosionSound);
        Destroy(currentPickup);

        TutorialManager.Instance.spawnedCrateAmount--;
        if (TutorialManager.Instance.spawnedCrateAmount < 0)
        {
            TutorialManager.Instance.spawnedCrateAmount = 0;
        }
    }

    //If we still need QTE, make sure to add the qte fails into this function.
    public void Fail()
    {
        if (!GameManager.Instance.hasEnded)
        {
            GameManager.Instance.subScore(2);

            if (playerPowerups.strengthPower)
            {
                playerPowerups.SetStrengthPowerup(false);
            }

            SoundEffectsManager.Instance.Play(explosionSound);
            RemoveCurrentPickup();
            ChangeState(PlayerState.Stunned);
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
                playerMovement.canMove = true;
                break;
            case PlayerState.Carrying:
                playerMovement.canMove = false;
                break;
            case PlayerState.Stunned:
                playerMovement.canMove = false;

                sparksParticleEffect.SetActive(true);
                if (maxPlayerStunnedTime < sparksParticleSystem.main.duration)
                {
                    sparksParticleDuration = maxPlayerStunnedTime;
                }
                else
                {
                    sparksParticleDuration = sparksParticleSystem.main.duration;
                }

                anim.SetBool("grab", false);
                anim.SetBool("shutdown", true);

                //transform.DOPunchPosition(new Vector3(Random.Range(0.1f, 0.2f), 0, Random.Range(0.1f, 0.2f)), maxPlayerStunnedTime / 2);
                break;
        }

        playerState = state;
    }

    void Update()
    {
        if (!playerMovement.canMove && playerState != PlayerState.Stunned)
        {
            boxPickUpTime -= Time.deltaTime;

            if (boxPickUpTime <= 0)
            {
                playerMovement.canMove = true;

                if (playerPowerups.strengthPower)
                {
                    currentPickup.transform.position = strengthCarryingPosition.position;
                }
                else
                {
                    currentPickup.transform.position = carryingPosition.position;
                }

                boxPickUpTime = maxBoxPickUpTime;
            }
        }

        if (playerState == PlayerState.Stunned)
        {
            Stunned();
        }

        DetectCollisions();
        ShowPickUpIndicator();
    }

    void Stunned()
    {
        playerStunnedTime -= Time.deltaTime;
        sparksParticleDuration -= Time.deltaTime;

        if (sparksParticleDuration <= 0 && sparksParticleEffect.activeInHierarchy)
        {
            sparksParticleEffect.SetActive(false);
        }

        if (playerStunnedTime <= 0)
        {
            ChangeState(PlayerState.Moving);
            playerStunnedTime = maxPlayerStunnedTime;

            anim.SetBool("shutdown", false);
        }
    }

    void OnPauseToggle(InputValue inputValue)
    {
        if (!GameManager.Instance.hasEnded)
        {
            LevelManager.Instance.PauseToggle();
        }
    }

    void OnControllerColliderHit(ControllerColliderHit other)
    {
        if (other.gameObject.tag == "Hazard" && isHolding)
        {
            if (!playerPowerups.strengthPower)
            {
                Fail();
            }
        }
    }
}
