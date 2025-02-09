using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardScript : Enemy
{

    public Animator animationController;

    public override void Start()
    {
        animationController = GetComponent<Animator>();
    }

    public override void IdleBehavior()
    {
        animationController.SetInteger("State", 1);
        if (fov.visible)
        {
            currentState = State.Spotted;
        }
    }


    public override void SpottedBehavior()
    {
        animationController.SetInteger("State", 1);
        gun.shoot(fov.dirToTarget);
        target_x = fov.targetLocation.x;
        if (!fov.visible)
        {
            pursuit_time = 0;
            currentState = State.Pursuit;
        }
    }

    public override void PursuitBehavior()
    {
        animationController.SetInteger("State", 0);
        pursuit_time += Time.deltaTime;
        if(pursuit_time > pursuit_timeout)
        {
            currentState = State.Idle;
        }
       
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
