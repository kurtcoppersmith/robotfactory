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

    public List<Transform> spawnLocations;
    public Dictionary<Transform, bool> spawnLocationStatus { get; private set; }
    public Dictionary<GameObject, Transform> currentSpawnedItems { get; private set; } = new Dictionary<GameObject, Transform>();

    private ObjectPoolerGavin.Key atomKey = ObjectPoolerGavin.Key.AtomBomb;
    private ObjectPoolerGavin.Key fuseKey = ObjectPoolerGavin.Key.FuseBomb;
    private ObjectPoolerGavin.Key tntKey = ObjectPoolerGavin.Key.TNTBomb;
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
            int randBomb = Random.Range(0, 3);
            GameObject obj = null;

            switch (randBomb)
            {
                case 0:
                    obj = ObjectPoolerGavin.GetPooler(atomKey).GetPooledObject();
                    break;
                case 1:
                    obj = ObjectPoolerGavin.GetPooler(fuseKey).GetPooledObject();
                    break;
                case 2:
                    obj = ObjectPoolerGavin.GetPooler(tntKey).GetPooledObject();
                    break;
            }
            
            obj.transform.position = spawnLocations[i].position;
            obj.GetComponent<IdleCrate>().PickUp(false);
            obj.SetActive(true);

            currentSpawnedItems.Add(obj, spawnLocations[i]);
            spawnLocationStatus[spawnLocations[i]] = true;
            spawnNumbers.min++;

            if (tillNextPower <= 0)
            {
                if (GameManager.Instance.item1 == "" && GameManager.Instance.item2 == "")
                {
                    return;
                }

                GameObject powerupIndicator = obj.GetComponent<Crate>().powerupIndicator;
                powerupIndicator.SetActive(true);

                if (GameManager.Instance.item1 == "" && GameManager.Instance.item2 != "")
                {
                    switch (GameManager.Instance.item2)
                    {
                        case "Strength":
                            powerupIndicator.GetComponent<MeshRenderer>().material.color = Color.red;
                            obj.GetComponent<Crate>().SetPower(global::Crate.PowerUp.Strength);
                            Debug.Log("Strength!");
                            break;
                        case "Speed":
                            powerupIndicator.GetComponent<MeshRenderer>().material.color = Color.green;
                            obj.GetComponent<Crate>().SetPower(global::Crate.PowerUp.Speed);
                            Debug.Log("Speed!");
                            break;
                        case "Chasis":
                            powerupIndicator.GetComponent<MeshRenderer>().material.color = Color.blue;
                            obj.GetComponent<Crate>().SetPower(global::Crate.PowerUp.Chasis);
                            Debug.Log("Chasis!");
                            break;
                    }
                }
                else if (GameManager.Instance.item1 != "" && GameManager.Instance.item2 == "")
                {
                    switch (GameManager.Instance.item1)
                    {
                        case "Strength":
                            powerupIndicator.GetComponent<MeshRenderer>().material.color = Color.red;
                            obj.GetComponent<Crate>().SetPower(global::Crate.PowerUp.Strength);
                            Debug.Log("Strength!");
                            break;
                        case "Speed":
                            powerupIndicator.GetComponent<MeshRenderer>().material.color = Color.green;
                            obj.GetComponent<Crate>().SetPower(global::Crate.PowerUp.Speed);
                            Debug.Log("Speed!");
                            break;
                        case "Chasis":
                            powerupIndicator.GetComponent<MeshRenderer>().material.color = Color.blue;
                            obj.GetComponent<Crate>().SetPower(global::Crate.PowerUp.Chasis);
                            Debug.Log("Chasis!");
                            break;
                    }
                }
                else
                {
                    int randNumb = Random.Range(1, 100);

                    if (randNumb <= 50)
                    {
                        switch (GameManager.Instance.item1)
                        {
                            case "Strength":
                                powerupIndicator.GetComponent<MeshRenderer>().material.color = Color.red;
                                obj.GetComponent<Crate>().SetPower(global::Crate.PowerUp.Strength);
                                Debug.Log("Strength!");
                                break;
                            case "Speed":
                                powerupIndicator.GetComponent<MeshRenderer>().material.color = Color.green;
                                obj.GetComponent<Crate>().SetPower(global::Crate.PowerUp.Speed);
                                Debug.Log("Speed!");
                                break;
                            case "Chasis":
                                powerupIndicator.GetComponent<MeshRenderer>().material.color = Color.blue;
                                obj.GetComponent<Crate>().SetPower(global::Crate.PowerUp.Chasis);
                                Debug.Log("Chasis!");
                                break;
                        }
                    }
                    else
                    {
                        switch (GameManager.Instance.item2)
                        {
                            case "Strength":
                                powerupIndicator.GetComponent<MeshRenderer>().material.color = Color.red;
                                obj.GetComponent<Crate>().SetPower(global::Crate.PowerUp.Strength);
                                Debug.Log("Strength!");
                                break;
                            case "Speed":
                                powerupIndicator.GetComponent<MeshRenderer>().material.color = Color.green;
                                obj.GetComponent<Crate>().SetPower(global::Crate.PowerUp.Speed);
                                Debug.Log("Speed!");
                                break;
                            case "Chasis":
                                powerupIndicator.GetComponent<MeshRenderer>().material.color = Color.blue;
                                obj.GetComponent<Crate>().SetPower(global::Crate.PowerUp.Chasis);
                                Debug.Log("Chasis!");
                                break;
                        }
                    }
                }

                tillNextPower = Random.Range(tillNextPowerMin, tillNextPowerMax);
            }
            else
            {
                obj.GetComponent<Crate>().SetPower(global::Crate.PowerUp.None);
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
