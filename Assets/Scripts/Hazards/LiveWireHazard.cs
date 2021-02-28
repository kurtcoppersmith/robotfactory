using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveWireHazard : MonoBehaviour
{
    public string liveWireSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            HazardManager.Instance.TakeCareOffHazard(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
            Destroy(gameObject);

            if (TutorialManager.Instance != null && !TutorialManager.Instance.hitWire)
            {
                TutorialManager.Instance.hitWire = true;
            }

            //if (other.GetComponent<PlayerModel>().isHolding)
            //{
            //    other.GetComponent<PlayerModel>().playerMovement.SetPlayerSlowed(true);

            //    if (TutorialManager.Instance == null)
            //    {
            //        if (!other.gameObject.GetComponent<PlayerModel>().playerPowerups.chasisPower)
            //        {
            //            other.GetComponent<PlayerModel>().Fail();
            //            SoundEffectsManager.Instance.Play(liveWireSound);
            //        }
            //    }
            //    else
            //    {
            //        if (!other.gameObject.GetComponent<PlayerModel>().playerPowerups.chasisPower)
            //        {
            //            other.GetComponent<PlayerModel>().TutorialFail();
            //            SoundEffectsManager.Instance.Play(liveWireSound);
            //        }
            //    }
            //}
            //else
            //{
            //    other.GetComponent<PlayerModel>().playerMovement.SetPlayerSlowed(true);
            //    if (!other.gameObject.GetComponent<PlayerModel>().playerPowerups.chasisPower)
            //    {
            //        other.GetComponent<PlayerModel>().ChangeState(PlayerModel.PlayerState.Stunned);
            //        SoundEffectsManager.Instance.Play(liveWireSound);
            //    }
            //}
        }
    }
}
