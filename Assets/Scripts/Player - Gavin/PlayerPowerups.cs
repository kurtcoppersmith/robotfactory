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

    public GameObject startParticle;
    public GameObject durationParticle;
    public GameObject stopParticle;

    public Color strengthColor = Color.red;
    public Color speedColor = Color.green;
    public Color chasisColor = Color.blue;

    private PlayerModel playerMod;

    void Awake()
    {
        playerMod = GetComponent<PlayerModel>();

        maxPowerupDuration = powerupDuration;
    }

    void DoDurationParticle()
    {
        startParticle.SetActive(false);
        durationParticle.SetActive(true);
    }

    void StopFinalParticle()
    {
        stopParticle.SetActive(false);
    }

    public void SetSpeedPowerup(bool option)
    {
        speedPower = option;

        if (option)
        {
            playerMod.playerMovement.speed = powerupSpeed;
            powerupDuration = maxPowerupDuration;

            SetChasisPowerup(false);
            SetStrengthPowerup(false);

            startParticle.GetComponentInChildren<ParticleSystem>().startColor = speedColor;
            durationParticle.GetComponentInChildren<ParticleSystem>().startColor = speedColor;
            stopParticle.GetComponentInChildren<ParticleSystem>().startColor = speedColor;

            startParticle.SetActive(true);
            Invoke("DoDurationParticle", startParticle.GetComponentInChildren<ParticleSystem>().main.duration);
        }
        else
        {
            playerMod.playerMovement.speed = playerMod.playerMovement.initialSpeed;

            if (durationParticle.activeInHierarchy == true)
            {
                durationParticle.SetActive(false);
                stopParticle.SetActive(true);
                Invoke("StopFinalParticle", stopParticle.GetComponentInChildren<ParticleSystem>().main.duration);
            }
        }
    }

    public void SpeedPowerup()
    {
        powerupDuration -= Time.deltaTime;

        if (powerupDuration <= 0)
        {
            SetSpeedPowerup(false);
            playerMod.playerMovement.speed = playerMod.playerMovement.initialSpeed;
        }
    }

    public void SetChasisPowerup(bool option)
    {
        chasisPower = option;

        if (option)
        {
            powerupDuration = maxPowerupDuration;

            SetSpeedPowerup(false);
            SetStrengthPowerup(false);

            startParticle.GetComponentInChildren<ParticleSystem>().startColor = chasisColor;
            durationParticle.GetComponentInChildren<ParticleSystem>().startColor = chasisColor;
            stopParticle.GetComponentInChildren<ParticleSystem>().startColor = chasisColor;

            startParticle.SetActive(true);
            Invoke("DoDurationParticle", startParticle.GetComponentInChildren<ParticleSystem>().main.duration);
        }
        else
        {
            if(durationParticle.activeInHierarchy == true)
            {
                durationParticle.SetActive(false);
                stopParticle.SetActive(true);
                Invoke("StopFinalParticle", stopParticle.GetComponentInChildren<ParticleSystem>().main.duration);
            }
        }
    }

    public void ChasisPowerup()
    {
        powerupDuration -= Time.deltaTime;

        if(powerupDuration <= 0)
        {
            SetChasisPowerup(false);
        }
    }

    public void SetStrengthPowerup(bool option)
    {
        strengthPower = option;

        if (option)
        {
            tempStrengthCollider.SetActive(true);

            SetSpeedPowerup(false);
            SetChasisPowerup(false);

            startParticle.GetComponentInChildren<ParticleSystem>().startColor = strengthColor;
            durationParticle.GetComponentInChildren<ParticleSystem>().startColor = strengthColor;
            stopParticle.GetComponentInChildren<ParticleSystem>().startColor = strengthColor;

            startParticle.SetActive(true);
            Invoke("DoDurationParticle", startParticle.GetComponentInChildren<ParticleSystem>().main.duration);
        }
        else
        {
            tempStrengthCollider.SetActive(false);

            if (durationParticle.activeInHierarchy == true)
            {
                durationParticle.SetActive(false);
                stopParticle.SetActive(true);
                Invoke("StopFinalParticle", stopParticle.GetComponentInChildren<ParticleSystem>().main.duration);
            }
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
