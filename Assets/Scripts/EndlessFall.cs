using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EndlessFall : MonoBehaviour
{

    public Rigidbody boxbody;
    public float positionY = 30;
    public float positionReset = 0;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody boxbody = gameObject.GetComponent<Rigidbody>();
        
    }

    private void FixedUpdate()
    {
        if(boxbody.transform.position.y <= positionReset)
        {
            boxbody.transform.position = new Vector3(boxbody.transform.position.x, positionY, boxbody.transform.position.z);
            Debug.Log("RESET");
        }

    }

}
