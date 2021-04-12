using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public GameObject director;
    public GlobalStatus globalStatus;
    // Start is called before the first frame update
    void Start()
    {
        director = GameObject.Find("GameDirector");
        globalStatus = director.GetComponent<GlobalStatus>();
    }

    public void CountUp()
    {
        director.BroadcastMessage("CountCollectables");
    }

    private void OnDestroy()
    {
        //director.BroadcastMessage("CountCollectables");
    }
}
