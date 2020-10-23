using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerups : MonoBehaviour
{
    [Header("Speed Up Powerup Variables")]
    public float powerupSpeed = 6f;
    private bool speedPower = false;

    [Header("Chasis Up Powerup Variables")]
    public bool chasisPower = false;

    [Header("Strength Up Powerup Variables")]
    public bool strengthPower = false;
    public GameObject tempStrengthCollider;

    [Header("General Variables")]
    public float powerupDuration = 0f;
    private float maxPowerupDuration = 0f;

    //public float strengthDeliveries = 0f;
    //private float maxStrengthDeliveries = 0f;

    private PlayerModel playerMod;

    void Awake()
    {
        playerMod = GetComponent<PlayerModel>();

        maxPowerupDuration = powerupDuration;
        //maxStrengthDeliveries = strengthDeliveries;
    }

    public void SetSpeedPowerup(bool option)
    {
        speedPower = option;

        if (option)
        {
            playerMod.playerMovement.speed = powerupSpeed;
            powerupDuration = maxPowerupDuration;

            chasisPower = false;
            strengthPower = false;
        }
    }

    public void SpeedPowerup()
    {
        powerupDuration -= Time.deltaTime;

        if (powerupDuration <= 0)
        {
            speedPower = false;
            playerMod.playerMovement.speed = playerMod.playerMovement.initialSpeed;
        }
    }

    public void SetChasisPowerup(bool option)
    {
        chasisPower = true;

        if (option)
        {
            powerupDuration = maxPowerupDuration;
            
            speedPower = false;
            strengthPower = false;
        }
    }

    public void ChasisPowerup()
    {
        powerupDuration -= Time.deltaTime;

        if(powerupDuration <= 0)
        {
            chasisPower = false;
        }
    }

    public void SetStrengthPowerup(bool option)
    {
        strengthPower = option;

        if (option)
        {
            tempStrengthCollider.SetActive(true);

            speedPower = false;
            chasisPower = false;
        }
        else
        {
            tempStrengthCollider.SetActive(false);
        }
    }

    void Update()
    {
        if (speedPower)
        {
            SpeedPowerup();
        }

        if (chasisPower)
        {
            ChasisPowerup();
        }
    }
}
