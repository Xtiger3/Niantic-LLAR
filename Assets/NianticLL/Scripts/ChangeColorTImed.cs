using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorTImed : MonoBehaviour
{
    public float colorChangeSpeed = 1.0f; // Speed at which the color changes
    private float hue = 0.0f; // Initial hue value

    void Start()
    {
        hue = Random.value;
    }

    void Update()
    {
        // Update the hue value based on time
        hue += colorChangeSpeed * Time.deltaTime;

        // Keep the hue value within the range [0, 1]
        hue = Mathf.Repeat(hue, 1.0f);

        // Convert HSV to RGB to get the current color
        Color currentColor = Color.HSVToRGB(hue, 1.0f, 1.0f);

        // Apply the color to your object's material or sprite renderer
        GetComponentInChildren<Renderer>().material.color = currentColor;
    }
}
