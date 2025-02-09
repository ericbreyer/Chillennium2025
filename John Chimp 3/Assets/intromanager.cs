using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class intromanager : MonoBehaviour
{

    public Sprite johnchimp;
    public Sprite hamBankman;
    public GameObject scene2;
    public GameObject scene3;
    public SpriteRenderer ftb;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(timeline());

        var dm = FindObjectOfType<DialogueuwManager>();
        dm.ChangeYapSpeed(30);
    }

    IEnumerator timeline() {
        scene2.SetActive(false);
        scene3.SetActive(false);
        
        FindObjectOfType<waggle>().enabled = false;
        // SCENE 1

        var dm = FindObjectOfType<DialogueuwManager>();

        var title = FindObjectOfType<titleargsdvoihwobnv>();
        var tmpuguui = title.GetComponentInChildren<TextMeshProUGUI>();
        var text = "PREVIOUSLY ON JOHN CHIMP...";
        for(int i = 0; i <= text.Length; ++i) {
            tmpuguui.text = text.Substring(0, i);
            yield return new WaitForSeconds(2f/text.Length);
        }
        yield return new WaitForSeconds(1);

        var start = Time.time;
        var end = start + 1;
        while(Time.time < end) {
            title.GetComponentInChildren<TextMeshProUGUI>().color = new Color(title.GetComponentInChildren<TextMeshProUGUI>().color.r, title.GetComponentInChildren<TextMeshProUGUI>().color.g, title.GetComponentInChildren<TextMeshProUGUI>().color.b, 1-(Time.time - start));
            yield return new WaitForSeconds(0);
        }

        FindObjectOfType<titleargsdvoihwobnv>().gameObject.SetActive(false);


        var vp = FindObjectOfType<VideoPlayer>();
        vp.Play();

        yield return new WaitForSeconds(3);

        yield return new WaitWhile(() => vp.isPlaying);

        // SCENE 2

        scene2.SetActive(true);


        vp.enabled = false;
        FindObjectOfType<waggle>().enabled = true;

        dm.ChangeYapper(johnchimp);

        dm.QueueDialogue("Oh right my wife died, but at least I got all this money");
        dm.QueueDialogue("Buy the dip, ride the wave, then dump it all — BOOM, profit!");
        dm.QueueDialogue("JOHN CHIMP gets it done!!");

        yield return new WaitWhile(() => !dm.Done());

        // SCENE 3

        scene2.SetActive(false);
        scene3.SetActive(true);

        dm.ChangeYapper(hamBankman);

        dm.QueueDialogue("Look at this idiot.Flashing his gains. but I'll get you back JOHN CHIMP");
        dm.QueueDialogue("You think you’re the king of crypto ? Well, I, HAM BANKMAN, really pull the strings");
        dm.QueueDialogue("This NFT… it’s the last thing he’s got left of that dog. Time to make him really feel the loss");

        yield return new WaitWhile(() => !dm.Done());

        scene2.SetActive(true);
        scene3.SetActive(false);

        dm.ChangeYapper(johnchimp);

        dm.QueueDialogue("NOOOO My NFT of my dead dog is gone!!! That was my last connection to him!");
        dm.QueueDialogue("Alright, whoever did this is going down. I’m coming for you.");
        dm.QueueDialogue("I'll track his IP......");
        dm.QueueDialogue("Oh thats lucky... looks like he's on this train from Odessa to Ogden, he's gonna get it!");

        yield return new WaitWhile(() => !dm.Done());


        scene2.SetActive(false);
        scene3.SetActive(true);

        dm.ChangeYapper(hamBankman);

        dm.QueueDialogue("Let the crypto carnage begin!!!");

        yield return new WaitWhile(() => !dm.Done());


        scene2.SetActive(true);
        scene3.SetActive(false);

        dm.ChangeYapper(johnchimp);
        dm.QueueDialogue("Ok time to get my dog nft back!");

        yield return new WaitWhile(() => !dm.Done());

        //scene2.SetActive(false);

        var fstart = Time.time;
        var fend = fstart + 1;
        while (Time.time < fend) {
            ftb.color = new Color(ftb.color.r, ftb.color.g, ftb.color.b, (Time.time - fstart));
            yield return new WaitForSeconds(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) {
            Time.timeScale = 2;
        } else if (Input.GetKeyUp(KeyCode.Space)) {
            Time.timeScale = 1;
        }
    }
}
