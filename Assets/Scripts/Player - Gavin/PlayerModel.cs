﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerModel : MonoBehaviour
{
    public float playerStunnedTime = 3f;
    [Range(0,1)]
    public float bottomOfCharacter = 0.25f;
    public float collisionDetectionDistance = 0.2f;
    private float maxPlayerStunnedTime;

    public PlayerMovement playerMovement { get; private set; }
    public QTEManager qteManager { get; private set; }

    public PlayerPickup playerPickup;

    public BoxCollider pickupColliderGizmo;

    public Transform carryingPosition;

    public GameObject sparksParticleEffect;

    public ParticleSystem sparksParticleSystem;
    private float sparksParticleDuration;

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
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pickupColliderGizmo.transform.position, pickupColliderGizmo.size);
    }

    void OnBoxPickup(InputValue inputValue)
    {
        if (currentPickup != null || GameManager.Instance.hasEnded)
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
        CrateManager.Instance.Explode();
        CrateManager.Instance.spawnLocationStatus[CrateManager.Instance.currentSpawnedItems[currentPickup]] = false;
        playerPickup.currentColliders.Remove(currentPickup.GetComponent<Collider>());

        currentPickup.SetActive(false);
        currentPickup = null;
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

                    if (hit.collider.gameObject.tag == "Hazard" && currentPickup != null)
                    {
                        qteManager.Fail();
                    }
                }

            }
            DebugExtension.DebugCapsule(p1 + new Vector3(Mathf.Cos(i), 0, Mathf.Sin(i)), p2 + new Vector3(Mathf.Cos(i), 0, Mathf.Sin(i)), 0);
        }

        //if (Physics.Raycast(transform.position + Vector3.up, -Vector3.up, out hit))
        //{
        //    playerMovement.charController.Move(Vector3.up * (1 - hit.distance));
        //}
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

                sparksParticleEffect.SetActive(true);
                sparksParticleDuration = sparksParticleSystem.main.duration;
                transform.DOPunchPosition(new Vector3(Random.Range(0.1f, 0.2f), 0, Random.Range(0.1f, 0.2f)), maxPlayerStunnedTime / 2);
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

        DetectCollisions();
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
        }
    }

    void OnPauseToggle(InputValue inputValue)
    {
        if (playerState == PlayerState.Moving && !GameManager.Instance.hasEnded)
        {
            LevelManager.Instance.PauseToggle();
        }
    }

    void OnControllerColliderHit(ControllerColliderHit other)
    {
        if (other.gameObject.tag == "Hazard" && currentPickup != null)
        {
            Debug.Log("Test");
            qteManager.Fail();
        }
    }
}
