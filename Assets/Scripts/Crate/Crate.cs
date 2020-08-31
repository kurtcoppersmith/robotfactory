using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    public enum Colors{Red, Blue, Green}
    private float timer;
    private Material materialColor;
    private CrateManager manager;
    public bool delivered;
    public Color color;

    // Start is called before the first frame update
    void Start()
    {
        //manager
        if (GameObject.Find("Crate Manager") != null)
            manager = GameObject.Find("Crate Manager").GetComponent<CrateManager>();
        else
            Debug.LogError("Unable to find Crate Manager");
        //material
        materialColor = this.gameObject.GetComponent<Renderer>().material;
        //set crate color
        SpawnColor();
        //set crate timer
        timer = manager.duration;
        //
        delivered = false;
    }

    // Update is called once per frame
    void Update()
    {
        //lower crate timer
        timer -= Time.deltaTime;
        //call explode when timer = 0;
        if(timer<=0 && !delivered)
        {
            manager.Explode();
            Debug.Log("Time is up");
        }
    }

    private void SpawnColor()
    {
        switch((Colors)Random.Range(0,3))
        {
            case Colors.Blue:
                color = Color.blue;
                materialColor.color = Color.blue;
                break;
            case Colors.Green:
                color = Color.green;
                materialColor.color = Color.green;
                break;
            case Colors.Red:
                color = Color.red;
                materialColor.color = Color.red;
                break;
        }
    }
}
