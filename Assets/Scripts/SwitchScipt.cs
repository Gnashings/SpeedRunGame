using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchScipt : MonoBehaviour
{
    Animator anim;
    public GameObject director;
    public GlobalStatus globalStatus;
    bool switchedOn = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        director = GameObject.Find("GameDirector");
        globalStatus = director.GetComponent<GlobalStatus>();
    }

    public void Confirm()
    {
        if(switchedOn == false)
        {
            anim.Play("TurnOn");
            director.BroadcastMessage("CountSwitches");
        }

        switchedOn = true;
    }
}
