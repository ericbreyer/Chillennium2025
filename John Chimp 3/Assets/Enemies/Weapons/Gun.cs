using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float recoilMag = 0;         //not implemented yet
    public float bulletSpeed = 20;
    public float shootGapSec = 1;

    public float timeSinceShot = 10000;

    public virtual void shoot()
    {
        if (timeSinceShot > shootGapSec)
        {
            GameObject newBullet = Instantiate(bulletPrefab, transform);
            newBullet.GetComponent<Rigidbody2D>().velocity = bulletSpeed * transform.right;

            timeSinceShot = 0;

        }
    }


    private void FixedUpdate()
    {
        timeSinceShot += Time.fixedDeltaTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    
}
