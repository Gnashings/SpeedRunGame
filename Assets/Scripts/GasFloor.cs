using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasFloor : MonoBehaviour
{
    public float speed;
    public Collider killBox;

    void Awake()
    {
        killBox = GetComponent<Collider>();    
    }

    void FixedUpdate()
    {
        transform.root.Translate(Vector3.down * -speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerInfo = other.transform.GetComponent<PlayerController>();
        playerInfo.YouDie();
    }

}
