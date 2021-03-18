using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentInteractable : MonoBehaviour
{
    [Header("Cooldown Variables")]
    public bool canActivate = true;
    public float cooldownTimer = 0f;
    [HideInInspector] public float maxCooldownTimer = 0f;
    
    public virtual void InitiateInteractable(Character currentChar) { }
}