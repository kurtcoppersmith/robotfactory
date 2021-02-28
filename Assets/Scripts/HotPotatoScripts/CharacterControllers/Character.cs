﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    [Header("PlayerManager Variables")]
    public PlayerInput playerInput;
    public int playerIndex = -1;
    public bool isEnabled = false;
    public string characterName = "";

    [Header("External Colliders")]
    public CharacterHandCollider characterHand;
    public CharacterHandCollider catchCollider;

    [Header("Appearance Variables")]
    public MeshRenderer minimapIcon;
    public SkinnedMeshRenderer characterRenderer;
    public Camera playerCam = null;

    [Header("External Objects")]
    public GameObject currentPickup = null;
    public GameObject avatar;
    public Transform pickupTransform;

    [Header("Ability Cooldowns")]
    public float dashAbilityRecharge = 0f;
    [HideInInspector] public float maxDashAbilityRecharge = 0f;
    [HideInInspector] public bool canDash = true;

    public float attackAbilityRecharge = 0f;
    [HideInInspector] public float maxAttackAbilityRecharge = 0f;
    [HideInInspector] public bool canAttack = true;

    [Header("Dashing Variables")]
    public bool isDashing = false;
    public float dashingDistance = 0f;
    public float dashingSpeed = 0f;

    [HideInInspector] public float maxDashTime = 0f;
    [HideInInspector] public float dashTime = 0f;

    [Header("Knockback Variables")]
    public bool isKnockbacked = false;
    public float knockbackDistance = 0f;
    public float knockbackSpeed = 0f;

    [HideInInspector] public float maxKnockbackTime = 0f;
    [HideInInspector] public float knockbackTime = 0f;
    [HideInInspector] public Vector3 knockbackDir = Vector3.zero;

    [Header("Dazed Variables")]
    public float dazedSpeed = 0f;
    public float dazedTime = 0f;

    [HideInInspector] public bool isDazed = false;
    [HideInInspector] public float maxDazedTime = 0f;

    [Header("Current Velocity")]
    public Vector3 currentVelocity = Vector3.zero;

    public virtual void EnableObj() { isEnabled = true; }
    public virtual void Spawn(Vector3 spawnLocation) { }
    public virtual void Move() { }
    public virtual void Attack() { }
    public virtual void OnHit(Vector3 attackerPosition) 
    {
        if (currentPickup != null)
        {
            currentPickup.GetComponent<Pickup>().Dropped();
            Drop();
        }
    }

    public virtual void Drop()
    {
        if (currentPickup != null)
        {
            currentPickup.transform.parent = null;
            currentPickup = null;
            PlayerManager.Instance.RemoveCurrentHolder();
        }
    }

    public virtual void OnPickup(GameObject pickup) { }//currentPickup = pickup; }
}
