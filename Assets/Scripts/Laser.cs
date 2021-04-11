using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private CharacterController laser;
    public float movementSpeed;
    public float upperLimit;
    public float lowerLimit;
    private bool movingUp = false;
    public float MaxDistance;
    Vector3 originalPosition;
    Collider playerCol;
    RaycastHit hit;
    bool hitDetect;
    private void Awake()
    {
        laser = gameObject.GetComponent<CharacterController>();
        originalPosition = transform.root.position;
        if (movementSpeed == 0)
        {
            movementSpeed = 1;
        }
    }

    private void FixedUpdate()
    {
        if (movingUp == true)
        {
            if(transform.root.position.y > originalPosition.y + upperLimit)
            {
                movingUp = false;
            }
            else
                MoveUp();
        }
        else if (movingUp == false)
        {
            if(transform.root.position.y < originalPosition.y - lowerLimit)
            {
                movingUp = true;
            }
            else
                MoveDown();
        }
    }
    
    private void MoveDown()
    {
        transform.root.Translate(Vector3.down * movementSpeed * 0.01f);
    }

    private void MoveUp()
    {
        transform.root.Translate(Vector3.up * movementSpeed * 0.01f);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(transform.position, fwd, out hit, 100))
        {
            if(hit.collider.tag.Equals("Player"))
            {
                Debug.Log("HJIT");
                Debug.DrawRay(transform.position, fwd * hit.distance, Color.red);
                Debug.Break();
            }
        }
    }
}
