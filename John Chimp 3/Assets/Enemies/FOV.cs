using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FOV : MonoBehaviour
{
    public float viewRadius;
    public float viewAngle;
    public Collider2D[] playersInRange;
    public LayerMask obstacleMask, playerMask;
    
    //to expose to the Enemy
    public bool visible = false;
    public Vector2 dirToTarget;
    public GameObject gun;



    public Vector2 DirFromAngle(float angleDeg, bool global)
    {
        if(!global)
        {
            angleDeg += transform.eulerAngles.z;
        }
        return new Vector2(Mathf.Cos(angleDeg * Mathf.Deg2Rad), Mathf.Sin(angleDeg * Mathf.Deg2Rad));
    }


    void FindVisiblePlayers()
    {
        playersInRange = Physics2D.OverlapCircleAll(transform.position, viewRadius, playerMask);

        visible = false;

        for (int i = 0; i < playersInRange.Length; i++)
        {
            Transform player = playersInRange[i].transform;
            dirToTarget = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);

            //Debug.Log(dirToTarget);
            if (Vector2.Angle(dirToTarget, transform.right) < viewAngle / 2)
            {
                Debug.Log(Vector2.Angle(dirToTarget, transform.right));
                float distance = Vector2.Distance(transform.position, player.position);
                Debug.Log(distance);
                
                if (distance < viewRadius)
                {
                    if (!Physics2D.Raycast(transform.position, dirToTarget, distance, obstacleMask))
                    {
                        Debug.Log("Seen");
                        visible = true;

                    }
                    else
                    {
                        Debug.Log("Occluded");
                    }
                }
                else
                {
                    Debug.Log("Not Seen");
                }
            }
        }


    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void FixedUpdate()
    {
        //if(gun)
        //{
        //    float angle = Vector3.SignedAngle(gun.transform.right, dirToTarget, Vector3.forward);
        //    gun.transform.Rotate(0, 0, angle);
        //}
        FindVisiblePlayers();
        if (visible)
        {
            Transform pappa_transform = transform.parent;
            float pappa_transform_scale = (pappa_transform.localScale.x > 0) ? 1 : -1;
            float angle = Vector3.SignedAngle(transform.right.normalized, dirToTarget.normalized, Vector3.forward);
            transform.Rotate(0, 0, angle);
            
        }
        
        

        //float end_angle = transform.localEulerAngles.z;

        
        //if (end_angle > 90 && end_angle < 180)
        //{
        //    end_angle = 180 - end_angle;
        //    Vector3 temp = pappa_transform.localScale;
        //    pappa_transform.localScale = new Vector3(-1 * temp.x, temp.y, temp.z);
        //    transform.localEulerAngles = new Vector3(0, 0, end_angle);

        //}



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
