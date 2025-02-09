using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startpos;
    public GameObject cam;
    public float parallaxEffect;
    public float destroyXPosition = 15f;         // When to destroy

    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector3.right * parallaxEffect * Time.deltaTime);

         // Check if it's gone too far
        if (transform.position.x >= destroyXPosition)
        {
            // Spawn new object at start position
            Vector3 spawnPosition = new Vector3(startpos, transform.position.y, transform.position.z);
            Instantiate(this.gameObject, spawnPosition, Quaternion.identity);
            
            // Destroy current object
            Destroy(gameObject);
        }
    }
}
