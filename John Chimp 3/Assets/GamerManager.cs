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
        FindObjectOfType<DialogueuwManager>().QueueDialogue("Yo Mista White");
        FindObjectOfType<DialogueuwManager>().QueueDialogue("aoughapinowrblnwoibhwoibhnwoerinb");
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
