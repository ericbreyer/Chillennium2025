using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamerManager : MonoBehaviour
{

    [SerializeField]
    private int[] sceneBuildIdxsForLevels;
    private int curScene = 0;
    private SpriteRenderer bigFadeBoi;

    void Awake() {
        var objs = GameObject.FindObjectsOfType<GamerManager>();

        if (objs.Length > 1) {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }


    public void Respawn() {
        FindObjectOfType<playerMovement>().die();
        StartCoroutine(newcorouting());
    }

    private IEnumerator newcorouting()
    {
        yield return new WaitForSeconds(3);
        StartCoroutine(FadeCoroutine(() => {
            SceneManager.LoadScene(sceneBuildIdxsForLevels[curScene]);
            }
        ));
    }

    public void NextScene() {
        
        StartCoroutine(FadeCoroutine(() => {
            curScene += 1;
            SceneManager.LoadScene(sceneBuildIdxsForLevels[curScene]);
            Start();
            }
        ));
    }

    private IEnumerator FadeCoroutine(Action a) {
        
        

        float SPEEEEEEEEÈD = .005f;
        for(float i = 0; i < 1; i += SPEEEEEEEEÈD) {
            bigFadeBoi.color = new Color(bigFadeBoi.color.r, bigFadeBoi.color.g, bigFadeBoi.color.b, i);
            yield return null;
        }
        a.Invoke();
        for (float i = 1; i >= 0; i -= SPEEEEEEEEÈD) {
            bigFadeBoi.color = new Color(bigFadeBoi.color.r, bigFadeBoi.color.g, bigFadeBoi.color.b, i);
            yield return null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        bigFadeBoi = GetComponent<SpriteRenderer>();
        bigFadeBoi.color = new Color(bigFadeBoi.color.r, bigFadeBoi.color.g, bigFadeBoi.color.b, 0);

        Debug.Log("Current scene:" +  curScene);
        FindObjectOfType<DialogueuwManager>().yapqueue.Clear();
        DialogueuwManager yq = FindObjectOfType<DialogueuwManager>();
                
        switch (curScene + 1)
        {
            case 1:
            {

                FindObjectOfType<DialogueuwManager>().QueueDialogue("Hey yo john, its your boy Ham Altman... I'm here to get you through this mess.");
                FindObjectOfType<DialogueuwManager>().QueueDialogue("Do you even know how to walk? ....");
                FindObjectOfType<DialogueuwManager>().QueueDialogue("Didn't think so. Click on objects to plan your move. Once you make a route, click space.");
                FindObjectOfType<DialogueuwManager>().QueueDialogue("Once you've cleared a traincar of enemies, you can move on by going to the exit arrow");
                FindObjectOfType<DialogueuwManager>().QueueDialogue("Q - reset route, D - die and restart");
                break;
            }
            case 2:
            {
                FindObjectOfType<DialogueuwManager>().QueueDialogue("Don't leave anyone alive - you don't understand how much this NFT is worth");
                FindObjectOfType<DialogueuwManager>().QueueDialogue("Oh wait, where's your gun?");
                FindObjectOfType<DialogueuwManager>().QueueDialogue("You can only shoot while you're moving!? Well, give it a shot...");

                break;
            }
            case 3:
            {
                FindObjectOfType<DialogueuwManager>().QueueDialogue("What's he doing up there? Eh, take him out anyway");
                break;
            }
            case 4:
            {
                FindObjectOfType<DialogueuwManager>().QueueDialogue("This guy's scoped on on the train cabin. See if you can get him another way");
                break;
            }
            case 5:
            {
                FindObjectOfType<DialogueuwManager>().QueueDialogue("Watch out for the innocents, they'll go grab the cops.");
                break;
            }
            default:
            {
                break;
            }
        }
         yq.StartCoroutine(yq.ShowOneDialogue(yq.yapqueue.Dequeue()));
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)) {

            die();
            return;
        }
    }

    public void die() {
        FindObjectOfType<MusicManager>().musicDie();
        Respawn();
    }
}
