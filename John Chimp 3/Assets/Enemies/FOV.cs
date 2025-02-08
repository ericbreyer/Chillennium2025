using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV : MonoBehaviour
{
    public float viewRadius;
    public float viewAngle;
    public Collider2D[] playersInRange;
    public LayerMask obstacleMask, playerMask;
    public bool visible = false;


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
        playersInRange = Physics2D.OverlapCircleAll(transform.position, viewRadius);

        visible = false;

        for (int i = 0; i < playersInRange.Length; i++)
        {
            Transform player = playersInRange[i].transform;
            Vector2 dirToTarget = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);

            if (Vector2.Angle(dirToTarget, transform.right) < viewAngle / 2)
            {

                float distance = Vector2.Distance(transform.position, player.position);

                if (distance < viewRadius)
                {
                    if (!Physics2D.Raycast(transform.position, dirToTarget, distance, obstacleMask))
                    {
                        visible = true;
                    }
                }
            }
        }


    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
