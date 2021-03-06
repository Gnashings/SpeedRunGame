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
    //public static GlobalStatus Instance;

    //level completion checks
    public bool LevelOne;
    public bool LevelTwo;
    public bool LevelThree;

    public int items;

    public Scene currentScene;
    Text confirm;
    Text itemTotal;
    GameObject player;
    PlayerController playerInfo;

    void Awake()
    {
        /*
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        */
        itemTotal = GameObject.Find("Canvas/Item Total").GetComponent<Text>();
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
            SceneManager.LoadScene("WinScreenLogic");
        }
        if (LevelTwo == true)
        {
            //SceneManager.LoadScene("WinScreenSpeed");
        }
        if (LevelThree == true)
        {
           // SceneManager.LoadScene("WinScreenScout");
        }
    }

    void CountSwitches()
    {

        playerInfo.canPhase = true;

        if (playerInfo.switchCount == 2)
        {
            LevelOne = true;
            SceneManager.LoadScene("WinScreen");
            LevelStats.LevelOneCompleted = true;
        }
        else
            playerInfo.switchCount++;
            itemTotal.text = playerInfo.switchCount.ToString();
            Debug.Log(playerInfo.switchCount);
        
    }

    public void CountCollectables()
    {
        if(playerInfo.itemCount == 3)
        {
            LevelTwo = true;
            SceneManager.LoadScene("WinScreenScout");
            LevelStats.LevelThreeCompleted = true;
        }
        else
            playerInfo.itemCount++;
            itemTotal.text = playerInfo.itemCount.ToString();

            Debug.Log(playerInfo.itemCount);
    }

    void CountCheckpoints()
    {
        if (playerInfo.objCount == 4)
        {
            LevelThree = true;
            SceneManager.LoadScene("WinScreenSpeed");
            LevelStats.LevelTwoCompleted = true;
        }
        else
            playerInfo.objCount++;
    }

}
