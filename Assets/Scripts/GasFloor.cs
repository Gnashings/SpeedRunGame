using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasFloor : MonoBehaviour
{
    public float speed;

    // Update is called once per frame
    void Update()
    {
        transform.root.Translate(Vector3.down * speed * 0.01f);

        //todo collectable.
    }
}
