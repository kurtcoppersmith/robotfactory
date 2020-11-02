using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthPowerupCollider : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Stand")
        {
            PlayerModel playerMod = GetComponentInParent<PlayerModel>();
            if (playerMod.isHolding)
            {
                if (playerMod.currentPickup.GetComponent<Crate>().bombType == other.gameObject.GetComponent<CrateStand>().bombType)
                {
                    playerMod.Passed();
                }
                else
                {
                    playerMod.Fail();
                }
            }
            
        }
    }
}
