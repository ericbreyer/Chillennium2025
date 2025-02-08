using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueuwManager : MonoBehaviour
{

    [SerializeField]
    private Image yapper;
    [SerializeField]
    private TextMeshProUGUI yappage;
    [SerializeField]
    private Canvas c;

    private Queue<string> yapqueue = new Queue<string>();

    private double yapstart;
    private int yapspeed = 20;
    private string to_yap = "";

    // Start is called before the first frame update
    void Start()
    {
        c.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (to_yap.Length > 0) {
            var charsToShow = (int)((Time.timeAsDouble - this.yapstart) * (double)yapspeed);
            charsToShow = Mathf.Min(charsToShow, to_yap.Length);
            this.yappage.text = this.to_yap.Substring(0, charsToShow);
        } else if(yapqueue.Count > 0) {

            StartCoroutine(ShowOneDialogue(yapqueue.Dequeue()));
        } else {
            c.gameObject.SetActive(false);
        }
    }

    public void QueueDialogue(string t) {
        yapqueue.Enqueue(t);
    }

    private IEnumerator ShowOneDialogue(string text) {
        c.gameObject.SetActive(true);

        this.yapstart = Time.timeAsDouble;
        this.to_yap = text;
        int charsShown;
        yield return new WaitWhile(() => {
            charsShown = (int)((Time.timeAsDouble - this.yapstart) * (double)yapspeed);
            return charsShown < to_yap.Length;
        });
        if (yapqueue.Count > 0) {
            yield return new WaitForSeconds(2f);
        }
        else {
            yield return new WaitForSeconds(3.5f);
        }
        this.to_yap = "";
    }

}
