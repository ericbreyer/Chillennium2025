using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SongParts
{
    public AudioClip[] parts;
}

public class MusicManager : MonoBehaviour
{
    private float planVol;
    private float actVol;
    public float fadeInTime;
    public float loopFadeTime;
    [SerializeField]
    private bool act2target = false;
    [SerializeField]
    private bool plan2target = false;
    [SerializeField]
    private float plan2 = 0;
    private float act2 = 0;
    private bool dead = false;
    public float fadeOutTime;
    private bool laststartBool = false;
    public playerMovement pm;
    [SerializeField]
    public SongParts[] planning;
    [SerializeField]
    public SongParts[] action;
    [SerializeField]
    public SongParts[] death;

    private AudioSource planSource;
    private AudioSource planSource2;
    private AudioSource actSource;
    private AudioSource actSource2;
    private AudioSource deadSource;

    private int planSeed1, planSeed2, actionSeed1, actionSeed2, deathSeed1, deathSeed2;

    void Awake() {
        var objs = GameObject.FindObjectsOfType<MusicManager>();

        if (objs.Length > 1) {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private
    // Start is called before the first frame update
    void Start()
    {
        planSeed1 = Random.Range(0, planning.Length);
        planSeed2 = Random.Range(0, planning[planSeed1].parts.Length);

        actionSeed1 = Random.Range(0, action.Length);
        actionSeed2 = Random.Range(0, action[actionSeed1].parts.Length);

        deathSeed1 = Random.Range(0, death.Length);
        deathSeed2 = Random.Range(0, death[deathSeed1].parts.Length);

        planSource = gameObject.AddComponent<AudioSource>();
        actSource = gameObject.AddComponent<AudioSource>();
        planSource2 = gameObject.AddComponent<AudioSource>();
        actSource2 = gameObject.AddComponent<AudioSource>();
        deadSource = gameObject.AddComponent<AudioSource>();

        planSource.clip = planning[planSeed1].parts[planSeed2];
        planSource2.clip = planSource.clip;
        actSource.clip = action[actionSeed1].parts[actionSeed2];
        actSource2.clip = actSource.clip;
        deadSource.clip = death[deathSeed1].parts[deathSeed2];

        fadeInTime = Mathf.Max(0.01f, fadeInTime);
        fadeOutTime = Mathf.Max(0.01f, fadeOutTime);
        loopFadeTime = Mathf.Max(0.01f, loopFadeTime);

        planVol = 1;
        planSource.volume = 1f;
        planSource.Play();
        planSource2.volume = 0f;
        actSource.volume = 0f;
        actVol = 0;
        actSource2.volume = 0f;
        actSource.Play();

        planSource2.Play();
        actSource2.Play();

        //planSource2.enabled = false;
    }


    // Update is called once per frame
    void Update()
    {
        if(act2target)
        {
            act2 = Mathf.Min((1/loopFadeTime)* Time.deltaTime + act2, 1f);
        }
        else
        {
            act2 = Mathf.Max(act2 - (1/loopFadeTime)* Time.deltaTime, 0f);
        }
        if(plan2target)
        {
            plan2 = Mathf.Min(plan2 + (1/loopFadeTime)* Time.deltaTime, 1f);
        }
        else
        {
            plan2 = Mathf.Max(plan2 - (1/loopFadeTime)* Time.deltaTime, 0f);
        }
        if (dead)
        {
            actSource.volume = Mathf.Max(actSource.volume - Time.deltaTime * (1/0.1f), 0f) * (1-act2);
            actSource2.volume = Mathf.Max(actSource2.volume - Time.deltaTime * (1/0.1f), 0f) * act2;
            planSource.volume = Mathf.Max(planSource.volume - Time.deltaTime * (1/0.1f), 0f) * (1-plan2);
            planSource2.volume = Mathf.Max(planSource2.volume - Time.deltaTime * (1/0.1f), 0f) * plan2;
            return;
        }
        if(pm.startBool == 0 && laststartBool == true)
        {
            planSource.time = actSource.time % 4f;
            planSource2.time = actSource.time % 4f;
            planVol = 0;
            planSource.volume = 0;
            planSource2.volume = 0;
            planSource.Play();
            planSource2.Play();
        }
        if(pm.startBool == 1 && laststartBool == false)
        {
            actSource.time = planSource.time % 4f;
            actSource2.time = actSource.time;
            actVol = 0;
            actSource.volume = 0;
            actSource2.volume = 0;
            actSource.Play();
            actSource2.Play();
        }
        if(pm.startBool == 1)
        {
            planVol = Mathf.Min(planVol - Time.deltaTime * (1/fadeInTime), 0f);
            actVol = Mathf.Max(actVol + Time.deltaTime * (1/fadeInTime), 1f);
            planSource.volume = planVol * (1-plan2); //fade in is on purpose
            actSource.volume = actVol * (1-act2);
            planSource2.volume = planVol * plan2; 
            actSource2.volume = actVol * act2;
        }
        if(pm.startBool == 0)
        {
            planVol = Mathf.Max(planVol + Time.deltaTime * (1/fadeInTime), 0f);
            actVol = Mathf.Min(actVol - Time.deltaTime * (1/fadeOutTime), 1f);
            actSource.volume = actVol * (1-act2);
            planSource.volume = planVol * (1-plan2);
            actSource2.volume = actVol * act2;
            planSource2.volume = planVol * plan2;
        }
        if(actSource.time > 36f && !act2target || actSource2.time > 36f && act2target)
        {
            act2target = !act2target;
            if(act2target)
            {
                actSource2.time = 4f;
                actSource2.Play();
            }
            else
            {
                actSource.time = 4f;
                actSource.Play();
            }
        }
        if(planSource.time > 8f && !plan2target || planSource2.time > 8f && plan2target)
        {
            bool oldtarg = plan2target;
            plan2target = !plan2target;
            Debug.Log("Changing from " + oldtarg +  " to "  + plan2target);
            if(plan2target)
            {
                planSource2.time = 4f;
                planSource2.Play();
            }
            else
            {
                planSource.time = 4f;
                planSource.Play();
            }
        }

        laststartBool = (pm.startBool == 1);
    }

    public void musicDie()
    {
        deadSource.volume = 1f;
        deadSource.time = 0;
        deadSource.Play();
        dead = true;
    }

}
