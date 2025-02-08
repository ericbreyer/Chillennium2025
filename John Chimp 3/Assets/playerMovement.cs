using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class playerMovement : MonoBehaviour
{
    public float walkSpeed;
    public BoxCollider2D bc;
    [SerializeField]
    private float curAngle;
    public float ropeSpeed;
    public float ropeSpeed2;
    private Vector2 checkPoint;
    private bool swinging;
    private SpriteRenderer sr;
    public float swingAngularSpeed;
    public float swingRadius;
    public LayerMask groundMask;
    public float jumpTime;
    public int startBool = 0;
    private int moving = 0;
    private int move_cnt = 0;
    public int facingDir;
    private int initialDir;
    public bool dead = false;
    private bool roped = false;
    private int jank;
    private Rigidbody2D rb;
    [SerializeField]
    private List<MovementBehav> movementOrder;
    private MovementBehav curMovement;

    private LineRenderer mlr;


    public void addToMovementOrder(MovementBehav mb) {
        if(moving == 0 && (movementOrder.Count == 0 || movementOrder[movementOrder.Count - 1] != mb)) {
            movementOrder.Add(mb);
        }
        
    }

    public int getPlaceInMovementOrder(MovementBehav mb) {
        return movementOrder.LastIndexOf(mb);
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

        if (startBool == 0) {


            mlr.positionCount = 1 + movementOrder.Count;
            ps.Add(transform.position);
        } else {
            mlr.positionCount = movementOrder.Count;
        }
    
        foreach(MovementBehav mb in movementOrder) {
            ps.Add(mb.movPoint.transform.position);
        }
        mlr.SetPositions(ps.ToArray());
    }



    // Start is called before the first frame update
    void Start()
    {
       rb = GetComponent<Rigidbody2D>();
        mlr = gameObject.AddComponent<LineRenderer>();
        mlr.materials = new Material[] {Graphic.defaultGraphicMaterial};
        mlr.startWidth = .1f;
        mlr.endWidth = .1f;
        mlr.startColor = Color.red;
        mlr.endColor = Color.HSVToRGB(.15f, 1, 1);
        mlr.sortingLayerName = "line";

        movementOrder = new List<MovementBehav>();
        sr = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {

        drawMovementOrder();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            startBool = 1;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(facingDir == 1)
        {
           sr.flipX = false;
        }
        else if(facingDir == -1)
        {
            sr.flipX = true;
        }
        if(startBool == 1 & moving == 0) //start a new movement
        {
            Debug.Log(movementOrder.Count+ " <-- moves left");
            if(movementOrder.Count > 0)
            {
                moving = 1;
                roped = false;
                swinging = false;
                
                if(move_cnt > 0) {
                    Debug.Log("removing move");
                    movementOrder.RemoveAt(0);
                    if (movementOrder.Count == 0) {
                        moving = 0;
                        return;
                    }
                }
                Debug.Log("Continuing on with update");
                curMovement = movementOrder[0];
                move_cnt += 1;
                switch (curMovement.behav)
                {
                    case movType.move:
                    {
                        float target_x = curMovement.movPoint.transform.position.x;
                        //Debug.Log(target_x);
                        initialDir = target_x - transform.position.x < 0 ? -1 : 1;
                        facingDir = initialDir;
                        
                        break;
                        
                    }
                    case movType.grapple:
                    {
                        StartRope();
                        if (movementOrder.Count > 0)
                        {
                            rb.gravityScale = 1f;
                        }
                        
                        break;
                    }
                    case movType.swing:
                    {
                        StartRope();
                        break;
                    }
                }
            }
            else
            {
                Debug.Log("Done with all movements");
                startBool = 0;
                move_cnt = 0;
            }
        }
        if(moving == 1)
        {
            switch (curMovement.behav)
            {
                case movType.move:
                {
                    move();
                    break;
                }
                case movType.grapple:
                {
                    MiddleRopeGrapple();
                    break;
                }
                case movType.swing:
                {
                    if (swinging)
                    {
                        Endrope(initialDir);
                    }
                    else
                    {
                        MiddleRope(initialDir);
                    }
                    break;
                }
            }
        }
    }
    void Jump(float ypos)
    {
        rb.velocity = new Vector2(rb.velocity.x, CalculateJumpVelocity(ypos - transform.position.y + 1.5f, jumpTime));
    }

    void StartRope()
    {
        roped = false;
        Vector2 anchorPoint = curMovement.movPoint.transform.position;
        initialDir = anchorPoint.x - transform.position.x < 0 ? -1 : 1;
        facingDir = initialDir;
    }

    void MiddleRopeGrapple()
    {
        Vector2 anchorPoint = curMovement.movPoint.transform.position;
        Vector2 checkPoint = anchorPoint + Vector2.down * 1.01f;

        if (bigCheck())
        {
            roped = true;
            //Debug.Log("we ropin");
            //Instantiate(rope);
            //rope.endpoint = curMovement.movPoint.transform.position;
            //rope.monkey = this;
        }
        else
        {
            Debug.Log("moving to grapple");
            move();
        }
        if (roped)
        {
            rb.gravityScale = 0;
            rb.velocity = (checkPoint - (Vector2)transform.position).normalized * ropeSpeed;

        }
        if((transform.position.x - checkPoint.x) * initialDir >= 0f)
        {
            if (!roped)
            {
                //Debug.Log("The ropes didnt work lol");
                moving = 0;
            }
            else
            {
                transform.position = new Vector2(checkPoint.x, checkPoint.y);
                facingDir = curMovement.facingDir == 0 ? facingDir : curMovement.facingDir; //if target is directional, face there
                rb.velocity = Vector2.zero;
                moving = 0;
                //rb.gravityScale = 1;
                
                            
            }
        }


    }
    void MiddleRope(int offset)
    {
        Vector2 anchorPoint = curMovement.movPoint.transform.position;
        
        if (!roped)
        {
            jank = 0;
            checkPoint = anchorPoint - Vector2.right * curMovement.facingDir * swingRadius;
            if (bigCheck())
            {
                roped = true;
                initialDir = checkPoint.x - transform.position.x < 0 ? -1 : 1;
//Debug.Log("we ropin, side check");
                jank = (curMovement.facingDir == 1) ? 0 : 1;
                //Instantiate(rope);
                //rope.endpoint = curMovement.movPoint.transform.position;
                //rope.monkey = this;
            }
            
            else
            {
                checkPoint = anchorPoint - Vector2.up * swingRadius;
                if (bigCheck())
                {
                    roped = true;
                    initialDir = checkPoint.x - transform.position.x < 0 ? -1 : 1;
                    jank = (curMovement.facingDir == 1) ? 2 : 3;
                    //Debug.Log("we ropin, bottom check");
                    //Instantiate(rope);
                    //rope.endpoint = curMovement.movPoint.transform.position;
                    //rope.monkey = this;
                }
                else
                {
                    Debug.Log("moving to grapple");
                    move();
                }
                
            }
        }

        
        if (roped)
        {
            rb.gravityScale = 0;
            rb.velocity = (checkPoint - (Vector2)transform.position).normalized * ropeSpeed2;

        }
        if((transform.position.x - checkPoint.x) * initialDir >= 0f)
        {
            if (!roped)
            {
                Debug.Log("The ropes didnt work lol");
            }
            else
            {
                transform.position = new Vector2(checkPoint.x, checkPoint.y);
                facingDir = curMovement.facingDir == 0 ? facingDir : curMovement.facingDir; //if target is directional, face there
                rb.velocity = Vector2.zero;
                //Debug.Log("we swinging now");
                swinging = true;
                curAngle = Mathf.Atan2(((Vector2)transform.position - anchorPoint).x, ((Vector2)transform.position - anchorPoint).y) * Mathf.Rad2Deg % 360f;
                if(jank == 0)
                {
                    curAngle += -90;
                }
                else if (jank == 1)
                {
                    curAngle += -90;
                }
                else if (jank == 2)
                {
                    curAngle += 90;
                }
                else if (jank == 3)
                {
                    curAngle += 90;
                }
                Debug.Log("Initial angle: " + curAngle + "Jank: " + jank);
                //rb.gravityScale = 1;
                
                            
            }
        }
    }
    void Endrope(int offset)
    {   
        Vector2 anchorPoint = curMovement.movPoint.transform.position;
        
        curAngle += -1* curMovement.facingDir * swingAngularSpeed * Time.fixedDeltaTime;
        float radins = curAngle * Mathf.Deg2Rad;
        transform.position = anchorPoint + new Vector2(Mathf.Cos(radins), Mathf.Sin(radins)) * swingRadius;

        if (isGrounded())
        {
            Debug.Log("we got grounded");
            moving = 0;
            roped = false;
            rb.velocity = new Vector2(0, rb.velocity.y);
            facingDir = curMovement.facingDir;
            rb.gravityScale = 1;
        }
    }

    bool bigCheck()
    {
        RaycastHit2D hit = Physics2D.Linecast(new Vector2(transform.position.x, transform.position.y) + new Vector2(0.49f * facingDir, 1f), checkPoint+ new Vector2(0.49f * facingDir, 1f), groundMask);
        RaycastHit2D hit2 = Physics2D.Linecast(new Vector2(transform.position.x, transform.position.y) + new Vector2(0.49f * facingDir, -1f), checkPoint+ new Vector2(0.49f * facingDir, -1f), groundMask);
        RaycastHit2D hit3 = Physics2D.Linecast(new Vector2(transform.position.x, transform.position.y) + new Vector2(-0.49f * facingDir, -1f), checkPoint+ new Vector2(-0.49f * facingDir, -1f), groundMask);
        RaycastHit2D hit4 = Physics2D.Linecast(new Vector2(transform.position.x, transform.position.y), checkPoint, groundMask);
        Debug.DrawLine(new Vector2(transform.position.x, transform.position.y) + new Vector2(0.5f * facingDir, 1f), checkPoint+ new Vector2(0.49f * facingDir, 1f), Color.red);
        Debug.DrawLine(new Vector2(transform.position.x, transform.position.y) + new Vector2(0.5f * facingDir, -1f), checkPoint+ new Vector2(0.49f * facingDir, -1f), Color.red);
        Debug.DrawLine(new Vector2(transform.position.x, transform.position.y) + new Vector2(-0.5f * facingDir, -1f), checkPoint+ new Vector2(-0.49f * facingDir, -1f), Color.red);
        Debug.DrawLine(new Vector2(transform.position.x, transform.position.y), checkPoint);

        return (hit.collider == null && hit2.collider == null && hit3.collider == null);
    }
    void move()
    {
        
        float target_x = curMovement.movPoint.transform.position.x;
        if (curMovement.movPoint.transform.position.y < bc.bounds.max.y && transform.position.y > bc.bounds.max.y && (isGrounded() || rb.velocity.y > -0.5) && curMovement.movPoint.transform.position.x - 0.51f< bc.bounds.max.x && curMovement.movPoint.transform.position.x + 0.51f> bc.bounds.min.x)
        {
            Debug.Log("cond met");
            rb.velocity = new Vector2((target_x - gameObject.transform.position.x > 0 ? -1 * walkSpeed : 1 * walkSpeed), rb.velocity.y);
        }
        else
        {
            Debug.Log("cond not met");
            rb.velocity = new Vector2((target_x - gameObject.transform.position.x > 0 ? walkSpeed : -1 * walkSpeed), rb.velocity.y);
        }
        
        facingDir = rb.velocity.x < 0 ? -1 : 1;
        //Debug.Log("Raycast loacation: " + transform.position + " direction: " +  Vector2.right * facingDir + " Length: " + facingDir * walkSpeed * jumpTime);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * facingDir , walkSpeed * jumpTime + 0.5f, groundMask);
        Debug.DrawRay(transform.position, Vector2.right * facingDir, Color.red , walkSpeed * jumpTime + 0.5f);
        if(hit.collider != null)
        {
            //Debug.Log("we hit something with ray");
            //Debug.Log(hit.collider.gameObject.name);   
        }
        if (hit.collider != null && isGrounded())
        {
            //Debug.Log("Jumping");
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
        float gravity = - (2*peakHeight/(time * time));
        rb.gravityScale = gravity / Physics2D.gravity.y;
        return (-1 * gravity * time);
    }

    public void die() {
        startBool = 0;
        dead = true;
        moving = 0;
        clearMovementOrder();
        StartCoroutine(die_co());
    }

    private IEnumerator die_co() {
        for(int i = 0; i < 10000; i++) {
            this.transform.Rotate(Vector3.forward, (i/360f) / 2);
            yield return new WaitForEndOfFrame();
        }
    }
}
