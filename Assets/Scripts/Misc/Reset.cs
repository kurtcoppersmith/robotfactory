using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Pickup")
        {
            //put back into object pooler
            CrateManager.Instance.Explode();
        }
        Debug.Log(other.gameObject.name);
    }
}
