using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject gamedir;

    void Start()
    {
        gamedir = GameObject.Find("GameDirector");
    }

    private void OnDestroy()
    {
        gamedir.BroadcastMessage("CountCheckpoints");
    }
}
