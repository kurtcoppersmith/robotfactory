using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyTransformGizmo : MonoBehaviour
{
    public float boxSize = 0f;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position, new Vector3(boxSize, boxSize, boxSize));
    }
}
