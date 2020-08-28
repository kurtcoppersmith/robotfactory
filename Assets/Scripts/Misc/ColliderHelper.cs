using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColliderHelper : MonoBehaviour
{
    [Serializable]
    public class CollisionEvent : UnityEvent<Collision> { }

    [Serializable]
    public class TriggerEvent : UnityEvent<Collider> { }

    [Header("Collision Events")]
    public CollisionEvent onCollisionEnter;
    public CollisionEvent onCollisionStay;
    public CollisionEvent onCollisionExit;

    [Header("Trigger Events")]
    public TriggerEvent onTriggerEnter;
    public TriggerEvent onTriggerStay;
    public TriggerEvent onTriggerExit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision other)
    {
        onCollisionEnter?.Invoke(other);
    }

    public void OnCollisionStay(Collision other)
    {
        onCollisionStay?.Invoke(other);
    }

    public void OnCollisionExit(Collision other)
    {
        onCollisionExit?.Invoke(other);
    }

    public void OnTriggerEnter(Collider other)
    {
        onTriggerEnter?.Invoke(other);
    }

    public void OnTriggerStay(Collider other)
    {
        onTriggerStay?.Invoke(other);
    }

    public void OnTriggerExit(Collider other)
    {
        onTriggerExit?.Invoke(other);
    }
}
