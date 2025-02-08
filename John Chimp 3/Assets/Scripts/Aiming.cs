using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Aiming : MonoBehaviour {
    public GameObject Gun;
    public Rigidbody2D Rb;

    private Camera cam;
    private float radToDegree = 57.2957795131f;

    // Start is called before the first frame update
    void Start() {
        Rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;

    }

    // Update is called once per frame
    void FixedUpdate() {
        updateGunDir();
    }


    void updateGunDir() {
        Vector3 mousePos = Input.mousePosition;
        Vector3 mousePosWorld = cam.ScreenToWorldPoint(mousePos);
        Vector3 linebw = mousePosWorld - transform.position;
        float angle = -1 * Mathf.Atan2(linebw.x, linebw.y) * radToDegree; //negative because unity rotation works in the opposite direction I want
        Gun.transform.eulerAngles = new Vector3(0, 0, angle + 90);
    }

}