using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public enum State
    {
        Idle,
        Spotted,
        Behavior, 
        Death, 
        Custom1, 
        Custom2
    };


    public State currentState = State.Idle;
    public State startState = State.Idle;
    public GameObject fovObj;
    public FOV fov;
    public float rotate_speed;
    public Gun gun;


    public virtual void IdleBehavior()
    {
    }


    public virtual void SpottedBehavior()
    {

    }

    public virtual void BehaviorBehavior()
    {

    }

    public virtual void DeathBehavior()
    {
        Destroy(gameObject);
    }

    public virtual void CustomBehavior()
    {
        return;
    }

    public virtual void Custom2Behavior()
    {
        return;
    }
    
    private void Start()
    {
        fovObj = transform.Find("FOV").gameObject;
        fov = fovObj.GetComponent<FOV> ();
        currentState = startState;
        gun = transform.GetComponentInChildren<Gun>();
        
    }

    private void debugRotation()
    {
        bool left = Input.GetKey(KeyCode.Z);
        bool right = Input.GetKey(KeyCode.X);
        //float xIn = Input.GetAxis("Horizontal");
        float xIn = (left) ? -1 : right ? 1 : 0;
        transform.Rotate(0, 0, -1 * xIn * rotate_speed);
    }




    private void FixedUpdate()
    {

        debugRotation();
        switch (currentState)
        {
            case State.Idle:
                IdleBehavior(); break;
            case State.Spotted:
                SpottedBehavior(); break;
            case State.Behavior:
                BehaviorBehavior(); break;

        }
    }

}
