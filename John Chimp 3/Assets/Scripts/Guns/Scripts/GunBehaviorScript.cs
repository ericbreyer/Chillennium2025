using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public struct GunDef
{
    public string name;
    public float cooldown;
    public float spread;
}

public class GunBehaviorScript : MonoBehaviour
{
    private bool mousePressed = false;
    public Bullet bulletType;
    public GameObject smokeEffect;
    public GameObject sparkEffect;
    public float timeSinceShot;
    public GameObject firePoint;

    //public AudioClip shot;
    //private AudioSource s;

    
    [SerializeField]
    private GunDef gun;

    // Start is called before the first frame update
    
    private void Update() { 
        if (Input.GetMouseButton(0))
            shootReq();
    }

    private void Start()
    {
        //s = gameObject.AddComponent<AudioSource>();
        //s.clip = shot;
    }

    private void FixedUpdate()
    {
        timeSinceShot += Time.fixedDeltaTime;
      
        
    }


    void shootReq()
    {
        if(FindObjectOfType<playerMovement>().startBool == 0) {
            return;
        }

        if(timeSinceShot >= gun.cooldown)
        {   
            float spread = gun.spread;
            timeSinceShot = 0;
            //s.Play();
            Bullet bullet = Instantiate(bulletType, transform.position + transform.forward, transform.rotation);
            Instantiate(smokeEffect, transform.position, transform.rotation * new Quaternion(0, 0, 0, 1));
            Instantiate(sparkEffect, transform);
            Vector3 dir = firePoint.transform.position - transform.position;
            Debug.Log("gun dir " + dir);
            Bystander[] bystanders = FindObjectsOfType<Bystander>();
            for(int i = 0; i < bystanders.Length; i++)
            {
                bystanders[i].signalBullet();
            }

            bullet.shoot(firePoint.transform.position, gameObject.transform.eulerAngles.z + UnityEngine.Random.Range(-spread, spread)); //idk why we need the 90 but it doesn't work without it
        }

    }
    
}
