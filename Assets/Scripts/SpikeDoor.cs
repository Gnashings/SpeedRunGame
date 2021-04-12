using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeDoor : MonoBehaviour
{
    public GameObject target;
    public Collider killBox;
    public float rotationSpeed;


    private void Start()
    {
        //target = GameObject.Find("Pivot").GetComponent<GameObject>();
        killBox = GetComponent<Collider>();
    }

    void FixedUpdate()
    {
        transform.RotateAround(target.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            PlayerController playerInfo = other.transform.GetComponent<PlayerController>();
            playerInfo.YouDie();
        }
    }
}
