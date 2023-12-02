using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    private float startYPos;
    public float bounceSpeed = 5f;
    public float bounceHeight = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        startYPos = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, startYPos + (Mathf.Sin(Time.time * bounceSpeed) * bounceHeight), transform.position.z);
    }
}
