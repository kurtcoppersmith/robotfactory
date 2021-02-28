using BasicTools.ButtonInspector;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pickup : MonoBehaviour
{
    public float innerRadius = 0;
    public float outerRadius = 0;

    public float fallSpeed = 0;

    private bool hasDropped = false;
    [HideInInspector] public bool hasLanded = true;
    private bool isPickedUp = false;
    private bool canPickUp = true;
    public float pickupCooldown = 0f;
    private float maxPickupCooldown = 0f;

    public GameObject minimapIcon;

    private Vector3 randomLocation = Vector3.zero;
    private Vector3 middleLocation = Vector3.zero;
    [ButtonAttribute("Test Drop", "Dropped")] [SerializeField]
    private bool _btnDrop;

    private void Awake()
    {
        hasLanded = true;
        maxPickupCooldown = pickupCooldown;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, innerRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, outerRadius);
    }

    public void SetPickedUp(bool result)
    {
        isPickedUp = result;

        if (result)
        {
            canPickUp = false;
        }
    }

    public bool IsPickedUp()
    {
        return isPickedUp;
    }

    public bool CanBePickedUp()
    {
        return canPickUp;
    }

    void UpdatePickupCooldown()
    {
        if (!canPickUp)
        {
            pickupCooldown -= Time.deltaTime;
            if(pickupCooldown <= 0)
            {
                canPickUp = true;
                pickupCooldown = maxPickupCooldown;
            }
        }
    }

    Vector3 FindRandomPosition()
    {
        float ratio = innerRadius / outerRadius;
        float radius = Mathf.Sqrt(Random.Range(ratio * ratio, 1f)) * outerRadius;
        float angle = Random.Range(0, 2f * Mathf.PI);

        Vector3 finalDir = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
        finalDir *= radius;
        finalDir += transform.position;

        NavMeshHit hit;
        NavMesh.SamplePosition(finalDir, out hit, radius, 1);
        return hit.position;
    }

    public void Dropped()
    {
        hasDropped = true;
        hasLanded = false;
        isPickedUp = false;

        randomLocation = FindRandomPosition();
        middleLocation = new Vector3((transform.position.x + randomLocation.x) / 2, 0f, (transform.position.z + randomLocation.z) / 2);
        middleLocation.y = transform.position.y + 3f;

        Vector3[] currentPath = 
            { middleLocation, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), middleLocation,
            randomLocation, middleLocation, new Vector3(randomLocation.x, randomLocation.y + 1, randomLocation.z) };
        transform.DOPath(currentPath, 1f, PathType.CubicBezier, PathMode.Sidescroller2D, 10, Color.red);
    }

    private void Update()
    {
        if (hasDropped && !hasLanded)
        {
            if (Vector3.Distance(transform.position, randomLocation) < transform.localScale.y)
            {
                transform.DOKill();
                hasDropped = false;
                hasLanded = true;
            }
        }

        UpdatePickupCooldown();
    }
}
