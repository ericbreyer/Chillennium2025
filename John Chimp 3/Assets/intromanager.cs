using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class intromanager : MonoBehaviour
{

    public Sprite johnchimp;
    public Sprite hamBankman;
    public GameObject scene1;
    public GameObject scene2;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(timeline());
    }

    IEnumerator timeline() {
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
        vp.enabled = false;

        dm.ChangeYapper(johnchimp);

        dm.QueueDialogue("That's how you turn a couple grand into a couple million, baby! Buy the dip, ride the wave, then dump it all — BOOM, profit!");
        dm.QueueDialogue("JOHN CHIMP gets it done!!");
        dm.QueueDialogue("These diamond hands never loose");
        dm.QueueDialogue("My dog may be dead, but at least I've got his NFT and a couple mil in the bank");

        yield return new WaitWhile(() => !dm.Done());

        dm.ChangeYapper(hamBankman);
        dm.QueueDialogue("Look at this idiot.Flashing his gains like he’s untouchable.Let him enjoy it while it lasts");
        dm.QueueDialogue("You may have pulled one over on me, but I'll get you back JOHN CHIMP");
        dm.QueueDialogue("I’ve got something special planned for you, Chimp.You think you’re the king of crypto ? Well, I, HAM BANKMAN,  really pull the strings");
        dm.QueueDialogue("This NFT… it’s the last thing he’s got left of that dog. Time to make him really feel the loss");
        dm.QueueDialogue("By the time he realizes what’s happening, it’ll be too late. And when he comes crawling for it? We’ll see how he handles a real player");
        dm.QueueDialogue("Game on, Chimp. You’re about to learn what it means to mess with me");


        yield return new WaitWhile(() => !dm.Done());

        dm.ChangeYapper(johnchimp);
        dm.QueueDialogue("No way... I just got rug-pulled!");
        dm.QueueDialogue("My NFT of my dead dog is gone!!! That was my last connection to him!");
        dm.QueueDialogue("Alright, whoever did this is going down. I’m coming for you.");
        dm.QueueDialogue("I'll track his IP......");
        dm.QueueDialogue("Alright, looks like he's on the train from Odessa to Ogden, he's gonna get it!");

        yield return new WaitWhile(() => !dm.Done());

        dm.ChangeYapper(hamBankman);
        dm.QueueDialogue("It’s time. Chimp thinks he’s untouchable, but that’s about to change");
        dm.QueueDialogue("I’m putting a bounty on his head. Whoever brings me John Chimp, alive or dead, gets the reward");
        dm.QueueDialogue("I know he's gonna be on that train, what a predictable little primate");
        dm.QueueDialogue("\"Attention all crypto killers!...\"");
        dm.QueueDialogue("\"Chimp has something that belongs to me, and I’m going to make sure he regrets ever crossing me\"");
        dm.QueueDialogue("\"Whoever gets to him first gets the glory. The rest? Well, let’s just say... there’s no second place in this game\"");
        dm.QueueDialogue("Let the crypto carnage begin!!!");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
