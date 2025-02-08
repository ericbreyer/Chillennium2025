using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamerManager : MonoBehaviour
{

    public Scene[] scenes;
    private int curScene = 0;
    private SpriteRenderer bigFadeBoi;
    private float fadeNum = 0;

    void Awake() {
        var objs = GameObject.FindObjectsOfType<GamerManager>();

        if (objs.Length > 1) {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }


    public void Respawn() {
        StartCoroutine(FadeCoroutine( () => 
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex)
        ));
    }

    private IEnumerator FadeCoroutine(Action a) {
        float SPEEEEEEEEÈD = .01f;
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

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
