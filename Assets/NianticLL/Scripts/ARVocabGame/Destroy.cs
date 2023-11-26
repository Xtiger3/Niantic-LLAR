using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public float deathTime = 2f;

    private void Start()
    {
        Destroy(gameObject, deathTime);
    }
}
