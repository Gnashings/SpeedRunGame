using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchScipt : MonoBehaviour
{
    Animator anim;
    public GameObject director;
    public GlobalStatus globalStatus;

    void Start()
    {
        anim = GetComponent<Animator>();
        director = GameObject.Find("GameDirector");
        globalStatus = director.GetComponent<GlobalStatus>();
    }

    public void Confirm()
    {
        anim.Play("TurnOn");
        director.BroadcastMessage("CountSwitches");
    }
}
