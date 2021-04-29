using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeWall : MonoBehaviour
{
    private CharacterController spikeWall;
    private bool movingForward = true;

    public float movementSpeed;
    public float forwardLimit;
    
    public bool canMove = false;
    public Collider killBox;
    
    Vector3 originalPosition;

    private void Awake()
    {
        spikeWall = gameObject.GetComponent<CharacterController>();
        originalPosition = transform.root.position;
        
    }

    private void Start()
    {
        killBox = GetComponent<Collider>();
    }

    private void FixedUpdate()
    {
        if (canMove == true)
        {
            if (movingForward == true)
            {
               if (transform.root.position.z<originalPosition.z-forwardLimit)
               {
                   movingForward = false;
               }
               else
                 MoveForward();
            }
    
        }
    }

    private void MoveForward()
    {
        transform.root.Translate(Vector3.left*movementSpeed*Time.deltaTime);
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
