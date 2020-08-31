using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateManager : MonoBehaviour
{
    //spawn time in seconds
    public float SpawnTime;
    //time till next spawn
    private float RemaningSpawnTime;
    //time till explosion
    public float duration;
    //crate prefab for instantiating
    public GameObject Crate;
    // Start is called before the first frame update
    void Start()
    {
        SpawnCrate();
        RemaningSpawnTime = SpawnTime;
    }

    // Update is called once per frame
    void Update()
    {
       
        if(RemaningSpawnTime<=0)
        {
            SpawnCrate();
            RemaningSpawnTime = SpawnTime;
        }
        RemaningSpawnTime -= Time.deltaTime;
    }

    void SpawnCrate()
    {
        Instantiate(Crate, new Vector3(0, 2, 0), Quaternion.identity);
    }

    public void Explode()
    {
        //Call Game over 
        Debug.Log("You Lose Game Over");
    }

    public void DeliverCrate()
    {
        Debug.Log("Crate Delivered");
    }
}
