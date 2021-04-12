﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handles and tracks reports by level director.
/// Keeps information between scenes.
/// No other script should interact.
/// </summary>
public class GlobalStatus : MonoBehaviour
{
    public static GlobalStatus Instance;

    //level completion checks
    public bool LevelOne;
    public bool LevelTwo;
    public bool LevelThree;

    public Scene currentScene;
    Text confirm;
    GameObject player;
    PlayerController playerInfo;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        player = GameObject.Find("Player");
        playerInfo = player.GetComponent<PlayerController>();
        LevelOne = false;
        LevelTwo = false;
        LevelThree = false;
    }

    void Update()
    {
        if (LevelOne == true)
        {
            //SceneManager.LoadScene("WinScreen");
        }
        if (LevelTwo == true)
        {
            //SceneManager.LoadScene("WinScreen");
        }
        if (LevelThree == true)
        {
            SceneManager.LoadScene("WinScreen");
        }
    }

    void CountSwitches()
    {

        playerInfo.canPhase = true;

        if (playerInfo.switchCount == 2)
        {
            LevelOne = true;
            SceneManager.LoadScene("WinScreen");

        }
        else
            playerInfo.switchCount++;
        Debug.Log(playerInfo.switchCount);
    }

    void CountCollectables()
    {
        if(playerInfo.itemCount == 3)
        {
            LevelTwo = true;
            SceneManager.LoadScene("WinScreen");

        }
        else
            playerInfo.itemCount++;
    }

    void CountCheckpoints()
    {
        if (playerInfo.objCount == 3)
        {
            LevelThree = true;
            SceneManager.LoadScene("WinScreen");
        }
        else
            playerInfo.objCount++;
    }

}
