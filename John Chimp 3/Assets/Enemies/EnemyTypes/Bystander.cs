using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bystander : Enemy
{
    public GameObject exitSpot;
    public GameObject SurpriseBlock;
    public bool bulletHeard = false;
    public GameObject copPrefab;
    float x_target;

    public override void Start()
    {
        base.Start();
        x_target = exitSpot.transform.position.x;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            signalBullet();
        }
    }
    public void signalBullet()
    {
        bulletHeard = true;
    }

    IEnumerator SurprisedLook()
    {
        SurpriseBlock.SetActive(true);
        yield return new WaitForSeconds(1);
        Destroy(SurpriseBlock);
        SurpriseBlock = null;
        GetComponentInChildren<TearGenerator>().startTears();
        currentState = State.Spotted;


    }

    public override void IdleBehavior()
    {
        if (bulletHeard)
        {
            StartCoroutine(SurprisedLook());
        }
            
    }


    public override void SpottedBehavior()
    {
        if (moveToTarget(x_target))
        {
            copPrefab.SetActive(true);
            currentState = State.Custom1; //just don't do antyhing after this
        }
        
    }

    public override void CustomBehavior()
    {
        base.CustomBehavior();
    }


}
