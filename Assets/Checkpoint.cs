using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject director;
    public GlobalStatus globalStatus;
    void Start()
    {
        director = GameObject.Find("GameDirector");
        globalStatus = director.GetComponent<GlobalStatus>();
    }

    private void OnDestroy()
    {
        director.BroadcastMessage("CountCheckpoints");
    }
}
