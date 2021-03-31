using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneInteractable : EnvironmentInteractable
{
    void Awake()
    {
        maxCooldownTimer = cooldownTimer;
        UpdateCooldownSlider(1);
    }

    public override void InitiateInteractable(Character currentChar)
    {
        if (!canActivate)
        {
            return;
        }

        base.InitiateInteractable(currentChar);
        List<int> characterPool = new List<int>();

        for (int i = 0; i < PlayerManager.Instance.characters.Length; i++)
        {
            if (i == currentChar.playerIndex)
            {
                continue;
            }

            characterPool.Add(i);
        }

        int characterPoolIndex = Random.Range(0, characterPool.Count);
        Debug.Log(characterPool[characterPoolIndex]);
        PlayerManager.Instance.characters[characterPool[characterPoolIndex]].Stun();

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
