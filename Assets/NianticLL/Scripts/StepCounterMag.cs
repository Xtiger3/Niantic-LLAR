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
    private Pedometer pedometer;

    void Start()
    {
        pedometer = new Pedometer(OnStep);
        // Reset UI
        OnStep(0, 0);

    }

    private void OnStep(int steps, double distance)
    {
        StepCounterDisplay.text = steps.ToString();
        DistCounterDisplay.text = (distance * 3.28084).ToString("F2") + " ft";
    }

    private void OnDisable()
    {
        // Release the pedometer
        pedometer.Dispose();
        pedometer = null;
    }
}
