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
        if (collisionCrate.color == standColor)
        {
            crateManager.GetComponent<CrateManager>().DeliverCrate();
        }
        else
        {
            crateManager.GetComponent<CrateManager>().Explode();
        }
    }
}
