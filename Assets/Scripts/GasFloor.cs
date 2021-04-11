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

    void Update()
    {
        transform.root.Translate(Vector3.down * -speed * 0.01f);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerInfo = other.transform.GetComponent<PlayerController>();
        playerInfo.YouDie();
    }

}
