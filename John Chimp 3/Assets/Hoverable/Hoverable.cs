using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Hoverable : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D bc;
    [SerializeField]
    private Sprite sp;
    [SerializeField]
    private string tooltip;

    private GameObject hilight;
    private TextMeshPro text;
    private TextMeshPro textbg;
    private double showTextStart;

    private static int charsPerSec = 50;

    public void init(BoxCollider2D bc, Sprite sp, string tooltip) {
        this.bc = bc;
        this.sp = sp;
        this.tooltip = tooltip;
        _init();
    }

    void _init() { 
        this.hilight = new GameObject("hilight");
        this.hilight.SetActive(false);
        this.hilight.transform.parent = this.gameObject.transform;
        var hl =   this.hilight.AddComponent<SpriteRenderer>();
        hl.sprite = sp;
        hl.transform.localScale = new Vector2(1.25f, 1.25f);
        hl.transform.localPosition = Vector3.zero;
        hl.color = Color.yellow;
        hl.sortingLayerName = "hilight";


        var text = new GameObject("txt");
        var textbg = new GameObject("txtbg");

        textbg.transform.parent = this.hilight.transform;
        text.transform.parent = this.hilight.transform;
        
        var t = text.AddComponent<TextMeshPro>();
        var tbg = textbg.AddComponent<TextMeshPro>();
        this.text = t;
        this.textbg = tbg;
        t.transform.localPosition = new Vector2(0, .5f);
        t.rectTransform.sizeDelta = new Vector2(2, 10);
        t.alignment = TextAlignmentOptions.BottomLeft;
        //t.enableWordWrapping = true;
        t.rectTransform.pivot = new Vector2(.5f, 0);
        t.fontSize = 3;
        t.sortingOrder = 1;

        tbg.transform.localPosition = new Vector3(0, .5f, -.1f);
        tbg.rectTransform.sizeDelta = new Vector2(2, 10);
        tbg.alignment = TextAlignmentOptions.BottomLeft;
        //tbg.enableWordWrapping = true;
        tbg.rectTransform.pivot = new Vector2(.5f, 0);
        tbg.fontSize = 3;

    }


    private void OnMouseEnter() {
        this.hilight.SetActive(true);
        this.showTextStart = Time.timeAsDouble;
    }

    private void OnMouseExit() {
        this.hilight.SetActive(false);
    }

    private void Update() {
        var charsToShow = (int)((Time.timeAsDouble - this.showTextStart) * (double)charsPerSec);
        charsToShow = Mathf.Min(charsToShow, tooltip.Length);
        this.text.text = this.tooltip.Substring(0, charsToShow) ;
        this.textbg.text = "<mark=#000000 padding=\"10, 10, 10, 10\">" + this.tooltip.Substring(0, charsToShow) + "</mark>";
    }
}
