using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float walkSpeed;
    public LayerMask groundMask;
    public float jumpTime;
    private int startBool = 0;
    private int moving = 0;
    public int facingDir;
    private int initialDir;
    private Rigidbody2D rb;
    public List<MovementBehav> movementOrder;
    private MovementBehav curMovement;
    // Start is called before the first frame update
    void Start()
    {
       rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            startBool = 1;
        }
        if(startBool == 1 & moving == 0) //start a new movement
        {
            if(movementOrder.Count > 0)
            {
                moving = 1;
                curMovement = movementOrder[0];
                movementOrder.RemoveAt(0);
                switch (curMovement.behav)
                {
                    case movType.move:
                    {
                        float target_x = curMovement.movPoint.transform.position.x;
                        Debug.Log(target_x);
                        initialDir = target_x - transform.position.x < 0 ? -1 : 1;
                        facingDir = initialDir;
                        break;
                        
                    }
                }
            }
            else
            {
                Debug.Log("Done with all movements");
            }
        }
        if(moving == 1)
        {
            switch (curMovement.behav)
            {
                case movType.move:
                {
                    float target_x = curMovement.movPoint.transform.position.x;
                    rb.velocity = new Vector2((target_x - gameObject.transform.position.x > 0 ? walkSpeed : -1 * walkSpeed), rb.velocity.y);
                    facingDir = rb.velocity.x < 0 ? -1 : 1;
                    //Debug.Log("Raycast loacation: " + transform.position + " direction: " +  Vector2.right * facingDir + " Length: " + facingDir * walkSpeed * jumpTime);
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * facingDir , walkSpeed * jumpTime + 0.5f, groundMask);
                    Debug.DrawRay(transform.position, Vector2.right * facingDir, Color.red , walkSpeed * jumpTime + 0.5f);
                    if(hit.collider != null)
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
                    if((transform.position.x - target_x) * initialDir >= 0f)
                    {
                        transform.position = new Vector2(target_x, transform.position.y);
                        facingDir = curMovement.facingDir == 0 ? facingDir : curMovement.facingDir; //if target is directional, face there
                        rb.velocity = Vector2.zero;
                        moving = 0;
                    }
                    break;
                }
            }
        }
    }
    void Jump(float ypos)
    {
        rb.velocity = new Vector2(rb.velocity.x, CalculateJumpVelocity(Physics2D.gravity.y, ypos - transform.position.y + 1.1f, jumpTime));
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

    public float CalculateJumpVelocity(float gravity, float peakHeight, float time)
    {
        return (peakHeight / time) - (gravity * time / 2);
    }
}
