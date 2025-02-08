using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CopScript : Enemy
{
    
    public float d;
    public GameObject target;



    public override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        transform.localScale = new Vector3(facingDir * transform.localScale.x, transform.localScale.y, 1);
        startLocalScale = transform.localScale;
        initialDir = target.transform.position.x > transform.position.x ? 1 : -1;
        facingDir = initialDir;
        if(target)
        {
            target_x = target.transform.position.x;
        }
        


    }

    




    public override void IdleBehavior()
    {
        moveToTarget(target_x);
        if(fov.visible)
        {
            currentState = State.Spotted;
        }

    }

    public override void SpottedBehavior()
    {
        gun.shoot();
        if(!fov.visible)
        {
            currentState = State.Idle;
        }

    }



}
