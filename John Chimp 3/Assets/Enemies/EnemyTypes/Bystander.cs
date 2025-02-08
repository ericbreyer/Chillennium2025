using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bystander : Enemy
{
    public GameObject exitSpot;
    public bool bulletHeard = false;
    public GameObject copPrefab;
    public GameObject copGoTo;


    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            signalBullet();
        }
    }
    public void signalBullet()
    {
        bulletHeard = true;
    }

    public override void IdleBehavior()
    {
        if (bulletHeard)
            currentState = State.Spotted;
    }

    public override void SpottedBehavior()
    {
        if (moveToTarget(exitSpot.transform.position.x))
        {
            copPrefab.SetActive(true);
            currentState = State.Pursuit; //just don't do antyhing after this
        }
        
    }


}
