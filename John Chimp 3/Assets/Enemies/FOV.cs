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
    public Vector3 targetLocation;
    public GameObject gun;
    float startangle = 0;



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
            targetLocation = player.transform.position;
            //Debug.Log(dirToTarget);
            if (Vector2.Angle(dirToTarget, transform.right * transform.parent.localScale.x) < viewAngle / 2)
            {
                Debug.Log(Vector2.Angle(dirToTarget, transform.right * transform.parent.localScale.x));
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


    public void setRotation(float angle)
    {
        transform.localEulerAngles = new Vector3(0, 0, angle);
    }

    private void FixedUpdate()
    {
        //if(gun)
        //{
        //    float angle = Vector3.SignedAngle(gun.transform.right, dirToTarget, Vector3.forward);
        //    gun.transform.Rotate(0, 0, angle);
        //}
        FindVisiblePlayers();



        }

    // Update is called once per frame
    void Update()
    {
        
    }
}
