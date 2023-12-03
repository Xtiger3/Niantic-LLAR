using TMPro;
using UnityEngine;

public class TestWalk : MonoBehaviour
{
    //private const float StepThreshold = 1.45f; // Adjust threshold
    //private bool isStepDetected = false;
    //private int stepCount = 0;

    public TextMeshProUGUI tt;


    void Update()
    {
        //Vector3 acceleration = Input.acceleration;

        //float accelerationMagnitude = acceleration.magnitude;
        ////Debug.Log(accelerationMagnitude);

        //if (accelerationMagnitude > StepThreshold)
        //{
        //    if (!isStepDetected)
        //    {
        //        stepCount++;
        //        isStepDetected = true;
        //    }
        //}
        //else
        //{
        //    isStepDetected = false;
        //}

        UpdateUI();
    }

    void UpdateUI()
    {
        tt.text = "STEPS: " + VocabularySet.Instance.stepCount;
    }
}