using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class guard : MonoBehaviour
{

    [SerializeField]
    private Vector2 targetPoint;

    private LineRenderer lr;

    [SerializeField]
    private GameObject rifle;

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetPoint, .1f);
        Gizmos.DrawLine(transform.position, targetPoint);
    }

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.SetPosition(0, this.transform.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lr.SetPosition(1, targetPoint);
        Vector3 direction = (targetPoint - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rifle.transform.rotation = Quaternion.Euler(0, 0, angle + 180);






        var cc = Physics2D.OverlapCircle(targetPoint, .1f);
            if (cc != null) {
            var p = cc.GetComponent<playerMovement>();
            if(p != null && !p.dead) {
                FindObjectOfType<GamerManager>().die();
            }
        }
    }
}
