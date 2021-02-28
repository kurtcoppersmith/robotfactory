using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilSlickHazard : MonoBehaviour
{
    public string oilSlickSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            HazardManager.Instance.TakeCareOffHazard(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
            Destroy(gameObject);
            SoundEffectsManager.Instance.Play(oilSlickSound);

            if (TutorialManager.Instance != null && !TutorialManager.Instance.hitOil)
            {
                TutorialManager.Instance.hitOil = true;
            }

            //if (other.GetComponent<PlayerModel>().isHolding)
            //{
            //    other.GetComponent<PlayerModel>().playerMovement.SetPlayerIced(true);
            //    //other.GetComponent<PlayerModel>().qteManager.Fail();
            //}
            //else
            //{
            //    other.GetComponent<PlayerModel>().playerMovement.SetPlayerIced(true);
            //    //other.GetComponent<PlayerModel>().ChangeState(PlayerModel.PlayerState.Stunned);
            //}
        }
    }
}
