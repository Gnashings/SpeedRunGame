using System.Collections;
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
        if (LevelOne)
        {
            SceneManager.LoadScene("WinScreen");
        }
        if (LevelTwo)
        {
            SceneManager.LoadScene("WinScreen");
        }
        if (LevelThree)
        {
            SceneManager.LoadScene("WinScreen");
        }
    }

    void OnSceneLoaded()
    {
        if (currentScene.name.Equals("HubLevel"))
        {
            if (LevelOne == true)
            {
                Debug.Log("TODO LEVEL ONE");
            }
            if (LevelTwo == true)
            {
                Debug.Log("TODO LEVEL TWO");
            }
            if (LevelThree == true)
            {
                Debug.Log("TODO LEVEL THREE");
            }
        }
    }

    void CountSwitches()
    {

        playerInfo.canPhase = true;

        if (playerInfo.switchCount == 3)
        {
            LevelOne = true;
        }
        else
            playerInfo.switchCount++;
    }

    void CountCollectables()
    {
        if(playerInfo.itemCount == 4)
        {
            LevelTwo = true;
        }
        else
            playerInfo.itemCount++;
    }

    void CountCheckpoints()
    {
        if (playerInfo.objCount == 4)
        {
            LevelThree = true;
        }
        else
            playerInfo.objCount++;
    }

}
