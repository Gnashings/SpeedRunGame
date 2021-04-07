using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public GameObject gamedir;
    // Start is called before the first frame update
    void Start()
    {
        gamedir = GameObject.Find("GameDirector");
    }

    private void OnDestroy()
    {
        Debug.LogWarning("Not reported to the global status.");
        gamedir.BroadcastMessage("Poggo", true);
    }
}
