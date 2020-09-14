using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    #region Singleton and Pool Dictionary
    public static ObjectPooler Instance;

    void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        //Creates Pool dictionary
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        //adds items to the pool
        foreach (Pool item in objects)
        {
            Queue<GameObject> objectQueue = new Queue<GameObject>();
            for (int i = 0; i < item.size; i++)
            {
                //creates each item and sets it to false
                GameObject obj = Instantiate(item.prefab);
                obj.SetActive(false);
                objectQueue.Enqueue(obj);
            }
            poolDictionary.Add(item.tag, objectQueue);
        }
    }
    #endregion
    //makes pool public to the editer
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> objects = new List<Pool>();
    public Dictionary<string,Queue<GameObject>> poolDictionary;
    public GameObject SpawnFromPool(string tag, Vector3 spawnPoint, Quaternion rotation)
    {
        if(!poolDictionary.ContainsKey(tag))
        {
            Debug.LogError("Tag does not exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = spawnPoint;
        objectToSpawn.transform.rotation = rotation;
        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();

        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }
        poolDictionary[tag].Enqueue(objectToSpawn);
        return objectToSpawn;
    }
}
