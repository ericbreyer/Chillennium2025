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
        gun.shoot();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
