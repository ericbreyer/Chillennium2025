using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wantedposter : MonoBehaviour
{


    [SerializeField]
    Sprite[] hatOption;
    [SerializeField]
    SpriteRenderer hat;


    // Start is called before the first frame update
    void Start()
    {
        hat.sprite = hatOption[Random.Range(0, hatOption.Length)];   
    }
}
