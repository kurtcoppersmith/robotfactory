using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CreditScroll : MonoBehaviour
{
    public Transform startingPosition;
    public Transform endingPosition;
    public GameObject textToScroll;
    public float scrollSpeed;

    void OnEnable()
    {
        textToScroll.transform.position = startingPosition.position;
    }

    void Update()
    {
        textToScroll.transform.position = Vector3.Lerp(textToScroll.transform.position, endingPosition.position, scrollSpeed * Time.deltaTime);
    }

}
