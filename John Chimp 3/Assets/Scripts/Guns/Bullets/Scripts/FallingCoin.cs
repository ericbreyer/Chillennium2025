using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingCoin : MonoBehaviour
{
    public float lifeSpan = 1.5f;
    public float age = 0f;

    public AudioClip clip;
    private AudioSource source;
    
    // Start is called before the first frame update
    void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        
        
    }

    private void Update()
    {
        age += Time.deltaTime;
        if(lifeSpan <= age)
            Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        {
            Debug.Log("clink please");
            source.volume = 0.5f * Mathf.Clamp((10 - (transform.position - GameObject.FindWithTag("Player").transform.position).magnitude) / 10, 0, 1);
            source.time = Random.Range(0f, 0.15f);
            source.Play();
        }
    }

}
