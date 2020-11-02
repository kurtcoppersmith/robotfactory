using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateStand : MonoBehaviour
{
    public Crate.BombType bombType;
    
    public GameObject crateManager;
    private Crate collisionCrate;

    public GameObject deliveryParticleEffect;

    void StopDeliveryParticles()
    {
        deliveryParticleEffect.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        collisionCrate = collision.gameObject.GetComponent<Crate>();
        if (collisionCrate.bombType == bombType && collision.transform.parent.gameObject.tag == "Player")
        {
            if (TutorialManager.Instance == null)
            {
                CrateManager.Instance.DeliverCrate();
                collision.transform.parent.gameObject.GetComponent<PlayerModel>().Passed();

                deliveryParticleEffect.SetActive(true);
                Invoke("StopDeliveryParticles", deliveryParticleEffect.GetComponentInChildren<ParticleSystem>().main.duration);
            }
            else
            {
                collision.transform.parent.gameObject.GetComponent<PlayerModel>().TutorialPassed();

                if (TutorialManager.Instance.currentObjective == 1 || TutorialManager.Instance.currentObjective == 2)
                {
                    TutorialManager.Instance.hasCompletedCurrent = true;
                }

                deliveryParticleEffect.SetActive(true);
                Invoke("StopDeliveryParticles", deliveryParticleEffect.GetComponentInChildren<ParticleSystem>().main.startLifetime.constant);
            }
        }
        else if(collisionCrate.bombType != bombType && collision.transform.parent.gameObject.tag == "Player")
        {
            if (TutorialManager.Instance == null)
            {
                collision.transform.parent.gameObject.GetComponent<PlayerModel>().Fail();
            }
            else
            {
                collision.transform.parent.gameObject.GetComponent<PlayerModel>().TutorialFail();
            }
        }
        
    }
}
