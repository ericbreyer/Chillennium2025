using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{

    Animator animationController;
    Rigidbody2D rb;
    playerMovement pmScript;

    // Start is called before the first frame update
    void Start()
    {
        animationController = GetComponent<Animator>();
        pmScript = GetComponent<playerMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(pmScript.isGrounded() && rb.velocity.x > 0)
        {
            animationController.SetInteger("State", 1);
        }
        else
        {
            animationController.SetInteger("State", 0);
        }
    }
}
