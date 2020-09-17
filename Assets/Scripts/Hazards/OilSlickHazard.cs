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
                //needs to be changed to speed up player and make them drift when moving like on ice
                other.GetComponent<PlayerModel>().qteManager.Fail();
            }
            else
            {
                //needs to be changed to speed up player and make them drift when moving like on ice
                other.GetComponent<PlayerModel>().ChangeState(PlayerModel.PlayerState.Stunned);
            }
        }
    }
}
