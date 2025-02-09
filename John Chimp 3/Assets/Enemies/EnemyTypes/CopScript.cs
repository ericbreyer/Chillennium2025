using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CopScript : Enemy
{
    
    public float d;
    public GameObject target;
    public Animator animationController;



    public override void Start()
    {
        base.Start();
        animationController = GetComponent<Animator>();
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





    public bool fadingOut2 = false;
    public bool finishedFadingOut2 = false;
    public IEnumerator fadeout2()
    {
        fadingOut2 = true;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Destroy(transform.Find("Gun").gameObject);
        SpriteRenderer sr2 = GetComponentInChildren<SpriteRenderer>();
        for (int i = 0; i < 100; i++)
        {
            sr2.color = new Color(1, 1, 1, (100f - i) / 100f);
            sr.color = new Color(1, 1, 1, (100f - i) / 100f);
            yield return new WaitForSeconds(timeToOut / 100);
        }
        finishedFadingOut2 = true;
        yield return null;
    }

    public override void CustomBehavior()
    {
        animationController.SetInteger("State", 1);
        if (finishedFadingOut2)
        {
            Destroy(gameObject);
        }

        //add peace out animation
        if (!fadingOut2)
            StartCoroutine(fadeout2());
        return;
    }

    public override void IdleBehavior()
    {

        animationController.SetInteger("State", 0);
        if(moveToTarget(target_x))
        {

            currentState = State.Custom1;
            
        }
        
        
        if(fov.visible)
        {
            currentState = State.Spotted;
        }

    }

    public override void SpottedBehavior()
    {
        animationController.SetInteger("State", 1);
        gun.shoot(fov.dirToTarget);
        if(!fov.visible)
        {
            currentState = State.Idle;
        }

    }



}
