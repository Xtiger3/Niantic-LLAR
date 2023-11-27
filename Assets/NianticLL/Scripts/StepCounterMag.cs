using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PedometerU;

public class StepCounterMag : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var pedometer = new Pedometer(OnStep);

        void OnStep(int steps, double distance)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
