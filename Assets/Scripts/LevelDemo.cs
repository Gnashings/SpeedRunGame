using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class LevelDemo : MonoBehaviour
{
    [Header("Bridges")]
    [SerializeField] public List<Bridge> bridge;
    [SerializeField] public int bridgeIndex;
    [Header("Objective")]
    [SerializeField] public List<Collectible> collectItem;
    [SerializeField] public int collectionCount;
    [Header("Kill Box")]
    [SerializeField] public List<GameObject> DroneSpawn;
    [SerializeField] public GameObject enemy;
    [SerializeField] private int spawnIndex;
    [SerializeField] public float spawnAfter;
    [SerializeField] public float spawnEvery;
    private float range = 20.0f;

    // Start is called before the first frame update
    void Start()
    {

        InvokeRepeating("DroneSpawner", spawnAfter, spawnEvery);
        if (bridge != null && bridge.Count >= 2)
        {
            //range must be zero to the bridge.length, as the final number isn't inclusive
            bridgeIndex = Random.Range(0, 2);
            Debug.Log("Removing Bridge at index: " + bridgeIndex);
            bridge[bridgeIndex].gameObject.SetActive(false);
        }

        if (collectItem != null && collectItem.Count >= 2)
        {
            //range must be zero to the collectItem.length, as the final number isn't inclusive
            collectionCount = Random.Range(0, 2);
            {

            }
        }
        //Instantiate(enemy, new Vector3(-2.343854f, 3.784546f, -0.07437605f), Quaternion.identity);
    }

    public void DroneSpawner()
    {
        spawnIndex = Random.Range(0, DroneSpawn.Count - 1);
        Vector3 spawnPoint = DroneSpawn[spawnIndex].gameObject.transform.position;
        Debug.Log(DroneSpawn[spawnIndex] + " " + spawnPoint);
        //Instantiate(enemy, new Vector3(-2.343854f, 3.784546f, -0.07437605f), Quaternion.identity);
        //Instantiate(enemy, spawnPoint, Quaternion.identity);
        Instantiate(enemy, RandomNavmeshLocation(range), Quaternion.identity);
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

    public void BridgeLoader()
        {
            if (bridge != null && bridge.Count >= 2)
            {
                //range must be zero to the bridge.length, as the final number isn't inclusive
                //bridgeIndex = Random.Range(0, 2);
                Debug.Log("Removing Bridge at index: " + bridgeIndex);
                bridge[bridgeIndex].gameObject.SetActive(false);
            }
        }

    }
