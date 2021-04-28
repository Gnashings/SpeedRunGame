using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    public float Health;
    public float TotalHealth;


    void Awake()
    {
        if (LevelStats.FirstSpawn == false)
        {
            FirstSpawnInit();
        }
        else if(LevelStats.FirstSpawn == true)
        {
            GetSavedHealth();
        }
        
    }

    void Start()
    {
        UpdateHealth();
    }

    void Update()
    {
        
    }

    void FirstSpawnInit()
    {
        LevelStats.FirstSpawn = true;
        Health = 100f;
        TotalHealth = 100f;
        LevelStats.Health = Health;
        LevelStats.TotalHealth = TotalHealth;
    }

    public void GetSavedHealth()
    {
        Health = LevelStats.Health;
        TotalHealth = LevelStats.TotalHealth;
    }

    public void UpdateHealth()
    {
        LevelStats.Health = Health;
        LevelStats.TotalHealth = TotalHealth;
    }

    void OnDestroy()
    {
        LevelStats.Health = Health;
        LevelStats.TotalHealth = TotalHealth;
        UnityEngine.Debug.Log("lmfao");
    }
}
