using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CrusherHazard : MonoBehaviour
{
    public RangeFloat startTime = new RangeFloat(0, 1.5f);
    private float startTimeCounter = 0;
    public float speed = 0f;
    public float soften = .9f;

    public Vector3 localMovement = Vector3.zero;
    private Vector3 originalLocation = Vector3.zero;
    private bool hasStartedMoving = false;

    void Awake()
    {
        originalLocation = transform.position;
        startTime.SelectRandom();
        startTimeCounter = (float)startTime.selected;
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
        float duration = Vector3.Distance(transform.position + localMovement, transform.position) / speed * soften;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalMove(transform.position + localMovement, duration)).Append(transform.DOMove(originalLocation, duration)).SetLoops(-1);
    }
    
    void Update()
    {
        startTimeCounter -= Time.deltaTime;

        if (startTimeCounter <= 0 && !hasStartedMoving)
        {
            Move();
            hasStartedMoving = true;
        }
    }

}
