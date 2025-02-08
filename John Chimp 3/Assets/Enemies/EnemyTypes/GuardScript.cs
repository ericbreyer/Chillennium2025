using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardScript : Enemy
{



    public override void IdleBehavior()
    {
        if (fov.visible)
        {
            currentState = State.Spotted;
        }
    }


    public override void SpottedBehavior()
    {
        Debug.Log("Shoot");
        //gun.shoot();
        //target_x = fov.targetLocation.x;
        if (!fov.visible)
        {
            pursuit_time = 0;
            currentState = State.Idle;
        }
    }

    public override void PursuitBehavior()
    {
        pursuit_time += Time.deltaTime;
        if(pursuit_time > pursuit_timeout)
        {
            currentState = State.Idle;
        }

        fov.setRotation(0);
       
        if(fov.visible)
        {
            currentState = State.Spotted;
        }

        moveToTarget(target_x);

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
