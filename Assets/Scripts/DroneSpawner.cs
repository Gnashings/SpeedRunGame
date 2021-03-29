using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneSpawner : MonoBehaviour
{
    public GameObject enemy;
    [SerializeField] public float spawnAfter;
    [SerializeField] public float spawnEvery;
    public bool spawnEnemy = true;
    private float range = 20f;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("DroneSpawn", spawnAfter, spawnEvery);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DroneSpawn()
    {
        if (spawnEnemy == true)
        {
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
            Debug.Log("SPAWNABLE");
            finalPosition = hit.position;
            return finalPosition;
        }
        return finalPosition;
    }
}
