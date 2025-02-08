using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float walkSpeed;
    public float jumpTime;
    private int startBool = 0;
    private int moving = 0;
    private int facingDir;
    private int initialDir;
    private Rigidbody2D rb;
    private List<MovementBehav> movementOrder;
    private MovementBehav curMovement;

    private LineRenderer mlr;


    public void addToMovementOrder(MovementBehav mb) {
        movementOrder.Add(mb);
    }

    public int getPlaceInMovementOrder(MovementBehav mb) {
        return movementOrder.IndexOf(mb);
    }

    public void clearMovementOrder() {
        this.movementOrder.Clear();
    }

    void drawMovementOrder() {
        if(movementOrder.Count == 0) {
            mlr.SetPositions(new Vector3[0]);
            mlr.positionCount = 0;
        }
        List<Vector3> ps = new List<Vector3>();
        mlr.positionCount = 1 + movementOrder.Count;
        ps.Add(transform.position);
        foreach(MovementBehav mb in movementOrder) {
            ps.Add(mb.transform.position);
        }
        mlr.SetPositions(ps.ToArray());
    }



    // Start is called before the first frame update
    void Start()
    {
       rb = GetComponent<Rigidbody2D>();
        mlr = gameObject.AddComponent<LineRenderer>();
        movementOrder = new List<MovementBehav>();
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Q)) {
            clearMovementOrder();
        }

        drawMovementOrder();

        if (Input.GetButton("Space"))
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
                    rb.velocity = new Vector2((target_x - gameObject.transform.position.x > 1 ? walkSpeed : -1 * walkSpeed), rb.velocity.y);
                    facingDir = rb.velocity.x < 0 ? -1 : 1;
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(facingDir * walkSpeed * jumpTime, 0));
                    if (hit.collider != null & isGrounded())
                    {
                        Collider2D col = hit.collider;
                        Jump(col.bounds.max.y);
                    }
                    if((transform.position.x - target_x) * initialDir >= 0f)
                    {
                        transform.position = new Vector2(target_x, transform.position.y);
                        facingDir = curMovement.facingDir == 0 ? facingDir : curMovement.facingDir; //if target is directional, face there
                        moving = 0;
                    }
                    break;
                }
            }
        }
    }
    void Jump(float ypos)
    {
        rb.velocity = new Vector2(rb.velocity.x, CalculateJumpVelocity(Physics2D.gravity.y, ypos - transform.position.y + 0.1f, jumpTime));
    }

    bool isGrounded()
    {
        return Mathf.Approximately(rb.velocity.y, 0f);
    }

    public float CalculateJumpVelocity(float gravity, float peakHeight, float time)
    {
        return (peakHeight / time) - (gravity * time / 2);
    }
}
