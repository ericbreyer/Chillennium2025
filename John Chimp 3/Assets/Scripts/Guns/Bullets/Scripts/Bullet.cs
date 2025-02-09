using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public static class UsefulConsts
{
    public static float degreeToRad = 0.0174532925199f;
    public static float radToDegree = 57.2957795131f;
    public static Vector2 unitVecFromAngle(float deg)
    {
        return new Vector2(Mathf.Cos(deg * UsefulConsts.degreeToRad), Mathf.Sin(deg * UsefulConsts.degreeToRad));
    }
}


public class Bullet : MonoBehaviour
{

    public float maxDist = 10f;
    public List<string> bounceableTags = new List<string>(new[] { "Wall" });
    public List<string> termTags = new List<string>(new[] { "Enemy" });
    public GameObject spinCoin;
    private int collisionCount = 0;
    private float toDestroyCnt = 0;
    private Vector2 collisionPt;
    private Vector2 collisionNormal;
    private Vector2 startpos;

    public virtual void preComp()
    {
        Debug.Log("preComp Not Defined");
        return;
    }


    public virtual void handleGunShot(Vector2 start, float dir)
    {
        Debug.Log("handleGunShot Not Defined");
        return;

    }

    public virtual void renderGunShot()
    {
        Debug.Log("Render Not Defined");
        return;
    }

    public virtual void shoot(Vector2 start, float dir)
    {   
        startpos = start;
        handleGunShot(start, dir);
    }


    private void Update()
    {
        if(toDestroyCnt > (startpos - collisionPt).magnitude / 100)
        {
            GameObject coin = Instantiate(spinCoin, collisionPt, Quaternion.Euler(0, 0, 0));
            
            Rigidbody2D rb = coin.AddComponent<Rigidbody2D>();
            rb.angularVelocity = 1440;
            rb.gravityScale = 3;

            coin.GetComponent<Rigidbody2D>().AddForce(collisionNormal * 150 + Vector2.up * Random.Range(0, 300));
            Destroy(gameObject);
        }
        if(collisionCount > 0)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            toDestroyCnt+= Time.deltaTime;
            transform.position = collisionPt;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Bullet collision with: " + collision.gameObject);
        if (collisionCount == 1)
            return;
        collisionCount++;
        GetComponent<Collider2D>().enabled = false;
        collisionPt = collision.GetContact(0).point;
        collisionPt += collision.GetContact(0).normal * 0.1f;
        collisionNormal = collision.GetContact(0).normal;

        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
            Destroy(collision.gameObject);
        }
    }
}

