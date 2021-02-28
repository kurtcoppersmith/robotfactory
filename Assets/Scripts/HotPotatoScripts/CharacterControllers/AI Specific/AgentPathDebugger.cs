using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentPathDebugger : MonoBehaviour
{
    public NavMeshAgent nav;
    public List<Vector3> positions = new List<Vector3>();


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        for (int i = 1; i < positions.Count; i++)
        {
            Gizmos.DrawLine(positions[i - 1], positions[i]);
        }
    }

    private void Update()
    {
        if (nav.hasPath)
        {
            positions = new List<Vector3>(nav.path.corners);
        }
        else
        {
            positions.Clear();
        }
    }
}
