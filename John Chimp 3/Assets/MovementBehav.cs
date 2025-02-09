using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
public enum movType
    {
        move,
        jump,
        grapple,
        swing,
        sit,
        nft,
        hide,
        nextLevel,
        hat
    }

public enum hatTypes
{
    fedora, tophat, beret, propeller, dumbldor
}

[System.Serializable]
public class MovementBehav : MonoBehaviour
{
    public GameObject movPoint;
    public int facingDir;
    
    public movType behav;

    private SpriteRenderer sequenceTooltip;
    private TextMeshPro text;
    private playerMovement player;
    public int hatType;
    public Sprite[] hats;




    //TODO - add mouse highlight function for this object

    // Start is called before the first frame update
    void Start()
    {
        var circle = AssetDatabase.LoadAssetAtPath<Sprite>("Packages/com.unity.2d.sprite/Editor/ObjectMenuCreation/DefaultAssets/Textures/v2/Circle.png");
        var go = new GameObject("seqtt");
        this.sequenceTooltip = go.AddComponent<SpriteRenderer>();
        this.sequenceTooltip.sprite = circle;
        this.sequenceTooltip.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        this.sequenceTooltip.color = new Color(1f, 0f, 0f, 0.7f);
        this.sequenceTooltip.transform.parent = this.movPoint.transform;
        this.sequenceTooltip.transform.localPosition = new Vector2(0, 0);
        this.sequenceTooltip.sortingLayerName = "tooltip";

        var tgo = new GameObject("seqttt");
        this.text = tgo.AddComponent<TextMeshPro>();
        this.text.transform.parent = this.sequenceTooltip.transform;
        this.text.transform.localPosition = new Vector2(0, 0);
        this.text.rectTransform.sizeDelta = new Vector2(2, 1);
        this.text.fontSize = 5;
        this.text.alignment = TextAlignmentOptions.Midline;
        this.text.sortingLayerID = SortingLayer.NameToID("bigtop");

        var hov = this.gameObject.AddComponent<Hoverable>();
        hov.init(this.GetComponent<BoxCollider2D>(), this.GetComponent<SpriteRenderer>().sprite, behav.ToString());


        player = FindObjectOfType<playerMovement>();

        if(behav == movType.hat)
        {
            GetComponent<SpriteRenderer>().sprite = hats[hatType];
        }

    }

    // Update is called once per frame
    void Update()
    {
        var sequenceNum = player.getPlaceInMovementOrder(this);
        if (sequenceNum > -1) {
            sequenceTooltip.gameObject.SetActive(true);
            text.text = (sequenceNum + 1).ToString();
        }
        else {
            sequenceTooltip.gameObject.SetActive(false);
        }
    }

    private void OnMouseDown() {
        player.addToMovementOrder(this);
    }
}
