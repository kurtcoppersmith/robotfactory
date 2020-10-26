using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateManager : SingletonMonoBehaviour<CrateManager>
{
    //spawn time in seconds
    public float minSpawnTime;
    public float maxSpawnTime;
    //time till next spawn
    public float RemaningSpawnTime;
    //time till explosion
    public float duration;
    //speed of the crates
    public float crateSpeed;
    //power crate varibles
    public int tillNextPowerMin;
    public int tillNextPowerMax;
    private int tillNextPower;
    public bool spawnPowerCrate;
    //crate prefab for instantiating
    public GameObject Crate;

    private GameObject player;

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
        tillNextPower = Random.Range(tillNextPowerMin, tillNextPowerMax);

        for (int i = 0; i < spawnLocations.Count; i++)
        {
            spawnLocationStatus.Add(spawnLocations[i], false);
        }
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pooler = ObjectPooler.Instance;
        RemaningSpawnTime = Random.Range(minSpawnTime,maxSpawnTime);
        SpawnCrate();
        tillNextPower--;
    }

    // Update is called once per frame
    void Update()
    {
        if (RemaningSpawnTime <= 0 && !GameManager.Instance.hasEnded)
        {
            SpawnCrate();
            tillNextPower--;
            RemaningSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
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
            //if(spawnPowerCrate)
            currentSpawnedItems.Add(obj, spawnLocations[i]);
            if(tillNextPower <= 0)
            {
                obj.GetComponent<Crate>().powerupIndicator.SetActive(true);

                obj.GetComponent<Crate>().SpawnPower();
                tillNextPower = Random.Range(tillNextPowerMin, tillNextPowerMax);

                Random.Range(1, 100);

                switch (GameManager.Instance.item1)
                {
                    case "Strength":
                        obj.GetComponent<Crate>().powerupIndicator.GetComponent<Material>().color = Color.red;
                        break;
                    case "Speed":
                        obj.GetComponent<Crate>().powerupIndicator.GetComponent<Material>().color = Color.green;
                        break;
                    case "Chasis":
                        obj.GetComponent<Crate>().powerupIndicator.GetComponent<Material>().color = Color.blue;
                        break;
                    default:
                        break;
                }

                switch (GameManager.Instance.item2)
                {
                    case "Strength":
                        obj.GetComponent<Crate>().powerupIndicator.GetComponent<Material>().color = Color.red;
                        break;
                    case "Speed":
                        obj.GetComponent<Crate>().powerupIndicator.GetComponent<Material>().color = Color.green;
                        break;
                    case "Chasis":
                        obj.GetComponent<Crate>().powerupIndicator.GetComponent<Material>().color = Color.blue;
                        break;
                }

                //switch(obj.GetComponent<Crate>().power)
                //{
                //    case global::Crate.PowerUp.Strength:
                //        Debug.Log("Str");
                //        player.GetComponent<PlayerModel>().playerPowerups.SetStrengthPowerup(true);
                //        break;
                //    case global::Crate.PowerUp.Speed:
                //        Debug.Log("Spd");
                //        player.GetComponent<PlayerModel>().playerPowerups.SetSpeedPowerup(true);
                //        break;
                //    case global::Crate.PowerUp.Chasis:
                //        Debug.Log("Chs");
                //        player.GetComponent<PlayerModel>().playerPowerups.SetChasisPowerup(true);
                //        break;
                //}
            }
            spawnLocationStatus[spawnLocations[i]] = true;
            spawnNumbers.min++;
        }
    }

    void RemoveCrateFromCurrentActiveCount()
    {
        if (spawnNumbers.min > 0)
        {
            spawnNumbers.min--;

            if (spawnNumbers.min == spawnNumbers.max - 1)
            {
                RemaningSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
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
