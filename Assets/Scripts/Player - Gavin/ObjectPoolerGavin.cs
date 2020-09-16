using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolerGavin : MonoBehaviour
{
    public static ObjectPoolerGavin current;
    public int pooledAmount = 20;
    public bool willGrow = true;

    private int count = 1;

    List<GameObject> pooledObjects;

    public enum Key
    {
        Pickup
    }

    public Key key;
    public GameObject pooledObject;

    public static Dictionary<Key, ObjectPoolerGavin> dict = new Dictionary<Key, ObjectPoolerGavin>();



    void Start()
    {
        dict[key] = this;

        pooledObjects = new List<GameObject>();
        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = (GameObject)Instantiate(pooledObject);
            obj.transform.parent = this.transform;
            obj.name = pooledObject.name + " " + (count);
            count++;

            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        if (willGrow)
        {
            GameObject obj = (GameObject)Instantiate(pooledObject);
            obj.transform.parent = this.transform;
            obj.name = pooledObject.name + " " + (count);
            count++;

            pooledObjects.Add(obj);
            return obj;
        }

        return null;
    }

    public static ObjectPoolerGavin GetPooler(Key key)
    {
        return dict[key];
    }
}