using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum movType
    {
        move,
        jump,
        grapple,
        swing,
        wait
    }

[System.Serializable]
public class MovementBehav : MonoBehaviour
{
    public GameObject movPoint;
    public int facingDir;
    
    public movType behav;


    //TODO - add mouse highlight function for this object

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
