using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopSound : MonoBehaviour
{
    void Awake()
    {
        GameObject A = GameObject.FindGameObjectWithTag("music");
        Destroy(A);
    }
}
