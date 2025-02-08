using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waggle : MonoBehaviour
{
    public float wiggleAmount = 4; // Maximum rotation in degrees
    public float wiggleSpeed = 3;   // Speed of the wiggle

    void Update() {
        // Calculate the wiggle rotation using sine wave for smooth motion
        float wiggleRotation = Mathf.Sin(Time.time * wiggleSpeed) * wiggleAmount;

        // Apply the calculated rotation around the Z-axis
        transform.rotation = Quaternion.Euler(0, 0, wiggleRotation);
    }
}
