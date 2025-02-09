using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waggle : MonoBehaviour
{
    public float wiggleAmount = 0.5f; // Maximum translation in units (up/down)
    public float wiggleSpeed = 3f;    // Speed of the wiggle
    public Vector3 basepos;

    private void Start() {
        basepos = transform.position;
    }

    void Update() {
        // Calculate the wiggle offset using sine wave for smooth motion
        float wiggleOffset = Mathf.Max(wiggleAmount / 2f, Mathf.Sin(Time.time * wiggleSpeed) * wiggleAmount) - wiggleAmount/2f;

        // Apply the calculated offset to the Y position
        transform.position = new Vector3(basepos.x, basepos.y - wiggleOffset, basepos.z);
    }
}
