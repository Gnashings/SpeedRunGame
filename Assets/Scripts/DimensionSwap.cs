using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DimensionSwap : MonoBehaviour
{
    public GameObject[] KillBoxes;
    public GameObject[] Swappable;
    public GameObject[] AltObjects;
    public GameObject[] NormalObjects;
    public GameObject[] NormalHealth;
    public GameObject[] AltCollectable;
    public GameObject[] Switches;
    public GameObject[] SerumAltObjects;

    PlayerController player;

    void Awake()
    {
        KillBoxes = GameObject.FindGameObjectsWithTag("Kill Box");
        Swappable = GameObject.FindGameObjectsWithTag("Swappable");
        AltObjects = GameObject.FindGameObjectsWithTag("Alt Object");
        NormalObjects = GameObject.FindGameObjectsWithTag("Normal World Only");
        NormalHealth = GameObject.FindGameObjectsWithTag("Heal");
        AltCollectable = GameObject.FindGameObjectsWithTag("Alt Collect");
        Switches = GameObject.FindGameObjectsWithTag("Switch");
        SerumAltObjects = GameObject.FindGameObjectsWithTag("Serum");
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
            NormalWorldOnly(false);
            HPNormalWorldOnly(false);
            ShowAltWorldCollectable(true);
            ShowSerumAltObjects(true);
        }

        if (player.crossed == false)
        {
            SwapObjectMaterials(false);
            ShutDownKillBoxes(true);
            ShowAltWorldObjects(false);
            NormalWorldOnly(true);
            HPNormalWorldOnly(true);
            ShowAltWorldCollectable(false);
            ShowSerumAltObjects(false);
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
            if (AltObjects[i].GetComponent<SphereCollider>() != null)
            {
                AltObjects[i].GetComponent<SphereCollider>().enabled = change;
            }
            if (AltObjects[i].GetComponent<BoxCollider>() != null)
            {
                AltObjects[i].GetComponent<BoxCollider>().enabled = change;
            }
            if (AltObjects[i].GetComponent<MeshRenderer>() != null)
            {
                AltObjects[i].GetComponent<MeshRenderer>().enabled = change;
            }
            if (AltObjects[i].GetComponent<SkinnedMeshRenderer>() != null)
            {
                AltObjects[i].GetComponent<SkinnedMeshRenderer>().enabled = change;
            }
            if (AltObjects[i].GetComponent<ParticleSystem>() != null)
            {
                var emits = AltObjects[i].GetComponent<ParticleSystem>().emission;
                emits.enabled = !change;
            }
        }
    }
    void ShowSwitches(bool change)
    {
        for (int i = 0; i < Switches.Length; i++)
        {
            if (Switches[i].GetComponent<SphereCollider>() != null)
            {
                AltObjects[i].GetComponent<SphereCollider>().enabled = change;
            }
            if (Switches[i].GetComponent<BoxCollider>() != null)
            {
                Switches[i].GetComponent<BoxCollider>().enabled = change;
            }
            if (Switches[i].GetComponent<MaterialChange>() != null)
            {
                Switches[i].GetComponent<MaterialChange>().enabled = change;
            }
        }
    }
    void ShowAltWorldCollectable(bool change)
    {
        for (int i = 0; i < AltCollectable.Length; i++)
        {
            if (AltCollectable[i].GetComponent<MeshCollider>() != null)
            {
                AltCollectable[i].GetComponent<MeshCollider>().enabled = change;
            }
            if (AltCollectable[i].GetComponent<SphereCollider>() != null)
            {
                AltCollectable[i].GetComponent<SphereCollider>().enabled = change;
            }
            if (AltCollectable[i].GetComponent<MeshRenderer>() != null)
            {
                AltCollectable[i].GetComponent<MeshRenderer>().enabled = change;
            }
            if (AltCollectable[i].GetComponent<SkinnedMeshRenderer>() != null)
            {
                AltCollectable[i].GetComponent<SkinnedMeshRenderer>().enabled = change;
            }
            if (AltCollectable[i].GetComponent<ParticleSystem>() != null)
            {
                var emits = AltCollectable[i].GetComponent<ParticleSystem>().emission;
                emits.enabled = change;
            }
        }
    }

    void ShowSerumAltObjects(bool change)
    {
        for (int i = 0; i < SerumAltObjects.Length; i++)
        {
            Debug.Log(SerumAltObjects[i]);
            if (SerumAltObjects[i].GetComponent<MeshCollider>() != null)
            {
                SerumAltObjects[i].GetComponent<MeshCollider>().enabled = change;
            }
            if (SerumAltObjects[i].GetComponent<SphereCollider>() != null)
            {
                SerumAltObjects[i].GetComponent<SphereCollider>().enabled = change;
            }
            if (SerumAltObjects[i].GetComponent<SkinnedMeshRenderer>() != null)
            {
                SerumAltObjects[i].GetComponent<SkinnedMeshRenderer>().enabled = change;
            }
            if (SerumAltObjects[i].GetComponent<ParticleSystem>() != null)
            {
                var emits = SerumAltObjects[i].GetComponent<ParticleSystem>().emission;
                emits.enabled = change;
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

    void NormalWorldOnly(bool change)
    {
        
        for (int i = 0; i < NormalObjects.Length; i++)
        {
            if (NormalObjects[i].GetComponent<MeshCollider>() != null)
            {
                NormalObjects[i].GetComponent<MeshCollider>().enabled = change;
            }
            if (NormalObjects[i].GetComponent<SphereCollider>() != null)
            {
                NormalObjects[i].GetComponent<SphereCollider>().enabled = change;
            }
            if (NormalObjects[i].GetComponent<MeshRenderer>() != null)
            {
                NormalObjects[i].GetComponent<MeshRenderer>().enabled = change;
            }
            if (NormalObjects[i].GetComponent<ParticleSystem>() != null)
            {
                var emits = NormalObjects[i].GetComponent<ParticleSystem>().emission;
                emits.enabled = change;
            }
        }
    }

    void HPNormalWorldOnly(bool change)
    {

        for (int i = 0; i < NormalHealth.Length; i++)
        {
            
            if (NormalHealth[i].GetComponent<SphereCollider>() != null)
            {
                NormalHealth[i].GetComponent<SphereCollider>().enabled = change;
            }
                
            if (NormalHealth[i].GetComponent<SkinnedMeshRenderer>() != null)
            {
                NormalHealth[i].GetComponent<SkinnedMeshRenderer>().enabled = change;
            }
            if (NormalHealth[i].GetComponent<ParticleSystem>() != null)
            {
                var emits = NormalHealth[i].GetComponent<ParticleSystem>().emission;
                emits.enabled = change;
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
