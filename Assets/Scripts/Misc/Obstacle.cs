using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.parent != null && collision.transform.parent.gameObject.tag == "Player")
        {
            if (TutorialManager.Instance != null)
            {
                collision.transform.parent.GetComponent<QTEManager>().TutorialFail();

                if (TutorialManager.Instance.currentObjective == 0)
                {
                    TutorialManager.Instance.hasCompletedCurrent = true;
                }
            }
            else
            {
                collision.transform.parent.gameObject.GetComponent<QTEManager>().Fail();
            }
        }
    }
}
