using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private CharacterController laser;
    public float movementSpeed;
    public float upperLimit;
    public float lowerLimit;
    public bool movingUp = false;
    Vector3 originalPosition;

    private void Awake()
    {
        laser = gameObject.GetComponent<CharacterController>();
        originalPosition = transform.root.position;

        if(movementSpeed == 0)
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
                Debug.Log("POGGERS");
            }
            else
                MoveUp();
        }
        else if (movingUp == false)
        {
            if(transform.root.position.y < originalPosition.y - lowerLimit)
            {
                movingUp = true;
                Debug.Log("NOT POGGERS");
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

}
