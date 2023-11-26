using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity);
        transform.Rotate(0f, 100 * Time.deltaTime, 0f, Space.Self);
    }
}
