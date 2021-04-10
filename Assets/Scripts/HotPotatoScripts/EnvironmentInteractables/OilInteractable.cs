using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilInteractable : EnvironmentInteractable
{
    [Header("Oil Interactable")]
    public List<OilSpill> oilSpills = new List<OilSpill>();
    public float oilTime = 0f;

    void Awake()
    {
        maxCooldownTimer = cooldownTimer;
        UpdateCooldownSlider(1);

        for(int i = 0; i < oilSpills.Count; i++)
        {
            oilSpills[i].SetOilSpillTime(oilTime);
        }
    }

    public override void InitiateInteractable(Character currentChar)
    {
        if (!canActivate)
        {
            return;
        }

        base.InitiateInteractable(currentChar);
        
        for(int i = 0; i < oilSpills.Count; i++)
        {
            oilSpills[i].gameObject.SetActive(true);
            oilSpills[i].ActivateOilSpill();
        }

        canActivate = false;
        cooldownTimer = maxCooldownTimer;
    }

    public override void Update()
    {
        base.Update();

        if (!canActivate)
        {
            cooldownTimer -= Time.deltaTime;
            float cooldownVal =
                HelperUtilities.Remap(maxCooldownTimer - cooldownTimer, 0, maxCooldownTimer, 0, 1);
            UpdateCooldownSlider(cooldownVal);

            if (cooldownTimer <= 0)
            {
                canActivate = true;
                cooldownTimer = maxCooldownTimer;

                cooldownVal = 1;
                UpdateCooldownSlider(cooldownVal);
            }
        }
    }
}
