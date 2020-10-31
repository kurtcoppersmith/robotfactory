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
                collision.transform.parent.GetComponent<PlayerModel>().TutorialFail();
            }
            else
            {
                collision.transform.parent.gameObject.GetComponent<PlayerModel>().Fail();
            }
        }
    }
}
