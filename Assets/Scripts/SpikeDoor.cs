using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeDoor : MonoBehaviour
{
    public GameObject target;

    private void Start()
    {
        //target = GameObject.Find("Pivot").GetComponent<GameObject>();
    }

    void FixedUpdate()
    {
        transform.RotateAround(target.transform.position, Vector3.up, 20 * Time.deltaTime);
    }
}
