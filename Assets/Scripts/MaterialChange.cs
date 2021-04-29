using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChange : MonoBehaviour
{
    [HideInInspector] public bool check;
    MeshRenderer meshRenderer;
    SkinnedMeshRenderer skinnedMeshRenderer;
    [Tooltip("Place normal material in index 0, alt material in index 1")]
    public Material[] materials;

    void Start()
    {
        if (meshRenderer.GetComponent<MeshRenderer>() != null)
        {
            meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.enabled = true;
            meshRenderer.sharedMaterial = materials[0];
        }
        else if (skinnedMeshRenderer.GetComponent<MeshRenderer>() != null)
        {
            skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
            skinnedMeshRenderer.enabled = true;
            skinnedMeshRenderer.sharedMaterial = materials[0];
        }
        if (materials.Length < 2 && meshRenderer != null)
        {
            Debug.LogError("Please add materials to: " + gameObject);
        }
        if (!gameObject.tag.Equals("Swappable"))
        {
            Debug.LogError("Please add 'Swappable' tag to: " + gameObject);
        }
    }

void Update()
    {
        if (check == false)
        {
            if (meshRenderer.GetComponent<MeshRenderer>() != null)
            {
                meshRenderer.sharedMaterial = materials[0];
            }
            else if (skinnedMeshRenderer.GetComponent<MeshRenderer>() != null)
            {
                skinnedMeshRenderer.sharedMaterial = materials[0];
            }
        }
        else
        {
            if (meshRenderer.GetComponent<MeshRenderer>() != null)
            {
                meshRenderer.sharedMaterial = materials[1];
            }
            else if (skinnedMeshRenderer.GetComponent<MeshRenderer>() != null)
            {
                skinnedMeshRenderer.sharedMaterial = materials[1];
            }
        }
    }
}
