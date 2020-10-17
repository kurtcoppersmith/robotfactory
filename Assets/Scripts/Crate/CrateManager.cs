using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateManager : SingletonMonoBehaviour<CrateManager>
{
    //spawn time in seconds
    public float SpawnTime;
    //time till next spawn
    public float RemaningSpawnTime;
    //time till explosion
    public float duration;
    //speed of the crates
    public float crateSpeed;
    //crate prefab for instantiating
    public GameObject Crate;

    public GameObject CrateResetObject;

    //Object pooler for spawning the objects
    public ObjectPooler pooler;

    public List<Transform> spawnLocations;
    public Dictionary<Transform, bool> spawnLocationStatus { get; private set; }
    public Dictionary<GameObject, Transform> currentSpawnedItems { get; private set; } = new Dictionary<GameObject, Transform>();

    private ObjectPoolerGavin.Key crateKey = ObjectPoolerGavin.Key.Pickup;
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
        pooler = ObjectPooler.Instance;
        RemaningSpawnTime = SpawnTime;
        SpawnCrate();
    }

    // Update is called once per frame
    void Update()
    {
        if (RemaningSpawnTime <= 0 && !GameManager.Instance.hasEnded)
        {
            SpawnCrate();
            RemaningSpawnTime = SpawnTime;
        }
        RemaningSpawnTime -= Time.deltaTime;
    }

    void SpawnCrate()
    {
        for (int i = 0; i < spawnLocationStatus.Count; i++)
        {
                GameObject obj = ObjectPoolerGavin.GetPooler(crateKey).GetPooledObject();
                obj.transform.position = spawnLocations[i].position;
                obj.GetComponent<IdleCrate>().PickUp(false);
                obj.SetActive(true);
                currentSpawnedItems.Add(obj, spawnLocations[i]);
                
                spawnLocationStatus[spawnLocations[i]] = true;
                spawnNumbers.min++;
                break;
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
