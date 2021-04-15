using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OilSpill : MonoBehaviour
{
    public float oilSpillTime = 0f;
    public GameObject oilSpill;
    public Collider oilCollider;
    private float maxOilSpillTime = 0f;
    private bool isActive = false;

    public float deactiveTime = 0.5f;
    private float maxDeactiveTime = 0f;
    private bool shouldDeactivate = false;
    private Vector3 currentScale;

    private void Awake()
    {
        maxOilSpillTime = oilSpillTime;
        maxDeactiveTime = deactiveTime;
        currentScale = oilSpill.transform.localScale;
        oilSpill.transform.localScale = Vector3.zero;

        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!shouldDeactivate)
        {
            Character currentChar = null;
            currentChar = other.GetComponent<Character>();
            if (currentChar != null)
            {
                Vector3 pushPosition = other.gameObject.transform.position - currentChar.currentVelocity;
                currentChar.OnHit(pushPosition);

                Debug.Log(currentChar.playerIndex);

                oilSpillTime = maxOilSpillTime;
                isActive = false;
                oilCollider.enabled = false;
                oilSpill.transform.DOScale(Vector3.zero, maxDeactiveTime);
                shouldDeactivate = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!shouldDeactivate)
        {
            Character currentChar = null;
            currentChar = other.GetComponent<Character>();
            if (currentChar != null)
            {
                Vector3 pushPosition = other.gameObject.transform.position - currentChar.currentVelocity;
                currentChar.OnHit(pushPosition);

                Debug.Log(currentChar.playerIndex);

                oilSpillTime = maxOilSpillTime;
                isActive = false;
                oilCollider.enabled = false;
                oilSpill.transform.DOScale(Vector3.zero, maxDeactiveTime);
                shouldDeactivate = true;
            }
        }
    }

    private void OnEnable()
    {
        oilCollider.enabled = true;
        oilSpill.transform.DOScale(currentScale, maxDeactiveTime);
        shouldDeactivate = false;
    }
    
    public void SetOilSpillTime(float oilTime)
    {
        oilSpillTime = oilTime;
        maxOilSpillTime = oilTime;
    }

    public void ActivateOilSpill()
    {
        isActive = true;
    }

    private void Update()
    {
        if (isActive)
        {
            oilSpillTime -= Time.deltaTime;

            if (oilSpillTime <= 0)
            {
                oilSpillTime = maxOilSpillTime;
                isActive = false;
                oilCollider.enabled = false;
                oilSpill.transform.DOScale(Vector3.zero, maxDeactiveTime);
                shouldDeactivate = true;
            }
        }

        if (shouldDeactivate)
        {
            deactiveTime -= Time.deltaTime;
            if (deactiveTime <= 0)
            {
                deactiveTime = maxDeactiveTime;
                shouldDeactivate = false;
                gameObject.SetActive(false);
            }
        }
    }
}
