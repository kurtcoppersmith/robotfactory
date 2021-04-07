using BasicTools.ButtonInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshTestingScript : MonoBehaviour
{
    [ButtonAttribute("Change Nav Mesh", "ChangeNavMesh")]
    [SerializeField]
    private bool _btnSpawnShield;
    public NavMeshObstacle otherNavMesh;

    public void ChangeNavMesh()
    {
        otherNavMesh.carving = !otherNavMesh.carving;
    }
}
