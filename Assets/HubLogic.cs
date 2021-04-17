﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HubLogic : MonoBehaviour
{
    public GameObject door1;
    public GameObject door2;
    public GameObject door3;

    public GameObject director;
    public GlobalStatus globalStatus;

    private void Awake()
    {
        door1 = GameObject.Find("Level One Door");
        door2 = GameObject.Find("Level Two Door");
        door3 = GameObject.Find("Level Three Door");
        director = GameObject.Find("GameDirector");
        globalStatus = director.GetComponent<GlobalStatus>();

        ShutLevelDoors();
        ResetPlayerCollectibles();
    }

    private void ShutLevelDoors()
    {
        if (LevelStats.LevelOneCompleted == true)
        {
            door1.SetActive(false);
        }
        if (LevelStats.LevelTwoCompleted == true)
        {
            door2.SetActive(false);
        }
        if (LevelStats.LevelThreeCompleted == true)
        {
            door3.SetActive(false);
        }
    }

    //sees if the player completed a level or not
    private void ResetPlayerCollectibles()
    {
        if (LevelStats.LevelOneCompleted == false)
        {
            LevelStats.Switches = 0;
        }
        if (LevelStats.LevelTwoCompleted == false)
        {
            LevelStats.Checkpoints = 0;
        }
        if (LevelStats.LevelThreeCompleted == false)
        {
            LevelStats.Items = 0;
        }

    }
}
