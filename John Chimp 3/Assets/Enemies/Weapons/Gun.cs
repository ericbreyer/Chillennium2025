using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float recoilMag = 0;         //not implemented yet
    public float bulletSpeed = 20;
    public float shootGapSec = 1;
    public GameObject gunFront;
    public FOV fov;
    public Transform parent;
    public Transform pivotPt;

    public float timeSinceShot = 10000;

    public virtual void shoot(Vector3 direction)
    {
        if (timeSinceShot > shootGapSec)
        {
            Vector3 goalPos = FindObjectOfType<playerMovement>().gameObject.transform.position;
            Vector3 posDir = goalPos - pivotPt.position;
            GameObject newBullet = Instantiate(bulletPrefab, gunFront.transform.position, gunFront.transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().velocity = bulletSpeed * posDir.normalized;
            timeSinceShot = 0;

        }
    }



    


    private void FixedUpdate()
    {
        timeSinceShot += Time.fixedDeltaTime;
        if(fov.visible)
        {
            Vector3 goalPos = FindObjectOfType<playerMovement>().gameObject.transform.position;
            Vector3 posDir = goalPos - pivotPt.position;
            Vector3 pointingDir = transform.right * parent.localScale.x;

            float angle = Vector3.SignedAngle(pointingDir, posDir, Vector3.forward);
            transform.Rotate(0, 0, angle);
            Debug.Log("Rotating");

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    
}
