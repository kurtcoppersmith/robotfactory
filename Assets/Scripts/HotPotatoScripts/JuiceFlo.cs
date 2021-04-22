using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JuiceFlo : MonoBehaviour
{
    public List<Transform> movementPath = new List<Transform>();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        if (movementPath.Count > 0)
        {
            Gizmos.DrawLine(transform.position, movementPath[0].transform.position);
        }

        for (int i = 1; i < movementPath.Count; i++)
        {
            Gizmos.DrawLine(movementPath[i - 1].transform.position, movementPath[i].transform.position);
        }
    }

    private void Start()
    {
        Vector3 startingPos = transform.position;

        Sequence moveSequence = DOTween.Sequence();
        Sequence rotateSequence = DOTween.Sequence();

        for (int i = 0; i < movementPath.Count; i++)
        {
            float randNumb = Random.Range(4, 8);
            moveSequence.Append(transform.DOMove(movementPath[i].transform.position, randNumb));
            
            Vector3 tempCurrent, tempNext;
            if (i == 0)
            {
                tempCurrent = startingPos;
                tempNext = movementPath[i].transform.position;
            }
            else
            {
                tempCurrent = movementPath[i - 1].transform.position;
                tempNext = movementPath[i].transform.position;
            }
            tempCurrent.y = 0;
            tempNext.y = 0;
            Vector3 finalDir = tempNext - tempCurrent;

            rotateSequence.Append(transform.DORotateQuaternion(Quaternion.LookRotation(finalDir, Vector3.up), randNumb));
        }

        if (movementPath.Count > 0)
        {
            float randNumb = Random.Range(4, 8);
            moveSequence.Append(transform.DOMove(startingPos, randNumb));

            Vector3 tempCurrent = movementPath[movementPath.Count - 1].transform.position, tempNext = startingPos;
            tempCurrent.y = 0;
            tempNext.y = 0;
            Vector3 finalDir = tempNext - tempCurrent;

            rotateSequence.Append(transform.DORotateQuaternion(Quaternion.LookRotation(finalDir, Vector3.up), randNumb));
        }

        moveSequence.SetLoops(-1);
        moveSequence.Play();

        rotateSequence.SetLoops(-1);
        rotateSequence.Play();
    }
}
