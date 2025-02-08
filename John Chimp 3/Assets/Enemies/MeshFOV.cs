using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshFOV : MonoBehaviour
{
    public FOV fov;
    Mesh mesh;
    RaycastHit2D hit;
    [SerializeField] float meshRes;
    [HideInInspector] public Vector3[] vertex;
    [HideInInspector] public int[] triangles;
    [HideInInspector] public int stepCount;
    Vector3[] vertices;
    
    
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        fov = GetComponent<FOV>();
    }

    void MakeMesh()
    {
        stepCount = Mathf.RoundToInt(fov.viewAngle * meshRes);
        float stepAngle = fov.viewAngle / stepCount;

        //view vertices is a list of vertices where each is the point where the nth raycast hit an object
        List<Vector3> viewVertices = new List<Vector3>();

        hit = new RaycastHit2D();

        for (int s = 0; s <= stepCount; s++)
        {
            float angle = fov.transform.eulerAngles.y - fov.viewAngle / 2 + stepAngle * s;
            Vector3 dir = fov.DirFromAngle(angle, false);

            hit = Physics2D.Raycast(fov.transform.position, dir, fov.viewRadius, fov.obstacleMask);

            if (hit.collider == null)
            {
                viewVertices.Add(transform.position + dir.normalized * fov.viewRadius);
            }
            else
            {
                viewVertices.Add(transform.position + dir.normalized * hit.distance);
            }
        }

        int vertexCount = viewVertices.Count + 1;

        vertices = new Vector3[vertexCount];
        triangles = new int[(vertexCount - 2) * 3];

        //this represents the point at the observer
        vertices[0] = Vector3.zero;

        for(int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewVertices[i]);

            //this represents the indices into viewVertices
            if(i < vertexCount - 2)
            {
                triangles[i * 3 + 2] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3] = i + 2;
            }
            
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

    }


    // Update is called once per frame
    void Update()
    {
        MakeMesh();
    }
}
