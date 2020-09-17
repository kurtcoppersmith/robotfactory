using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveWireHazard : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        HazardManager.Instance.TakeCareOffHazard(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        Destroy(gameObject);

        if (other.gameObject.tag == "Player")
        {
            if (other.GetComponent<PlayerModel>().isHolding)
            {
                //needs to be changed to after stun slows player for a amount of time
                other.GetComponent<PlayerModel>().qteManager.Fail();
            }
            else
            {
                //needs to be changed to after stun slows player for a amount of time
                other.GetComponent<PlayerModel>().ChangeState(PlayerModel.PlayerState.Stunned);
            }
        }
    }
}
