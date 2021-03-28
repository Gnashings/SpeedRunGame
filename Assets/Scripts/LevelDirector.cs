using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Reports changes Global Status
/// Handles level loading based on information from GlobalStatus
/// </summary>
public class LevelDirector : MonoBehaviour
{
    [SerializeField] public List<Bridge> bridge;
    [SerializeField] public int bridgeIndex;

    [SerializeField] public List<Collectible> collectItem;
    [SerializeField] public int collectionCount;

    void Start()
    {
        if (bridge != null && bridge.Count >= 2)
        { 
            //range must be zero to the bridge.length, as the final number isn't inclusive
            bridgeIndex = Random.Range(0, 2);
            Debug.Log("Removing Bridge at index: " + bridgeIndex);
            bridge[bridgeIndex].gameObject.SetActive(false);
        }

        if (collectItem != null && collectItem.Count >= 2)
        {
            //range must be zero to the collectItem.length, as the final number isn't inclusive
            collectionCount = Random.Range(0, 2);
            {
               
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
