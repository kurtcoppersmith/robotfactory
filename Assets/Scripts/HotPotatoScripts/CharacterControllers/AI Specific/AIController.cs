using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : Character
{
    public enum MainState
    {
        Chasing,
        Carrying
    }
    public enum SubState
    {
        Chase,
        Dash,
        Attack,
        Stand,
        Flee,
        Knockback,
        RandomLocale
    }

    [Header("AI Specific Variables")]
    public MainState mainState = MainState.Chasing;
    public SubState subState = SubState.Chase;

    private NavMeshAgent nav;
    private CharacterController characterController;

    public RangeFloat maxChaseSubStateTimer = new RangeFloat(0f, 0f);
    private float chaseSubStateTimer = 0f;

    public RangeFloat maxChasePredictionTimer = new RangeFloat(0f, 0f);
    private float chasePredictionTimer = 0f;
    private bool shouldChasePredict = false;

    public RangeFloat maxCarrySubStateTimer = new RangeFloat(0f, 0f);
    private float carrySubStateTimer = 0f;

    public RangeFloat maxStandSubStateTimer = new RangeFloat(0f, 0f);
    private float standSubStateTimer = 0f;

    [Header("AI Percentiles")]
    [Range(0, 99)]
    public float dashPercent = 0f;
    [Range(0, 99)]
    public float attackPercent = 0f;
    [Range(0, 99)]
    public float standPercent = 0f;
    [Range(0, 99)]
    public float environmentPercent = 0f;
    [Range(0, 99)]
    public float randomLocalePercent = 0f;
    [Range(0,99)]
    public float velocityPredictionPercent = 0f;
    [Range(0f, 3f)]
    public float velocityPredictionRange = 0f;
    [Range(0f, 10f)]
    public float maxFleeRange = 0f;
    [Range(0f, 360f)]
    public float angleOfFleeDistance = 0f;
    [Range(0f, 360f)]
    public float angleOfChaseDistance = 0f;

    private Vector3 dashingDestination = Vector3.zero;
    private Vector3 previousPickupLocation = Vector3.zero;
    private Vector3 attackerPos = Vector3.zero;

    private float normalSpeed = 0f;
    private bool isStanding = false;

    private List<Transform> waypoints = new List<Transform>();
    private Transform currentFleeWaypoint = null;
    private Transform currentChaseWaypoint;

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        characterController = GetComponent<CharacterController>();
        characterController.enabled = false;

        normalSpeed = nav.speed;

        maxDashAbilityRecharge = dashAbilityRecharge;

        maxDashTime = dashingDistance / dashingSpeed;
        dashTime = maxDashTime;

        maxKnockbackTime = knockbackDistance / knockbackSpeed;
        knockbackTime = maxKnockbackTime;

        maxDazedTime = dazedTime;

        standSubStateTimer = maxStandSubStateTimer.GetRandom();
        chaseSubStateTimer = maxChaseSubStateTimer.GetRandom();
        chasePredictionTimer = maxChasePredictionTimer.GetRandom();

        maxUpdateScoreTimer = updateScoreTimer;
    }

    public override void EnableObj()
    {
        nav.enabled = true;
        characterController.enabled = false;

        waypoints = new List<Transform>(LevelManager.Instance.AIWaypoints);
        base.EnableObj();

    }

    public override void Spawn(Vector3 spawnLocation)
    {
        transform.position = spawnLocation;
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

        ChangeSubState(SubState.Chase);
    }

    public override void OnHit(Vector3 attackerPosition)
    {
        base.OnHit(attackerPosition);

        attackerPos = attackerPosition;

        if (currentPickup == null && mainState == MainState.Carrying)
        {
            mainState = MainState.Chasing;
        }

        ChangeSubState(SubState.Knockback);
    }

    public override void Holding()
    {
        base.Holding();

        updateScoreTimer -= Time.deltaTime;

        if (updateScoreTimer <= 0)
        {
            characterScore++;
            updateScoreTimer = maxUpdateScoreTimer;
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
                    ChangeMainState(MainState.Carrying);
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
                    ChangeMainState(MainState.Carrying);
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
                PlayerManager.Instance.GetCurrentHolder().Drop();

                OnPickup(PlayerManager.Instance.GetCurrentPickup());
                ChangeMainState(MainState.Carrying);
            }
        }
    }

    bool UpdatePreviousLocation()
    {
        Vector3 tempPrevious = previousPickupLocation, tempCurrent = PlayerManager.Instance.GetCurrentPickup().transform.position;
        tempPrevious.y = 0;
        tempCurrent.y = 0;

        if (Vector3.Distance(tempPrevious, tempCurrent) > 0.5f)
        {
            previousPickupLocation = tempCurrent;
            return true;
        }

        return false;
    }

    void FindNewChaseSubState()
    {
        ///If pickup is on the ground, rush to pick it up as fast as possible.
        if (!PlayerManager.Instance.GetCurrentPickup().GetComponent<Pickup>().IsPickedUp())
        {
            if (canDash)
            {
                int randChaseNumb = Random.Range(0, 100);
                if (randChaseNumb < dashPercent)
                {
                    ChangeSubState(SubState.Dash);
                    return;
                }
                else
                {
                    ChangeSubState(SubState.Chase);
                    return;
                }
            }
            else
            {
                ChangeSubState(SubState.Chase);
                return;
            }
        }

        ///Attack if the player is in front of the A.I.
        if (characterHand.currentCharactersInRange.Count > 0)
        {
            if (!canAttack)
            {
                ChangeSubState(SubState.Chase);
                return;
            }

            bool isPickupInRange = false;
            for (int i = 0; i < characterHand.currentCharactersInRange.Count; i++)
            {
                if(PlayerManager.Instance.characters[characterHand.currentCharactersInRange[i]].currentPickup != null)
                {
                    isPickupInRange = true;
                    break;
                }
            }

            if (isPickupInRange)
            {
                if (isStanding)
                {
                    isStanding = false;
                }

                ChangeSubState(SubState.Attack);
                return;
            }
            else
            {
                int randAttackNumb = Random.Range(0, 100);
                if (randAttackNumb < attackPercent)
                {
                    ChangeSubState(SubState.Attack);
                    return;
                }
            }
        }

        ///Dash, chase, use environment interactable, or stand around.

        int randDefaultNumb = Random.Range(0, 100);
        if (!canDash)
        {
            if (randDefaultNumb < standPercent)
            {
                ChangeSubState(SubState.Stand);
                return;
            }
            else
            {
                if (randDefaultNumb < randomLocalePercent)
                {
                    ChangeSubState(SubState.RandomLocale);
                    return;
                }
                else
                {
                    ChangeSubState(SubState.Chase);
                    return;
                }
            }
        }
        else
        {
            if (randDefaultNumb < dashPercent)
            {
                ChangeSubState(SubState.Dash);
                return;
            }
            else
            {
                ChangeSubState(SubState.Chase);
                return;
            }
        }
    }

    void FindNewCarrySubState()
    {
        int randDefaultNumb = Random.Range(0, 100);

        if (randDefaultNumb < dashPercent && canDash)
        {
            ChangeSubState(SubState.Dash);
            return;
        }
        else
        {
            ChangeSubState(SubState.Flee);
            return;
        }
    }

    void EnableNav()
    {
        if (!nav.enabled)
        {
            characterController.enabled = false;
            
            nav.enabled = true;
            nav.isStopped = false;
        }
    }

    void EnableCharacter()
    {
        if (!characterController.enabled)
        {
            nav.isStopped = true;
            nav.enabled = false;

            characterController.enabled = true;
        }
    }

    void Chase()
    {
        if (PlayerManager.Instance.GetCurrentHolder() == null)
        {
            EnableNav();
            nav.SetDestination(previousPickupLocation);
        }
        else
        {
            Vector3 tempPickup = previousPickupLocation, tempVelocity = PlayerManager.Instance.GetCurrentHolder().currentVelocity;
            tempPickup.y = 0;
            tempVelocity.y = 0;

            if (shouldChasePredict)
            {
                Vector3 navDir = (previousPickupLocation + tempVelocity * velocityPredictionRange);
                EnableNav();
                nav.SetDestination(navDir);
            }
            else
            {
                EnableNav();
                nav.SetDestination(previousPickupLocation);
            }
        }
    }

    void ChooseRandomLocale()
    {
        if (!nav.hasPath)
        {
            currentChaseWaypoint = FindChaseLocation(PlayerManager.Instance.GetCurrentHolder().transform.position, transform.position);
            Vector3 tempWaypoint = currentChaseWaypoint.transform.position;
            tempWaypoint.y = 0;


            nav.SetDestination(tempWaypoint);
        }

        Vector3 tempCurrentPos = transform.position, tempCurrentWaypoint = currentChaseWaypoint.transform.position;
        tempCurrentPos.y = 0;
        tempCurrentWaypoint.y = 0;
    }

    void UpdateRandomLocale()
    {
        if (PlayerManager.Instance.GetCurrentHolder() == null)
        {
            ChangeSubState(SubState.Chase);
            return;
        }

        Vector3 tempCurrentPos = transform.position, tempHolderPos = PlayerManager.Instance.GetCurrentHolder().gameObject.transform.position;
        tempCurrentPos.y = 0;
        tempHolderPos.y = 0;

        if (Vector3.Distance(tempCurrentPos, tempHolderPos) < maxFleeRange)
        {
            ChangeSubState(SubState.Chase);
        }
    }

    public Transform FindFleeLocation(Vector3 otherPos, Vector3 currentPos)
    {
        List<Transform> tempHideSpots = new List<Transform>();
        List<Transform> finalTempHideSpots = new List<Transform>();

        for (int i = 0; i < waypoints.Count; i++)
        {
            Vector3 tempHideSpot = waypoints[i].position, tempOtherPos = otherPos, tempCurrentPos = currentPos;
            tempHideSpot.y = 0;
            tempOtherPos.y = 0;
            tempCurrentPos.y = 0;

            if (Mathf.Abs(Vector3.Angle((tempHideSpot - tempCurrentPos).normalized, (tempOtherPos - tempCurrentPos).normalized)) > angleOfFleeDistance)
            {
                tempHideSpots.Add(waypoints[i]);
            }
        }

        for (int i = 0; i < tempHideSpots.Count; i++)
        {
            Vector3 tempHideSpot = waypoints[i].position, tempOtherPos = otherPos, tempCurrentPos = currentPos;
            tempHideSpot.y = 0;
            tempOtherPos.y = 0;
            tempCurrentPos.y = 0;

            if (Vector3.Distance(tempHideSpots[i].position, otherPos) > angleOfFleeDistance)
            {
                finalTempHideSpots.Add(tempHideSpots[i]);
            }
        }

        if (finalTempHideSpots.Count > 0)
        {
            int randNumb = Random.Range(0, finalTempHideSpots.Count);
            return finalTempHideSpots[randNumb];
        }
        else
        {
            int randNumb = Random.Range(0, waypoints.Count);
            return waypoints[randNumb];
        }
    }

    public Transform FindChaseLocation(Vector3 otherPos, Vector3 currentPos)
    {
        List<Transform> tempHideSpots = new List<Transform>();
        List<Transform> finalTempHideSpots = new List<Transform>();

        for (int i = 0; i < waypoints.Count; i++)
        {
            Vector3 tempHideSpot = waypoints[i].position, tempOtherPos = otherPos, tempCurrentPos = currentPos;
            tempHideSpot.y = 0;
            tempOtherPos.y = 0;
            tempCurrentPos.y = 0;

            if (Mathf.Abs(Vector3.Angle((tempHideSpot - tempCurrentPos).normalized, (tempOtherPos - tempCurrentPos).normalized)) < angleOfChaseDistance)
            {
                tempHideSpots.Add(waypoints[i]);
            }
        }

        for (int i = 0; i < tempHideSpots.Count; i++)
        {
            Vector3 tempHideSpot = waypoints[i].position, tempOtherPos = otherPos, tempCurrentPos = currentPos;
            tempHideSpot.y = 0;
            tempOtherPos.y = 0;
            tempCurrentPos.y = 0;

            if (Vector3.Distance(tempHideSpots[i].position, otherPos) < angleOfChaseDistance)
            {
                finalTempHideSpots.Add(tempHideSpots[i]);
            }
        }

        if (finalTempHideSpots.Count > 0)
        {
            int randNumb = Random.Range(0, finalTempHideSpots.Count);
            return finalTempHideSpots[randNumb];
        }
        else
        {
            int randNumb = Random.Range(0, waypoints.Count);
            return waypoints[randNumb];
        }
    }

    void Flee()
    {
        bool characterInRange = false;
        Vector3 finalDestination = Vector3.zero;
        foreach(Character otherCharacter in PlayerManager.Instance.characters)
        {
            if (otherCharacter == this)
            {
                continue;
            }

            Vector3 tempCurrentPos = transform.position, tempOtherPos = otherCharacter.gameObject.transform.position;
            tempCurrentPos.y = 0;
            tempOtherPos.y = 0;

            if (Vector3.Distance(tempCurrentPos, tempOtherPos) > maxFleeRange)
            {
                continue;
            }
            else
            {
                characterInRange = true;

                Vector3 calculatedDirection = tempCurrentPos - tempOtherPos;
                float finalMag = maxFleeRange - calculatedDirection.magnitude;
                calculatedDirection.Normalize();
                calculatedDirection *= finalMag;

                finalDestination += calculatedDirection;
            }
        }

        NavMeshHit navEdgeHit;
        if (NavMesh.FindClosestEdge(transform.position, out navEdgeHit, NavMesh.AllAreas))
        {
            if (navEdgeHit.distance < maxFleeRange)
            {
                Vector3 tempCurrentPos = transform.position, tempOtherPos = navEdgeHit.position;
                tempCurrentPos.y = 0;
                tempOtherPos.y = 0;

                Vector3 calculatedDirection = tempCurrentPos - tempOtherPos;
                float finalMag = maxFleeRange - calculatedDirection.magnitude;
                calculatedDirection.Normalize();
                calculatedDirection *= (finalMag * 2);

                finalDestination += calculatedDirection;
            }
        }

        if (characterInRange)
        {
            Vector3 tempCurrentPos = transform.position;
            tempCurrentPos.y = 0;

            currentVelocity = (finalDestination);

            EnableNav();
            nav.SetDestination(finalDestination + tempCurrentPos);
        }
        else
        {
            if (currentFleeWaypoint == null)
            {
                int randNumb = Random.Range(0, waypoints.Count);
                currentFleeWaypoint = waypoints[randNumb];

                currentVelocity = (currentFleeWaypoint.position - new Vector3(transform.position.x, 0, transform.position.z));

                nav.SetDestination(currentFleeWaypoint.position);
            }

        }
    }

    void UpdateFleeWaypoint()
    {
        if (currentFleeWaypoint == null || (subState != SubState.Flee || subState != SubState.Dash))
        {
            return;
        }

        Vector3 tempCurrentPos = transform.position;
        tempCurrentPos.y = 0;

        if (Vector3.Distance(tempCurrentPos, currentFleeWaypoint.position) < .3f)
        {
            currentFleeWaypoint = null;
        }
    }

    void ChaseDash()
    {
        if (PlayerManager.Instance.GetCurrentHolder() == null)
        {
            dashingDestination = previousPickupLocation;
            isDashing = true;
            canDash = false;
        }
        else
        {
            Vector3 tempPickup = previousPickupLocation, tempVelocity = PlayerManager.Instance.GetCurrentHolder().currentVelocity;
            tempPickup.y = 0;
            tempVelocity.y = 0;

            int randNumb = Random.Range(0, 100);
            if (randNumb < velocityPredictionPercent)
            {
                dashingDestination = (previousPickupLocation + tempVelocity * velocityPredictionRange);
                isDashing = true;
                canDash = false;

                SetDashRotation(dashingDestination);
            }
            else
            {
                dashingDestination = previousPickupLocation;
                isDashing = true;
                canDash = false;

                SetDashRotation(dashingDestination);
            }
        }
    }

    void CarryDash()
    {
        bool characterInRange = false;
        Vector3 finalDestination = Vector3.zero;

        foreach (Character otherCharacter in PlayerManager.Instance.characters)
        {
            if (otherCharacter == this)
            {
                continue;
            }

            Vector3 tempCurrentPos = transform.position, tempOtherPos = otherCharacter.gameObject.transform.position;
            tempCurrentPos.y = 0;
            tempOtherPos.y = 0;

            if (Vector3.Distance(tempCurrentPos, tempOtherPos) > maxFleeRange)
            {
                continue;
            }
            else
            {
                characterInRange = true;

                Vector3 calculatedDirection = tempCurrentPos - tempOtherPos;
                float finalMag = maxFleeRange - calculatedDirection.magnitude;
                calculatedDirection.Normalize();
                calculatedDirection *= finalMag;

                finalDestination += calculatedDirection;
            }
        }

        if (characterInRange)
        {
            Vector3 tempCurrentPos = transform.position;
            tempCurrentPos.y = 0;

            currentVelocity = (finalDestination + tempCurrentPos).normalized * dashingSpeed;

            dashingDestination = finalDestination + tempCurrentPos;
            isDashing = true;
            canDash = false;

            SetDashRotation(dashingDestination);
        }
        else
        {
            if (currentFleeWaypoint == null)
            {
                int randNumb = Random.Range(0, waypoints.Count);
                currentFleeWaypoint = waypoints[randNumb];

                currentVelocity = (currentFleeWaypoint.position - new Vector3(transform.position.x, 0, transform.position.z));

                dashingDestination = currentFleeWaypoint.position;
                isDashing = true;
                canDash = false;

                SetDashRotation(dashingDestination);
            }
            else
            {
                dashingDestination = currentFleeWaypoint.position;
                isDashing = true;
                canDash = false;

                SetDashRotation(dashingDestination);
            }
        }
    }

    void Stand()
    {
        bool isPickupInRange = false;
        for (int i = 0; i < characterHand.currentCharactersInRange.Count; i++)
        {
            if (PlayerManager.Instance.characters[characterHand.currentCharactersInRange[i]].currentPickup != null)
            {
                isPickupInRange = true;
                break;
            }
        }

        if (isPickupInRange)
        {
            ChangeSubState(SubState.Attack);
            return;
        }
        else
        {
            int randAttackNumb = Random.Range(0, 100);
            if (randAttackNumb < attackPercent)
            {
                ChangeSubState(SubState.Attack);
                return;
            }
        }
    }

    void Knockback()
    {
        isKnockbacked = true;
        knockbackTime = maxKnockbackTime;

        Vector3 tempAttackerPos = attackerPos, tempCharacterPos = transform.position;
        tempAttackerPos.y = 0;
        tempCharacterPos.y = 0;

        Vector3 finalDir = tempCharacterPos - tempAttackerPos;
        finalDir.Normalize();
        knockbackDir = finalDir;

        isDazed = true;
        dazedTime = maxDazedTime;
        nav.speed = dazedSpeed;
    }

    void UpdateDash()
    {
        if (!canDash)
        {
            dashAbilityRecharge -= Time.deltaTime;

            if (dashAbilityRecharge <= 0)
            {
                canDash = true;
                dashAbilityRecharge = maxDashAbilityRecharge;
            }
        }

        if (isDashing)
        {
            if (dashTime >= 0)
            {
                dashTime -= Time.deltaTime;
                if (!isKnockbacked)
                {
                    Vector3 tempDashingDestination = dashingDestination, tempCurrentPos = transform.position;
                    tempDashingDestination.y = 0;
                    tempCurrentPos.y = 0;

                    Vector3 finalDir = tempDashingDestination - tempCurrentPos;
                    finalDir.Normalize();

                    EnableCharacter();
                    characterController.Move(finalDir * dashingSpeed * Time.deltaTime);
                }
            }
            else
            {
                isDashing = false;
                dashTime = maxDashTime;
                
                if (mainState == MainState.Chasing)
                {
                    ChangeSubState(SubState.Chase);
                }
                else
                {
                    ChangeSubState(SubState.Flee);
                }
            }
        }
    }

    void UpdateKnockback()
    {
        if (isKnockbacked)
        {
            knockbackTime -= Time.deltaTime;
            if (knockbackTime >= 0)
            {
                EnableCharacter();
                characterController.Move(knockbackDir * knockbackSpeed * Time.deltaTime);
            }
            else
            {
                isKnockbacked = false;

                if (mainState == MainState.Chasing)
                {
                    ChangeSubState(SubState.Chase);
                }
                else
                {
                    ChangeSubState(SubState.Flee);
                }
            }
        }
    }

    void UpdateDaze()
    {
        if (isDazed)
        {
            dazedTime -= Time.deltaTime;
            if (dazedTime <= 0)
            {
                isDazed = false;
                nav.speed = normalSpeed;
            }
        }
    }

    void UpdateStand()
    {
        if (isStanding)
        {
            standSubStateTimer -= Time.deltaTime;
            if(standSubStateTimer <= 0)
            {
                isStanding = false;
                standSubStateTimer = maxStandSubStateTimer.GetRandom();
                ChangeSubState(SubState.Chase);
            }
        }
    }

    void SetDashRotation(Vector3 dashDestination)
    {
        Vector3 tempDashDestination = dashDestination, tempCurrentPos = transform.position;
        tempDashDestination.y = 0;
        tempCurrentPos.y = 0;

        transform.rotation = Quaternion.LookRotation((tempDashDestination - tempCurrentPos).normalized, Vector3.up);
    }

    public void ChangeMainState(MainState newMainState)
    {
        switch (newMainState)
        {
            case MainState.Chasing:
                ChangeSubState(SubState.Chase);
                chaseSubStateTimer = maxChaseSubStateTimer.GetRandom();

                break;
            case MainState.Carrying:
                ChangeSubState(SubState.Flee);
                carrySubStateTimer = maxCarrySubStateTimer.GetRandom();

                break;
        }

        mainState = newMainState;
    }

    public void ChangeSubState(SubState newSubState)
    {
        switch (newSubState)
        {
            case SubState.Chase:
                characterController.enabled = false;
                nav.enabled = true;
                nav.isStopped = false;
                nav.ResetPath();

                break;
            case SubState.Dash:
                if (nav.enabled)
                {
                    nav.isStopped = true;
                    nav.enabled = false;
                }

                characterController.enabled = true;

                if (mainState == MainState.Chasing)
                {
                    if (canDash)
                    {
                        ChaseDash();
                    }
                    else
                    {
                        ChangeSubState(SubState.Chase);
                    }
                }
                else
                {
                    if (canDash)
                    {
                        CarryDash();
                    }
                    else
                    {
                        ChangeSubState(SubState.Flee);
                    }
                }
                
                break;
            case SubState.Attack:
                if (canAttack)
                {
                    Attack();
                }
                else
                {
                    ChangeSubState(SubState.Chase);
                }

                break;
            case SubState.Stand:
                nav.isStopped = true;

                if (isStanding && canAttack)
                {
                    standSubStateTimer = maxStandSubStateTimer.GetRandom();
                }

                isStanding = true;
                break;
            case SubState.Flee:
                characterController.enabled = false;
                nav.enabled = true;
                nav.isStopped = false;
                if (subState != SubState.Flee)
                {
                    nav.ResetPath();
                }
                currentFleeWaypoint = null;

                break;
            case SubState.Knockback:
                if (nav.enabled)
                {
                    nav.isStopped = true;
                    nav.enabled = false;
                }
                
                characterController.enabled = true;

                Knockback();
                break;
            case SubState.RandomLocale:
                characterController.enabled = false;
                nav.enabled = true;
                nav.isStopped = false;
                nav.ResetPath();

                ChooseRandomLocale();
                break;
        }

        subState = newSubState;
    }

    void UpdateMainStates()
    {
        if (currentPickup == null && mainState != MainState.Chasing)
        {
            ChangeMainState(MainState.Chasing);
        }
        else if (currentPickup != null && mainState != MainState.Carrying)
        {
            ChangeMainState(MainState.Carrying);
        }

        switch (mainState)
        {
            case MainState.Chasing:
                chaseSubStateTimer -= Time.deltaTime;

                if (chaseSubStateTimer <= 0)
                {
                    FindNewChaseSubState();
                    chaseSubStateTimer = maxChaseSubStateTimer.GetRandom();
                }
                break;
            case MainState.Carrying:
                carrySubStateTimer -= Time.deltaTime;

                if(carrySubStateTimer <= 0)
                {
                    FindNewCarrySubState();
                    carrySubStateTimer = maxCarrySubStateTimer.GetRandom();
                }
                break;
        }
    }

    void UpdateSubStates()
    {
        switch (subState)
        {
            case SubState.Chase:
                chasePredictionTimer -= Time.deltaTime;

                if (chasePredictionTimer <= 0)
                {
                    int randNumb = Random.Range(0, 100);
                    if (randNumb < velocityPredictionPercent)
                    {
                        shouldChasePredict = true;
                    }
                    else
                    {
                        shouldChasePredict = false;
                    }

                    chasePredictionTimer = maxChasePredictionTimer.GetRandom();
                }

                Chase();
                break;
            case SubState.Dash:
                break;
            case SubState.Attack:
                if (subState == SubState.Attack)
                {
                    ChangeSubState(SubState.Chase);
                }
                break;
            case SubState.Stand:
                Stand();
                break;
            case SubState.Flee:
                Flee();
                UpdateFleeWaypoint();
                break;
            case SubState.Knockback:
                break;
            case SubState.RandomLocale:
                UpdateRandomLocale();
                break;
        }
    }

    public override void DisableObj()
    {
        base.DisableObj();

        nav.isStopped = true;
        nav.enabled = false;
        characterController.enabled = false;

        Debug.Log("AI Disabled");
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

            UpdatePreviousLocation();

            ///Update pickup status
            OnCatchPickUp();

            ///Update all States
            UpdateMainStates();
            UpdateSubStates();

            ///Update all character controller components
            UpdateDash();
            UpdateKnockback();
            UpdateDaze();
            UpdateStand();

            if (currentPickup != null)
            {
                Holding();
            }
        }
    }
}
