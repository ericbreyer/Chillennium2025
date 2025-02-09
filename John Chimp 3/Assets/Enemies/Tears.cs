using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tears : MonoBehaviour
{
    public float lifespan = 1.5f;
    public float anglediff = 25f;
    public float aliveTime = 0;
    public float initVY = 10;
    public float initVX = 2;
    public float initVariance = 4;
    void Start()
    {
        
        
        GetComponent<Rigidbody2D>().velocity = new Vector2(initVX, initVY);
       
        GetComponent<Rigidbody2D>().angularVelocity = anglediff; 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        aliveTime += Time.fixedDeltaTime;
        if(aliveTime > lifespan)
        {
            Destroy(gameObject);
        }
    }
}
