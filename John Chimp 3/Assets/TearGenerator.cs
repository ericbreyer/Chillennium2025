using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TearGenerator : MonoBehaviour
{
    public GameObject tearSourceL;
    public GameObject tearSourceR;
    public bool crying = false;
    public float minSPT = 0.05f;
    public float maxSPT = 0.2f;
    public float timeSinceCry = 0;
    public float nextWaitTime = 0.2f;
    public int next = 0;

    public void startTears()
    {
        crying = true;

    }

    public void FixedUpdate()
    {
        if(crying)
        {
            timeSinceCry += Time.deltaTime;
            if(timeSinceCry > nextWaitTime)
            {
                if(next == 0)
                {
                    GameObject obj = Instantiate(tearSourceL, tearSourceL.transform.position, tearSourceL.transform.rotation);
                    obj.SetActive(true);
                }
                else
                {
                    GameObject obj = Instantiate(tearSourceR, tearSourceR.transform.position, tearSourceR.transform.rotation);
                    obj.SetActive(true);
                }
                next = (next + 1) % 2;
                timeSinceCry = 0;
                nextWaitTime = Random.Range(minSPT, maxSPT);
            }
        }
    }

}
