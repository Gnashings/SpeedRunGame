using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChange : MonoBehaviour
{
    [HideInInspector] public bool check;
    MeshRenderer meshRenderer;
    [Tooltip("Place normal material in index 0, alt material in index 1")]
    public Material[] materials;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = true;
        meshRenderer.sharedMaterial = materials[0];

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
            meshRenderer.sharedMaterial = materials[0];
        }
        else
            meshRenderer.sharedMaterial = materials[1];
    }
}
