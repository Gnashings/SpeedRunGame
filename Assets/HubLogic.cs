using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HubLogic : MonoBehaviour
{
    bool lvlOneDoor;
    bool lvlTwoDoor;
    bool lvlThreeDoor;

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

        lvlOneDoor = false;
        lvlTwoDoor = false;
        lvlThreeDoor = false;
        
        if(globalStatus.LevelOne == true)
        {
            door1.SetActive(false);
        }
        if (globalStatus.LevelTwo == true)
        {
            door2.SetActive(false);
        }
        if (globalStatus.LevelThree == true)
        {
            door3.SetActive(false);
        }
    }


}
