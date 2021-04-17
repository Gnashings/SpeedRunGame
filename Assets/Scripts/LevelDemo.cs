using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelDemo : MonoBehaviour
{

    [Header("Bridges")]
    [SerializeField] public List<Bridge> bridge;
    [Header("Objective")]
    [SerializeField] int totalNormalCollectables;
    [SerializeField] public List<Collectible> collectItem;
    [SerializeField] public List<SwitchScipt> Interactables;
    [Header("Objective AltWorld")]
    [SerializeField] int totalAltCollectables;
    [SerializeField] public List<Collectible> altCollectItem;
    [SerializeField] public List<SwitchScipt> altInteractables;
    [Header("Kill Box")]
    [SerializeField] public List<GameObject> DroneSpawn;
    [SerializeField] public GameObject enemy;
    [SerializeField] private int spawnIndex;
    [SerializeField] public float spawnAfter;
    [SerializeField] public float spawnEvery;
    [SerializeField] public bool spawnEnemy = true;
    private readonly float range = 20.0f;

    //win condition checks
    private bool usingItems = false;
    private bool usingCheckpoints = false;
    private bool usingSwitches = false;
    private int totalCollectables = 0;
    private PlayerController player;
    GameObject[] switches;
    GameObject[] checkpoints;
    void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Start()
    {
        InvokeRepeating("DroneSpawner", spawnAfter, spawnEvery);

        if (bridge != null && bridge.Count >= 2)
        {
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
            int loadCollectables = totalNormalCollectables;

            for (int i = 0; i <= collectItem.Count - 1; i++)
            {
                if (loadCollectables == 0 || collectItem.Count <= totalNormalCollectables)
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
            int loadAltCollectables = totalAltCollectables;

            for (int k = 0; k <= altCollectItem.Count - 1; k++)
            {
                if (loadAltCollectables == 0 || altCollectItem.Count <= totalAltCollectables)
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

        //final check to ensure that any potential objectives are found.
        switches = GameObject.FindGameObjectsWithTag("Switch");
        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        if (switches != null && switches.Length != 0)
        {
            Debug.Log("counted switches: " + switches.Length);
            usingSwitches = true;
        }

        if (checkpoints != null && checkpoints.Length != 0)
        {
            Debug.Log("counted checkpoints: " + checkpoints.Length);
            usingCheckpoints = true;
        }

        //totals all the required collectables.
        totalCollectables = totalNormalCollectables + totalAltCollectables;
        if(totalCollectables != 0)
        {
            usingItems = true;
        }
    }

    void FixedUpdate()
    {
        CheckWinConditions();
    }

    private void CheckWinConditions()
    {
        if (usingSwitches == true && LevelStats.Switches == switches.Length)
        {
            LevelStats.LevelOneCompleted = true;
            SceneManager.LoadScene("WinScreen");
            Debug.Log("SWITCH CONDITION MET " + LevelStats.Switches);
        }

        if (usingCheckpoints == true && LevelStats.Checkpoints == checkpoints.Length)
        {
            LevelStats.LevelTwoCompleted = true;
            SceneManager.LoadScene("WinScreen");
            Debug.Log("COLLECT CONDITION MET " + LevelStats.Checkpoints);
        }

        if (usingItems == true && LevelStats.Items == totalCollectables)
        {
            LevelStats.LevelThreeCompleted = true;
            SceneManager.LoadScene("WinScreen");
            Debug.Log("ITEM CONDITION MET " + LevelStats.Items);
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
