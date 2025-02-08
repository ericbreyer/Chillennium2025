using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BasicBullet : Bullet
{
    //start handles the start position of the bullet shot
    //dir is the direction in degrees in the usual way (-1 * gun direction if you get it from there)

    public bool shot = false;
    public float lifetime = 0.5f;
    public float startTime = 0;
    public List<Vector3> positions = new List<Vector3>();
    public bool lineRendEnabled = false;
    public float speed = 100;
    public override void handleGunShot(Vector2 start, float dir)
    {
        startTime = Time.time;
        positions = new List<Vector3>();
        float dist = 0;
        Vector2 curRayStartPoint = start;
        positions.Add(start);
        Vector2 dirVec = UsefulConsts.unitVecFromAngle(dir);
        Ray2D bulletRay = new Ray2D(curRayStartPoint, dirVec);
        RaycastHit2D hit;
        LineRenderer lr = gameObject.GetComponent<LineRenderer>();
        
        GetComponent<Rigidbody2D>().velocity = dirVec * speed;
        //I'll work on this part later
        //while(getRayCast(bulletRay, dist, out hit) && dist < maxDist)
        //{
        //    //Debug.DrawRay(curRayStartPoint, dirVec, UnityEngine.Color.blue, 10);
        //    LineRenderer lr = gameObject.AddComponent<LineRenderer>();

        //    Debug.Log("Layer is: " + LayerMask.LayerToName(hit.collider.gameObject.layer));
        //    if (bounceableTags.Contains(LayerMask.LayerToName(hit.collider.gameObject.layer)))
        //    {
        //        Vector3[] pointArr = { curRayStartPoint, hit.point };
        //        lr.SetPositions(pointArr);
        //        curRayStartPoint = hit.point;
        //        dirVec = Vector2.Reflect(dirVec, hit.normal);
        //        dist += hit.distance;
        //        bulletRay = new Ray2D(curRayStartPoint, dirVec);
        //        Debug.Log("Distance shot is " + dist);
        //        break;
        //    }
        //    else
        //        break;
        //}
        if (lineRendEnabled)
        {
            Debug.Log("maxDist - dist = " + (maxDist - dist));
            if (maxDist - dist > 0)
            {
                Vector2 end = curRayStartPoint + (dirVec * 4);
                positions.Add(end);

                lr.positionCount = positions.Count;

                Debug.DrawRay(curRayStartPoint, dirVec, Color.red, 10);
            }
            shot = true;
        }

    }
    
    //TODO update strList to be concatenation of global lists
    public Collider2D getRayCast(Ray2D ray, float dist, out RaycastHit2D hit)
    {
        string[] strList = { "Wall", "Enemy" };  //should update this to 
        LayerMask lm = LayerMask.GetMask(strList);
        hit = Physics2D.Raycast(ray.origin, ray.direction, maxDist - dist, lm);
        return hit.collider;
    }

    private void FixedUpdate()
    {
        if (shot)
        {
            if (lineRendEnabled)
            {
                GetComponent<LineRenderer>().enabled = true;
                GetComponent<LineRenderer>().SetPositions(positions.ToArray());
            }
               
        }
            
        if(Time.time - startTime > lifetime)
        {
            Destroy(gameObject);
        }
    }


}
