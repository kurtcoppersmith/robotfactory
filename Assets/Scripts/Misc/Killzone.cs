using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour
{
    public GameObject origin;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //other.gameObject.GetComponent<PlayerMovement>().charController.enabled = false;
            other.transform.position = origin.transform.position;
            other.transform.rotation = origin.transform.rotation;
            //other.gameObject.GetComponent<PlayerMovement>().charController.enabled = true;

            //other.gameObject.GetComponent<PlayerMovement>().SetPlayerIced(false);
            //other.gameObject.GetComponent<PlayerMovement>().SetPlayerSlowed(false);
            //other.gameObject.GetComponent<PlayerModel>().playerPowerups.ResetAllPowerups();
            
            //if (other.GetComponent<PlayerModel>().isHolding)
            //{
            //    other.GetComponent<PlayerModel>().Fail();
            //}
            //else
            //{
            //    other.GetComponent<PlayerModel>().ChangeState(PlayerModel.PlayerState.Stunned);
            //}
        }
        
    }
}
