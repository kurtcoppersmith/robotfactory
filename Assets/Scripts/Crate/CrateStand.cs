using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateStand : MonoBehaviour
{
    public enum Colors { Red, Blue, Green }
    private Crate collisionCrate;
    public Colors colors;
    private Color standColor;
    public GameObject crateManager;
    // Start is called before the first frame update
    void Start()
    {
        if (colors == Colors.Red)
            standColor = Color.red;
        else if (colors == Colors.Blue)
            standColor = Color.blue;
        else
            standColor = Color.green;
    }

    private void OnCollisionEnter(Collision collision)
    {
        collisionCrate = collision.gameObject.GetComponent<Crate>();
        if (collisionCrate.color == standColor && collision.transform.parent.gameObject.tag == "Player")
        {
            if (TutorialManager.Instance == null)
            {
                CrateManager.Instance.DeliverCrate();
                collision.transform.parent.gameObject.GetComponent<PlayerModel>().Passed();
            }
            else
            {
                collision.transform.parent.gameObject.GetComponent<PlayerModel>().TutorialPassed();

                if (TutorialManager.Instance.currentObjective == 2 || TutorialManager.Instance.currentObjective == 3)
                {
                    TutorialManager.Instance.hasCompletedCurrent = true;
                }
            }
        }
        else if(collisionCrate.color != standColor && collision.transform.parent.gameObject.tag == "Player")
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
