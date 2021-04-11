using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private CharacterController laser;
    private bool movingUp = false;

    [Header("Movement")]
    public float movementSpeed;
    public float upperLimit;
    public float lowerLimit;
    public bool canMove = false;
    [Header("Requirements")]
    public LineRenderer shoot;
    public Transform firepoint;

    Vector3 originalPosition;
    public Collider playerCol;
    RaycastHit hit;


    private void Awake()
    {
        laser = gameObject.GetComponent<CharacterController>();
        shoot = GameObject.Find("Line").GetComponent<LineRenderer>();
        originalPosition = transform.root.position;
        if (movementSpeed == 0)
        {
            movementSpeed = 1;
        }
        shoot.enabled = false;
    }

    private void FixedUpdate()
    {
        if (canMove == true)
        {
            if (movingUp == true)
            {
                if (transform.root.position.y > originalPosition.y + upperLimit)
                {
                    movingUp = false;
                }
                else
                    MoveUp();
            }
            else if (movingUp == false)
            {
                if (transform.root.position.y < originalPosition.y - lowerLimit)
                {
                    movingUp = true;
                }
                else
                    MoveDown();
            }
        }
    }

    private void Update()
    {
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
        Vector3 fwd = transform.TransformDirection(Vector3.up);
        bool HitDetect = Physics.BoxCast(playerCol.bounds.center, transform.localScale, fwd, out hit, transform.rotation, 100);

        if (HitDetect)
        {
            shoot.enabled = true;
            //Debug.DrawRay(transform.position, fwd * hit.distance, Color.red);
            shoot.SetPosition(0, transform.position);
            shoot.SetPosition(1, hit.point);
            Debug.Log("hit");
            Gizmos.DrawRay(playerCol.transform.position, fwd * hit.distance);
            Gizmos.DrawWireCube(playerCol.transform.position + fwd * hit.distance, transform.localScale);
            if (hit.collider.tag.Equals("Player"))
            {

            }
        }    

        /*
         *         if (Physics.Raycast(transform.position, fwd, out hit, 100))
        {
            //if(hit.collider)
            //{
    
                shoot.enabled = true;
                Debug.DrawRay(transform.position, fwd * hit.distance, Color.red);
                shoot.SetPosition(0, transform.position);
                shoot.SetPosition(1, hit.point);
                Debug.Log("hit");
            if (hit.collider.tag.Equals("Player"))
                {
                    
                }
            //}
        }
         */
    }
}
