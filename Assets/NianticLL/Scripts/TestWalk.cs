using TMPro;
using UnityEngine;

public class TestWalk : MonoBehaviour
{
    private const float StepThreshold = 1.0f; // Adjust this threshold according to your needs
    private bool isStepDetected = false;
    private int stepCount = 0;

    public TextMeshProUGUI tt;


    void Update()
    {
        Vector3 acceleration = Input.acceleration;

        // Calculate the magnitude of the acceleration vector
        float accelerationMagnitude = acceleration.magnitude;

        // Check if a step is detected based on the threshold
        if (accelerationMagnitude > StepThreshold)
        {
            if (!isStepDetected)
            {
                // Increment step count when a step is detected
                stepCount++;
                isStepDetected = true;
            }
        }
        else
        {
            isStepDetected = false;
        }

        // Update your UI or perform other actions based on the step count
        UpdateUI();
    }

    void UpdateUI()
    {
        // Implement code to update your UI or perform actions based on step count
        // For example, you can display the step count in a Text component
        // GetComponent<Text>().text = "Steps: " + stepCount;
        tt.text = "Steps: " + stepCount;
    }
}