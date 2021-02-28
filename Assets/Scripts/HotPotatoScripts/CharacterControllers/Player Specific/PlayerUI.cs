using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("HUD Display")]
    public TMPro.TextMeshProUGUI playerNameTextMesh;
    public Image dashAbilityImage;
    public Image punchAbilityImage;

    [Header("Minimap Variables")]
    public Camera minimapCam;
    public RawImage minimapRenderer;
    public float minimapRenderDistance;

    private RenderTexture minimapRender;
    private int currentCharPlayerIndex = -1;

    private Dictionary<int, GameObject> otherMinimapIcons = new Dictionary<int, GameObject>();

    private void Awake()
    {
        minimapRender = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
        minimapRender.Create();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, minimapRenderDistance);
    }

    public void UpdateUIElements(int playerIndex, string characterName)
    {
        minimapCam.targetTexture = minimapRender;
        minimapRenderer.texture = minimapRender;

        minimapCam.cullingMask = minimapCam.cullingMask | (1 << LayerMask.NameToLayer($"Player{playerIndex + 1}Minimap"));

        playerNameTextMesh.text = characterName;
    }

    public void AddOtherMinimapIcons(int playerIndex)
    {
        currentCharPlayerIndex = playerIndex;

        for (int i = 0; i < PlayerManager.Instance.characters.Length; i++)
        {
            if (i != playerIndex)
            {
                GameObject obj = Instantiate(PlayerManager.Instance.characters[i].minimapIcon.gameObject);
                obj.layer = (LayerMask.NameToLayer($"Player{(currentCharPlayerIndex + 1)}Minimap"));
                obj.transform.parent = transform;
                obj.SetActive(false);
                otherMinimapIcons.Add(i, obj);
            }
        }

        GameObject bombObj = Instantiate(PlayerManager.Instance.GetCurrentPickupIcon());
        bombObj.layer = (LayerMask.NameToLayer($"Player{(currentCharPlayerIndex + 1)}Minimap"));
        bombObj.transform.parent = transform;
        bombObj.SetActive(false);
        otherMinimapIcons.Add(5, bombObj);
    }

    void UpdateMinimap()
    {
        for (int i = 0; i < PlayerManager.Instance.characters.Length; i++)
        {
            if (i != currentCharPlayerIndex)
            {
                Vector3 tempCurrentChar = transform.position, 
                        tempOtherChar = PlayerManager.Instance.characters[i].gameObject.transform.position;
                tempCurrentChar.y = 0;
                tempOtherChar.y = 0;

                if (Vector3.Distance(tempCurrentChar, tempOtherChar) > minimapRenderDistance)
                {
                    Vector3 dirToOtherChar = (tempOtherChar - tempCurrentChar).normalized;
                    dirToOtherChar *= minimapRenderDistance;

                    Vector3 minimapFinalPosition = tempCurrentChar + dirToOtherChar;
                    minimapFinalPosition.y = PlayerManager.Instance.characters[i].minimapIcon.gameObject.transform.position.y + 0.1f;
                    otherMinimapIcons[i].transform.position = minimapFinalPosition;
                    otherMinimapIcons[i].SetActive(true);
                }
                else
                {
                    if (otherMinimapIcons[PlayerManager.Instance.characters[i].playerIndex].activeInHierarchy)
                    {
                        otherMinimapIcons[PlayerManager.Instance.characters[i].playerIndex].SetActive(false);
                    }
                }
            }
        }

        UpdateBombIcon();
    }

    void UpdateBombIcon()
    {
        Vector3 tempCurrentChar = transform.position,
                        tempOtherChar = PlayerManager.Instance.GetCurrentPickup().transform.position;
        tempCurrentChar.y = 0;
        tempOtherChar.y = 0;

        if (Vector3.Distance(tempCurrentChar, tempOtherChar) > minimapRenderDistance)
        {
            Vector3 dirToOtherChar = (tempOtherChar - tempCurrentChar).normalized;
            dirToOtherChar *= minimapRenderDistance;

            Vector3 minimapFinalPosition = tempCurrentChar + dirToOtherChar;
            minimapFinalPosition.y = PlayerManager.Instance.GetCurrentPickupIcon().transform.position.y + 0.1f;
            otherMinimapIcons[5].transform.position = minimapFinalPosition;
            otherMinimapIcons[5].SetActive(true);
        }
        else
        {
            if (otherMinimapIcons[5].activeInHierarchy)
            {
                otherMinimapIcons[5].SetActive(false);
            }
        }
    }

    void LateUpdate()
    {
        if (otherMinimapIcons.Count > 0 && currentCharPlayerIndex != -1)
        {
            UpdateMinimap();
        }
    }

    private void OnDestroy()
    {
        minimapRender.Release();
    }
}
