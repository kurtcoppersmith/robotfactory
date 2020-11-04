using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlatformMover : MonoBehaviour
{
    //public float speed = 0f;
    public float duration = 0f;

    public Vector3 localMovement = Vector3.zero;
    private Vector3 originalLocation = Vector3.zero;

    void Awake()
    {
        originalLocation = transform.position;
    }

    void Start()
    {
        Move();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        if (Application.isPlaying)
        {
            Gizmos.DrawLine(transform.position, originalLocation + localMovement);
            Gizmos.DrawWireCube(originalLocation + localMovement, GetComponent<Collider>().bounds.size);
        }
        else
        {
            Gizmos.DrawLine(transform.position, transform.position + localMovement);
            Gizmos.DrawWireCube(transform.position + localMovement, GetComponent<Collider>().bounds.size);
        }

    }

    void Move()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalMove(transform.position + localMovement, duration)).Append(transform.DOMove(originalLocation, duration)).SetLoops(-1);
    }
}
