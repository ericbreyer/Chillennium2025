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
        rb.velocity = new Vector2((target_x - gameObject.transform.position.x > 0 ? walkSpeed : -1 * walkSpeed), rb.velocity.y);
        facingDir = rb.velocity.x < 0 ? -1 : 1;
        //Debug.Log("Raycast loacation: " + transform.position + " direction: " +  Vector2.right * facingDir + " Length: " + facingDir * walkSpeed * jumpTime);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * facingDir, walkSpeed * jumpTime + 0.5f, groundMask);
        Debug.DrawRay(transform.position, Vector2.right * facingDir, Color.red, walkSpeed * jumpTime + 0.5f);
        if (hit.collider != null)
        {
            Debug.Log("we hit something with ray");
            Debug.Log(hit.collider.gameObject.name);
        }
        if (hit.collider != null && isGrounded())
        {
            Debug.Log("Jumping");
            Collider2D col = hit.collider;
            //Debug.Log(col.gameObject.name);
            Jump(col.bounds.max.y);
        }

        if (fov.visible)
        {
            currentState = State.Spotted;
            
        }

        if(Mathf.Pow(transform.position.x, 2) >=  Mathf.Pow(target_x, 2) && transform.position.x * target_x >= 0)
        {
            Destroy(gameObject);
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
