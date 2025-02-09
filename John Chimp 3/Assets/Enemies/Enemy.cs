using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{

    public enum State
    {
        Idle,
        Spotted,
        Pursuit, 
        Death, 
        Custom1, 
        Custom2
    };


    public State currentState = State.Idle;
    public State startState = State.Idle;
    public GameObject fovObj;
    public FOV fov;
    public float rotate_speed;
    public Gun gun;
    public float pursuit_timeout = 2;
    public float pursuit_time = 0;
    
    //stuff related to the movement portion of the script
    public Rigidbody2D rb;
    public int facingDir;
    public float walkSpeed = 2;
    public float jumpTime = 0.3f;
    public LayerMask groundMask;
    public int initialDir = 1;
    public Vector3 startLocalScale;
    public float target_x;


    public virtual void IdleBehavior()
    {
    }


    public virtual void SpottedBehavior()
    {

    }

    public virtual void PursuitBehavior()
    {

    }

    public virtual void DeathBehavior()
    {
        Destroy(gameObject);
    }

    public float timeToOut = 2;
    public bool fadingOut = false;
    public bool finishedFadingOut = false;
    IEnumerator fadeout()
    {
        fadingOut = true;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        for (int i = 0; i < 100; i++)
        {
            sr.color = new Color(1, 1, 1, (100f - i) / 100f);
            yield return new WaitForSeconds(timeToOut / 100);
        }
        finishedFadingOut = true;
        yield return null;
    }

    public virtual void CustomBehavior()
    {
        if (finishedFadingOut)
        {
            Destroy(gameObject);
        }

        //add peace out animation
        if (!fadingOut)
            StartCoroutine(fadeout());
        return;
    }

    public virtual void Custom2Behavior()
    {
        return;
    }

    public virtual void Start()
    {
        fovObj = transform.Find("FOV").gameObject;
        fov = fovObj.GetComponent<FOV>();
        currentState = startState;
        gun = transform.GetComponentInChildren<Gun>();


        rb = GetComponent<Rigidbody2D>();
        transform.localScale = new Vector3(facingDir * transform.localScale.x, transform.localScale.y, 1);
        startLocalScale = transform.localScale;
        initialDir = (transform.localScale.x > 0) ? 1 : -1;
        facingDir = initialDir;

    }

    public void debugRotation()
    {
        bool left = Input.GetKey(KeyCode.Z);
        bool right = Input.GetKey(KeyCode.X);
        //float xIn = Input.GetAxis("Horizontal");
        float xIn = (left) ? -1 : right ? 1 : 0;
        transform.Rotate(0, 0, -1 * xIn * rotate_speed);
    }


    public bool isGrounded()
    {
        // Adjust the radius for overlap detection to check a broader area under the character
        float checkRadius = 0.1f; // Adjust the radius as necessary
        Vector2 position = transform.position;
        Vector2 groundCheckPosition = new Vector2(position.x, position.y - 1f); // Slightly below the player

        // Check if there's any collider below the character within the radius
        Collider2D hit = Physics2D.OverlapCircle(groundCheckPosition, checkRadius, groundMask); // groundLayerMask defines which layers count as ground

        return hit != null;
    }

    public float CalculateJumpVelocity(float peakHeight, float time)
    {
        float gravity = -(2 * peakHeight / (time * time));
        rb.gravityScale = gravity / Physics2D.gravity.y;
        return (-1 * gravity * time);
    }

    public void Jump(float ypos)
    {
        rb.velocity = new Vector2(rb.velocity.x, CalculateJumpVelocity(ypos - transform.position.y + 1.5f, jumpTime));
    }


    public bool moveToTarget(float x)
    {
        if(x > transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        rb.velocity = new Vector2((x - gameObject.transform.position.x > 0 ? walkSpeed : -1 * walkSpeed), rb.velocity.y);
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
        if (Mathf.Abs(transform.position.x - x) < 0.2)
        {
            return true;
        }
        else
            return false;
        //if (Mathf.Pow(transform.position.x, 2) >= Mathf.Pow(target_x, 2) && transform.position.x * target_x >= 0)
        //{
        //    Destroy(gameObject);
        //}
    }    



    private void FixedUpdate()
    {

        debugRotation();
        switch (currentState)
        {
            case State.Idle:
                IdleBehavior(); break;
            case State.Spotted:
                SpottedBehavior(); break;
            case State.Pursuit:
                PursuitBehavior(); break;
            case State.Custom1:
                CustomBehavior(); break;

        }
    }

}
