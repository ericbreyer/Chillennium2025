using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bigzoom : MonoBehaviour
{

    public float startDelay = .1f;
    public float zoomtime = 10;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.localScale = Vector3.zero;
        StartCoroutine(zooooom());
    }

    IEnumerator zooooom() {
        yield return new WaitForSeconds(startDelay);
        var zoomstart = Time.time;
        var zoomend = zoomstart + zoomtime;
        while(Time.time < zoomend) {
            Debug.Log("MGMGMGM " + Time.time + " " + zoomend);
            this.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, (Time.time - zoomstart) / zoomtime);
            yield return new WaitForSeconds(.00f);
            Debug.Log("MGMGMGM " + Time.time + " " + zoomend);

        }

    }

}
