using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.SceneManagement;

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

    //public GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        //Instantiate(enemy, new Vector3(0, 0, 0), Quaternion.identity);
        //InvokeRepeating("DroneSpawner", 1.0f, 0.5f);
    }
    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }

        LevelOne = false;
        LevelTwo = false;
        LevelThree = false;
    }

    void OnLevelWasLoaded(int level)
    {
        //Hub Level
        if (level == 0)
        {
            Debug.Log("Level Zero");
            //TODO Impliment shutoff for level director
            if (LevelOne == true)
            {
                //TODO Impliment level lockout for level director
            }
            if (LevelTwo == true)
            {
                //TODO Impliment level lockout for level director
            }
            if (LevelThree == true)
            {
                //TODO Impliment level lockout for level director
            }
        }   

        //Level One
        if (level == 1)
        {

        }

        //Level Two
        if (level == 2)
        {

        }

        //Level Three
        if (level == 3)
        {

        }
    }

}
