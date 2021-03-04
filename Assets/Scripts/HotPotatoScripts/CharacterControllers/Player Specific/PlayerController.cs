using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Character
{
    [HideInInspector] public PlayerUI playerUI;
    [HideInInspector] public PlayerMovement playerMovement;
    [HideInInspector] public PlayerGroundDetection groundDetection;

    private void Awake()
    {
        playerUI = GetComponent<PlayerUI>();
        playerMovement = GetComponent<PlayerMovement>();
        groundDetection = GetComponent<PlayerGroundDetection>();

        maxAttackAbilityRecharge = attackAbilityRecharge;
        maxUpdateScoreTimer = updateScoreTimer;
    }

    public override void EnableObj()
    {
        base.EnableObj();

        playerUI.UpdateUIElements(playerIndex, characterName);
        playerUI.AddOtherMinimapIcons(playerIndex);
        playerMovement.SetCharacterMovementVariables();

        characterHand.SetPlayerIndex(playerIndex);

        playerMovement.canMove = true;
    }

    public override void Spawn(Vector3 spawnLocation, Quaternion lookRotation)
    {
        transform.position = spawnLocation;
        //Fix this problem on startup.
        avatar.transform.rotation = lookRotation;
    }

    public override void Move()
    {
        base.Move();

        playerMovement.Move();
    }

    public override void Holding()
    {
        base.Holding();

        updateScoreTimer -= Time.deltaTime;

        if (updateScoreTimer <= 0)
        {
            characterScore++;
            playerUI.UpdatePlayerScore(characterScore);
            updateScoreTimer = maxUpdateScoreTimer;
        }
    }

    void OnAttack(InputValue inputValue)
    {
        if (!isEnabled || currentPickup != null)
        {
            return;
        }

        if (canAttack)
        {
            Attack();

            canAttack = false;
            playerUI.punchAbilityImage.gameObject.SetActive(true);
            playerUI.punchAbilityImage.fillAmount = 0;
        }
    }

    void UpdateAttack()
    {
        if (!canAttack)
        {
            attackAbilityRecharge -= Time.deltaTime;
            playerUI.punchAbilityImage.fillAmount = 
                HelperUtilities.Remap(maxAttackAbilityRecharge - attackAbilityRecharge, 0, maxAttackAbilityRecharge, 0, 1);

            if (attackAbilityRecharge <= 0)
            {
                playerUI.punchAbilityImage.fillAmount = 1;

                attackAbilityRecharge = maxAttackAbilityRecharge;
                canAttack = true;
                playerUI.punchAbilityImage.gameObject.SetActive(false);
            }
        }
    }

    public override void Attack()
    {
        base.Attack();

        if (characterHand.currentCharactersInRange.Count > 0)
        {
            foreach (int characterIndex in characterHand.currentCharactersInRange)
            {
                PlayerManager.Instance.characters[characterIndex].OnHit(transform.position);
            }
        }
    }

    public override void OnHit(Vector3 attackerPosition)
    {
        base.OnHit(attackerPosition);

        playerMovement.Knockback(attackerPosition);
    }

    public override void Drop()
    {
        base.Drop();
    }

    void OnInteract(InputValue inputValue)
    {
        
    }

    void OnCatchPickUp()
    {
        if (!isEnabled)
        {
            return;
        }


        if (catchCollider.currentPickupColliders.Count > 0 && currentPickup == null)
        {
            if (catchCollider.currentPickupColliders.Count > 1)
            {
                int randNumb = Random.Range(0, catchCollider.currentPickupColliders.Count);

                if (catchCollider.currentPickupColliders[randNumb].gameObject.GetComponent<Pickup>().hasLanded &&
                    catchCollider.currentPickupColliders[randNumb].gameObject.GetComponent<Pickup>().CanBePickedUp())
                {
                    if (PlayerManager.Instance.GetCurrentHolder() != null)
                    {
                        PlayerManager.Instance.GetCurrentHolder().Drop();
                    }

                    OnPickup(catchCollider.currentPickupColliders[randNumb].gameObject);
                }

                return;
            }
            else
            {
                if (catchCollider.currentPickupColliders[0].gameObject.GetComponent<Pickup>().hasLanded &&
                    catchCollider.currentPickupColliders[0].gameObject.GetComponent<Pickup>().CanBePickedUp())
                {
                    if (PlayerManager.Instance.GetCurrentHolder() != null)
                    {
                        PlayerManager.Instance.GetCurrentHolder().Drop();
                    }
                    
                    OnPickup(catchCollider.currentPickupColliders[0].gameObject);
                }
            }
        }
        else if (catchCollider.currentCharactersInRange.Count > 0 && currentPickup == null)
        {
            bool currentPickupInRange = false;
            for (int i = 0; i < catchCollider.currentCharactersInRange.Count; i++)
            {
                if (PlayerManager.Instance.characters[catchCollider.currentCharactersInRange[i]].currentPickup != null)
                {
                    currentPickupInRange = true;
                    break;
                }
            }

            if (currentPickupInRange && PlayerManager.Instance.GetCurrentPickup().GetComponent<Pickup>().CanBePickedUp())
            {
                GameObject otherCharacterPickup = PlayerManager.Instance.GetCurrentHolder().currentPickup;
                PlayerManager.Instance.GetCurrentHolder().Drop();

                OnPickup(otherCharacterPickup);
            }
        }
    }

    public override void OnPickup(GameObject pickup)
    {
        currentPickup = pickup;
        pickup.GetComponent<Pickup>().SetPickedUp(true);
        pickup.transform.position = pickupTransform.position;
        pickup.transform.parent = avatar.transform;
        PlayerManager.Instance.SetCurrentHolder(this);

        updateScoreTimer = 0;
    }

    public override void DisableObj()
    {
        base.DisableObj();

        playerCam.enabled = false;

        Debug.Log("Disabled");
    }

    private void Update()
    {
        if (isEnabled)
        {
            if (GameManager.Instance.hasEnded)
            {
                if (!isUnwrapped)
                {
                    DisableObj();
                }

                return;
            }

            Move();
            UpdateAttack();
            OnCatchPickUp();

            if (currentPickup != null)
            {
                Holding();
            }
        }
    }
}
