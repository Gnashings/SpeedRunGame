using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DimensionSwap : MonoBehaviour
{
    public GameObject[] KillBoxes;
    public GameObject[] Swappable;
    public GameObject[] AltObjects;
    PlayerController player;

    void Awake()
    {
        KillBoxes = GameObject.FindGameObjectsWithTag("Kill Box");
        Swappable = GameObject.FindGameObjectsWithTag("Swappable");
        AltObjects = GameObject.FindGameObjectsWithTag("Alt Object");
        player = gameObject.GetComponent<PlayerController>();
    }

    void Update()
    {
        KillBoxes = GameObject.FindGameObjectsWithTag("Kill Box");

        if (player.crossed == true)
        {
            SwapObjectMaterials(true);
            ShutDownKillBoxes(false);
            ShowAltWorldObjects(true);
        }

        if (player.crossed == false)
        {
            SwapObjectMaterials(false);
            ShutDownKillBoxes(true);
            ShowAltWorldObjects(false);
        }
    }

    void ShowAltWorldObjects(bool change)
    {
        for (int i = 0; i < AltObjects.Length; i++)
        {
            if (AltObjects[i].GetComponent<MeshCollider>() != null)
            {
                AltObjects[i].GetComponent<MeshCollider>().enabled = change;
            }
            if (AltObjects[i].GetComponent<MeshRenderer>() != null)
            {
                AltObjects[i].GetComponent<MeshRenderer>().enabled = change;
            }
            if (AltObjects[i].GetComponent<ParticleSystem>() != null)
            {
                var emits = AltObjects[i].GetComponent<ParticleSystem>().emission;
                emits.enabled = !change;
            }
        }
    }

    void SwapObjectMaterials(bool change)
    {
        for (int i = 0; i < Swappable.Length; i++)
        {
            if (Swappable[i].GetComponent<MaterialChange>() != null)
            {
                Swappable[i].GetComponent<MaterialChange>().check = change;
            }
        }
    }

    void ShutDownKillBoxes(bool change)
    {
        for (int i = 0; i < KillBoxes.Length; i++)
        {
            if (KillBoxes[i] != null)
            {

                if (KillBoxes[i].GetComponent<AudioSource>() != null)
                {
                    KillBoxes[i].GetComponent<AudioSource>().enabled = change;
                }
                if (KillBoxes[i].GetComponent<MonoBehaviour>() != null)
                {
                    KillBoxes[i].GetComponent<MonoBehaviour>().enabled = change;
                }
                if (KillBoxes[i].GetComponent<MeshRenderer>() != null)
                {
                    KillBoxes[i].GetComponent<MeshRenderer>().enabled = change;
                }
                if (KillBoxes[i].GetComponent<SkinnedMeshRenderer>() != null)
                {
                    KillBoxes[i].GetComponent<SkinnedMeshRenderer>().enabled = change;
                }
            }
        }
    }
}
