using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentInteractable : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject worldCanvas;
    public UnityEngine.UI.Slider cooldownSlider;
    public BoxCollider interactableCollider;
    public MeshRenderer objectMeshRenderer;
    public int materialOutlineIndex = 0;
    public Material greenMat;
    public Material redMat;

    private Material currentMat;

    [Header("Activation Variables")]
    public float colliderBoxBounds = 0;
    public bool shouldUseSameBoxColliderBounds = false;

    [Header("Cooldown Variables")]
    public bool canActivate = true;
    public float cooldownTimer = 0f;
    [HideInInspector] public float maxCooldownTimer = 0f;

    private void Awake()
    {
        currentMat = objectMeshRenderer.materials[materialOutlineIndex];
    }

    void Start()
    {
        worldCanvas.SetActive(false);
        interactableCollider.isTrigger = true;

        currentMat = greenMat;
        objectMeshRenderer.materials[materialOutlineIndex] = greenMat;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 cubeSize = new Vector3(colliderBoxBounds, colliderBoxBounds, colliderBoxBounds);
        Gizmos.DrawWireCube(transform.position, cubeSize);
    }

    public void UpdateCooldownSlider(float val)
    {
        cooldownSlider.value = val;
    }

    /// <summary>
    /// Must call base.Update() on any interactable.
    /// </summary>
    public virtual void Update()
    {
        if (interactableCollider.size.magnitude != colliderBoxBounds && shouldUseSameBoxColliderBounds)
        {
            
            interactableCollider.size = new Vector3(colliderBoxBounds, colliderBoxBounds, colliderBoxBounds);
        }

        if (cooldownSlider.value < 1)
        {
            if (currentMat != redMat)
            {
                Material[] tempMaterials = objectMeshRenderer.materials;
                tempMaterials[materialOutlineIndex] = redMat;

                objectMeshRenderer.materials = tempMaterials;

                currentMat = redMat;
            }
        }
        else
        {
            if (currentMat != greenMat)
            {
                Material[] tempMaterials = objectMeshRenderer.materials;
                tempMaterials[materialOutlineIndex] = greenMat;

                objectMeshRenderer.materials = tempMaterials;

                currentMat = greenMat;
            }
        }
    }

    void OnTriggerEnter(Collider player)
    {
        worldCanvas.SetActive(true);
    }

    void OnTriggerExit(Collider player)
    {
        worldCanvas.SetActive(false);
    }

    public virtual void InitiateInteractable(Character currentChar) { }
}