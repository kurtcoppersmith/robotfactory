using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHandCollider : MonoBehaviour
{
    public Character characterController;
    public float colliderRange = 0f;
    public List<Collider> currentPickupColliders { get; private set; } = new List<Collider>();
    public List<int> currentCharactersInRange { get; private set; } = new List<int>();

    private int currentPlayerIndex = -1;

    //interactables in range will go here as well.

    public void SetPlayerIndex(int aPlayerIndex)
    {
        currentPlayerIndex = aPlayerIndex;
        currentCharactersInRange.Clear();
        currentPickupColliders.Clear();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, colliderRange);
    }

    void AddElements()
    {
        if (!characterController.isEnabled)
        {
            return;
        }

        foreach(Character otherCharacter in PlayerManager.Instance.characters)
        {
            if (otherCharacter.gameObject.activeInHierarchy && characterController.playerIndex != otherCharacter.playerIndex)
            {
                Vector3 tempCurrentPos = transform.position, tempOtherPos = otherCharacter.gameObject.transform.position;
                tempCurrentPos.y = 0;
                tempOtherPos.y = 0;

                if (Vector3.Distance(tempCurrentPos, tempOtherPos) < colliderRange)
                {
                    if (!currentCharactersInRange.Contains(otherCharacter.playerIndex))
                    {
                        currentCharactersInRange.Add(otherCharacter.playerIndex);
                        return;
                    }
                }
            }
        }

        if (PlayerManager.Instance.GetCurrentPickup().activeInHierarchy)
        {
            Vector3 tempCurrentPos = transform.position, tempOtherPos = PlayerManager.Instance.GetCurrentPickup().transform.position;
            tempCurrentPos.y = 0;
            tempOtherPos.y = 0;

            if (Vector3.Distance(tempCurrentPos, tempOtherPos) < colliderRange)
            {
                if (!currentPickupColliders.Contains(PlayerManager.Instance.GetCurrentPickup().GetComponent<Collider>()))
                {
                    currentPickupColliders.Add(PlayerManager.Instance.GetCurrentPickup().GetComponent<Collider>());
                    return;
                }
            }
        }
    }

    void RemoveElements()
    {
        if (!characterController.isEnabled)
        {
            return;
        }

        foreach (Character otherCharacter in PlayerManager.Instance.characters)
        {
            if (otherCharacter.gameObject.activeInHierarchy && characterController.playerIndex != otherCharacter.playerIndex)
            {
                Vector3 tempCurrentPos = transform.position, tempOtherPos = otherCharacter.gameObject.transform.position;
                tempCurrentPos.y = 0;
                tempOtherPos.y = 0;

                if (Vector3.Distance(tempCurrentPos, tempOtherPos) > colliderRange)
                {
                    if (currentCharactersInRange.Contains(otherCharacter.playerIndex))
                    {
                        currentCharactersInRange.Remove(otherCharacter.playerIndex);
                        return;
                    }
                }
            }
        }

        if (PlayerManager.Instance.GetCurrentPickup().activeInHierarchy)
        {
            Vector3 tempCurrentPos = transform.position, tempOtherPos = PlayerManager.Instance.GetCurrentPickup().transform.position;
            tempCurrentPos.y = 0;
            tempOtherPos.y = 0;

            if (Vector3.Distance(tempCurrentPos, tempOtherPos) > colliderRange)
            {
                if (currentPickupColliders.Contains(PlayerManager.Instance.GetCurrentPickup().GetComponent<Collider>()))
                {
                    currentPickupColliders.Remove(PlayerManager.Instance.GetCurrentPickup().GetComponent<Collider>());
                    return;
                }
            }
        }
    }

    private void Update()
    {
        AddElements();
        RemoveElements();
    }
}
