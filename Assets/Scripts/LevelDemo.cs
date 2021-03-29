using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class LevelDemo : MonoBehaviour
{
    [Header("Bridges")]
    [SerializeField] public List<Bridge> bridge;
    [Header("Objective")]
    [SerializeField] int totalRequiredCollectables;
    [SerializeField] public List<Collectible> collectItem;
    [Header("Objective AltWorld")]
    [SerializeField] int totalAltRequiredCollectables;
    [SerializeField] public List<Collectible> altCollectItem;
    [Header("Kill Box")]
    [SerializeField] public List<GameObject> DroneSpawn;
    [SerializeField] public GameObject enemy;
    [SerializeField] private int spawnIndex;
    [SerializeField] public float spawnAfter;
    [SerializeField] public float spawnEvery;
    [SerializeField] public bool spawnEnemy = true;
    private readonly float range = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("DroneSpawner", spawnAfter, spawnEvery);

        if (bridge != null && bridge.Count >= 2)
        {
            //range must be zero to the bridge.length, as the final number isn't inclusive
            //bridgeIndex = Random.Range(0, 2);
            //Debug.Log("Removing Bridge at index: " + bridgeIndex);
            //bridge[bridgeIndex].gameObject.SetActive(false);

            for (int b = 0; b <= bridge.Count - 2; b += 2)
            {
                int back = Random.Range(0, 2);
                if (back == 1)
                {
                    bridge[b].gameObject.SetActive(false);
                    continue;
                }
                else
                    bridge[b + 1].gameObject.SetActive(false);
                    continue;
            }
        }

        //method this later
        if (collectItem != null && collectItem.Count >= 2)
        {   
            //allows us to count how many loadables are left to remove
            int loadCollectables = totalRequiredCollectables;

            for (int i = 0; i <= collectItem.Count - 1; i++)
            {
                if (loadCollectables == 0 || collectItem.Count <= totalRequiredCollectables)
                {
                    break;
                }
                //if we reach the half way mark, guarantee the next collectable will be removed
                if (collectItem.Count - i == collectItem.Count / 2)
                {
                    //Destroy(collectItem[i].gameObject);
                    collectItem[i].gameObject.SetActive(false);
                    loadCollectables -= 1;
                    continue;
                }

                if (Random.Range(0, 2) == 1)
                {
                    //Destroy(collectItem[i].gameObject);
                    collectItem[i].gameObject.SetActive(false);
                    loadCollectables -= 1;
                }
            }
        }

        //method this later
        if (altCollectItem != null && altCollectItem.Count >= 2)
        {
            //allows us to count how many loadables are left to remove
            int loadAltCollectables = totalAltRequiredCollectables;

            for (int k = 0; k <= altCollectItem.Count - 1; k++)
            {
                if (loadAltCollectables == 0 || altCollectItem.Count <= totalAltRequiredCollectables)
                {
                    break;
                }
                //if we reach the half way mark, guarantee the next collectable will be removed
                if (altCollectItem.Count - k == altCollectItem.Count / 2)
                {
                    //Destroy(altCollectItem[k].gameObject);
                    altCollectItem[k].gameObject.SetActive(false);
                    loadAltCollectables -= 1;
                    continue;
                }
                if (Random.Range(0, 2) == 1)
                {
                    //Destroy(altCollectItem[k].gameObject);
                    altCollectItem[k].gameObject.SetActive(false);
                    loadAltCollectables -= 1;
                }
            }
        }

    }

    public void DroneSpawner()
    {
        if (spawnEnemy == true)
        {
            spawnIndex = Random.Range(0, DroneSpawn.Count);
            Vector3 spawnPoint = DroneSpawn[spawnIndex].gameObject.transform.position;
            Instantiate(enemy, RandomNavmeshLocation(range), Quaternion.identity);
        }
    }

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    }
