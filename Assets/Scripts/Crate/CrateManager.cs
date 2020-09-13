using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateManager : SingletonMonoBehaviour<CrateManager>
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

    public List<Transform> spawnLocations;
    public Dictionary<Transform, bool> spawnLocationStatus { get; private set; }
    public Dictionary<GameObject, Transform> currentSpawnedItems { get; private set; } = new Dictionary<GameObject, Transform>();
    //Public variable to allow for easy spawning location of crate.

    public RangeInt spawnNumbers;
    
    new void Awake()
    {
        base.Awake();

        spawnLocationStatus = new Dictionary<Transform, bool>();
        spawnNumbers = new RangeInt(0, spawnLocations.Count);

        for (int i = 0; i < spawnLocations.Count; i++)
        {
            spawnLocationStatus.Add(spawnLocations[i], false);
        }
    }

    void Start()
    {
        
        SpawnCrate();
        RemaningSpawnTime = SpawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnNumbers.min < spawnNumbers.max && !GameManager.Instance.hasEnded)
        {
            if (RemaningSpawnTime <= 0)
            {
                SpawnCrate();
                RemaningSpawnTime = SpawnTime;
            }
            RemaningSpawnTime -= Time.deltaTime;
        }
    }

    void SpawnCrate()
    {
        for (int i = 0; i < spawnLocationStatus.Count; i++)
        {
            if (!spawnLocationStatus[spawnLocations[i]])
            {
                currentSpawnedItems.Add(Instantiate(Crate, spawnLocations[i].position, Quaternion.identity), spawnLocations[i]);
                spawnLocationStatus[spawnLocations[i]] = true;
                spawnNumbers.min++;
                break;
            }
        }
    }

    void RemoveCrateFromCurrentActiveCount()
    {
        if (spawnNumbers.min > 0)
        {
            spawnNumbers.min--;

            if (spawnNumbers.min == spawnNumbers.max - 1)
            {
                RemaningSpawnTime = SpawnTime;
            }
        }
    }

    public void Explode()
    {
        //Debug.Log("You Lose Game Over");

        RemoveCrateFromCurrentActiveCount();
    }

    public void DeliverCrate()
    {
        //Debug.Log("Crate Delivered");
        HazardManager.Instance.CleanUpHazards();
        HazardManager.Instance.SpawnHazards();

        RemoveCrateFromCurrentActiveCount();
    }
}
