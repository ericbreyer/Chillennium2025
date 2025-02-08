using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{



    private void FixedUpdate()
    {
        float yin = Input.GetAxis("Vertical");
        float xin = Input.GetAxis("Horizontal");
        GetComponent<Rigidbody2D>().velocity = new Vector2(xin * 5, yin * 5);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
