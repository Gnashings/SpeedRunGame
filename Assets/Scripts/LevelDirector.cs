using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDirector : MonoBehaviour
{
    [SerializeField] public List<Bridge> bridge;
    [SerializeField] public int bridgeIndex;

    void Start()
    {
        if (bridge != null && bridge.Count >= 2)
        { 
            //range must be zero to the bridge.length, as the final number isn't inclusive
            bridgeIndex = Random.Range(0, 2);
            Debug.Log("Removing Bridge at index: " + bridgeIndex);
            bridge[bridgeIndex].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
