﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveWireHazard : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        HazardManager.Instance.TakeCareOffHazard(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        Destroy(gameObject);

        Debug.Log("Wire Collision");

        if (other.gameObject.tag == "Player")
        {
            if (other.GetComponent<PlayerModel>().isHolding)
            {
                other.GetComponent<PlayerModel>().playerMovement.SetPlayerSlowed(true);
                other.GetComponent<PlayerModel>().qteManager.Fail();
            }
            else
            {
                other.GetComponent<PlayerModel>().playerMovement.SetPlayerSlowed(true);
                other.GetComponent<PlayerModel>().ChangeState(PlayerModel.PlayerState.Stunned);
            }
        }
    }
}
