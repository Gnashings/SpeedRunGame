using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    [Header("Health")]
    [SerializeField] public float Health;
    [SerializeField] public float TotalHealth;
    [SerializeField] public float totalHealthDecrease = 10f;
    [SerializeField] public float totalHealthLimit = 90f;
    [SerializeField] public float regenThreshold;
    [SerializeField] float regTimer = 0.0f;
    [SerializeField] float regTime = 5.0f;
    PlayerController player;
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
        player = GetComponent<PlayerController>();
    }

    void Update()
    {
        Regen();
    }

    private void Regen()
    {
        if (Health < regenThreshold)
        {
            if (player.crossed != true)
            {
                if (regTimer >= regTime)
                {
                    Health = regenThreshold;
                }
                regTimer += Time.unscaledDeltaTime;
            }

        }
        else
        {
            regTimer = 0.0f;
        }
    }

    public void HealPlayer(float heal)
    {
        if (Health + heal > TotalHealth)
        {
            Health = TotalHealth;
        }
        else
            Health += heal;
    }

    public void StimPlayer(float stim)
    {
        TotalHealth += stim;
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
        TotalHealth = LevelStats.TotalHealth;
        Health = LevelStats.TotalHealth;
    }

    public void UpdateHealth()
    {
        LevelStats.Health = Health;
        LevelStats.TotalHealth = TotalHealth;
    }

    void OnDestroy()
    {
        if (LevelStats.TotalHealth > totalHealthLimit)
        {
            LevelStats.TotalHealth = LevelStats.TotalHealth - totalHealthDecrease;
        }
    }
}
