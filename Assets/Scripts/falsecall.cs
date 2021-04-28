using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class falsecall : MonoBehaviour
{
    GameObject[] Fridges;
    public bool swap;
    void Start()
    {
        Fridges = GameObject.FindGameObjectsWithTag("Swappable");
    }

    void Update()
    {
        if (swap == true)
        {
            SwapObjectMaterials(swap);
        }

        if (swap == false)
        {
            SwapObjectMaterials(swap);
        }
    }

    void SwapObjectMaterials(bool change)
    {
        for (int i = 0; i < Fridges.Length - 1; i++)
        {
            if (Fridges[i].GetComponent<MaterialChange>() != null)
            {
                Fridges[i].GetComponent<MaterialChange>().check = change;
            }
        }
    }
}
