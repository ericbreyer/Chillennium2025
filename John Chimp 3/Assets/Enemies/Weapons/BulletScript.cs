using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    float lifeTime = 2;
    float aliveTime = 0;


    private void FixedUpdate()
    {
        aliveTime += Time.deltaTime;
        if(lifeTime < aliveTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider other)
    {
        if(other.GetComponent<playerMovement>() != null)
        {
            other.GetComponent<playerMovement>().die();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
