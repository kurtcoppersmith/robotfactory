using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilSlickHazard : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        HazardManager.Instance.TakeCareOffHazard(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        Destroy(gameObject);

        if (other.gameObject.tag == "Player")
        {
            if (other.GetComponent<PlayerModel>().isHolding)
            {
                other.GetComponent<PlayerModel>().playerMovement.SetPlayerIced(true);
                //other.GetComponent<PlayerModel>().qteManager.Fail();
            }
            else
            {
                other.GetComponent<PlayerModel>().playerMovement.SetPlayerIced(true);
                //other.GetComponent<PlayerModel>().ChangeState(PlayerModel.PlayerState.Stunned);
            }
        }
    }
}
