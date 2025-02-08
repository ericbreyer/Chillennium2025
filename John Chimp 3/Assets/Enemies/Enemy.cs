using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float rotate_speed;
    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        float xIn = Input.GetAxis("Horizontal");
        transform.Rotate(0, 0, -1 * xIn * rotate_speed);
    }

}
