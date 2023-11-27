using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PedometerU;
using TMPro;

public class StepCounterMag : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI StepCounterDisplay;
    public TextMeshProUGUI DistCounterDisplay;

    void Start()
    {
        var pedometer = new Pedometer(OnStep);

        
    }

    void OnStep(int steps, double distance)
    {
        StepCounterDisplay.text = steps.ToString();
        DistCounterDisplay.text = (distance * 3.28084).ToString("F2") + " ft";
    }
}
