using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bystander : Enemy
{
    public GameObject exitSpot;
    public GameObject SurpriseBlock;
    public bool bulletHeard = false;
    public GameObject copPrefab;
    float x_target;
    Animator animationController;

    public override void Start()
    {
        base.Start();
        x_target = exitSpot.transform.position.x;
        animationController = GetComponent<Animator>(); 
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            signalBullet();
        }
    }
    public void signalBullet()
    {
        bulletHeard = true;
    }

    IEnumerator SurprisedLook()
    {
        SurpriseBlock.SetActive(true);
        yield return new WaitForSeconds(1);
        Destroy(SurpriseBlock);
        SurpriseBlock = null;
        GetComponentInChildren<TearGenerator>().startTears();
        currentState = State.Spotted;


    }

    bool unheard = true;
    public override void IdleBehavior()
    {
        
        if (unheard && bulletHeard)
        {
            unheard = false;
            animationController.SetInteger("State", 1);
            GameObject JohnChimp = FindObjectOfType<playerMovement>().gameObject;
            Vector3 storedScale = transform.localScale;

            float diff = JohnChimp.transform.position.x - transform.position.x;
            Debug.Log("dir: " + diff);
            if(diff > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
                Debug.Log("Look wayward");
            }
                
            StartCoroutine(SurprisedLook());
            Debug.Log("done");
            transform.localScale = storedScale;

        }
        else if(!unheard)
        {
            animationController.SetInteger("State", 0);
        }
            
    }


    public override void SpottedBehavior()
    {
        animationController.SetInteger("State", 2);
        if (moveToTarget(x_target))
        {
            copPrefab.SetActive(true);
            currentState = State.Custom1; //just don't do antyhing after this
        }
        
    }

    


}
