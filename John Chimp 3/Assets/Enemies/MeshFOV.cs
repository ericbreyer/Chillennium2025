using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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

    MeshRenderer meshRenderer;
    Color fovDefaultColor = new Color(0.5f, 0, 0, 0.5f);
    Color fovSpottedColor = new Color(1, 0, 0, 0.5f);

    [Header("Debug Options")]
    public Color fovColor = new Color(0f, 1f, 0f, 0.25f); // Semi-transparent green

    private Mesh CreateTriangleMesh(Vector3 leftB, Vector3 rightB)
    {
        mesh = new Mesh();

        Vector3[] vertices = new Vector3[3]
        {
            transform.position,
            transform.position + leftB, 
            transform.position + rightB,
        };

        for (int i = 0; i < 3; i++)
        {
            vertices[i] = transform.InverseTransformPoint(vertices[i]);
        }

        int[] triangles = new int[3]
        { 0, 1, 2,};

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        return mesh;
    }
    
    
    private void OnDrawGizmos()
    {
        Gizmos.color = fovColor;

        // Draw FOV as a filled triangle in the Scene view
        Vector3 leftBoundary = fov.DirFromAngle(-fov.viewAngle / 2, true) * fov.viewRadius;
        Vector3 rightBoundary = fov.DirFromAngle(fov.viewAngle / 2, true) * fov.viewRadius;

        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);

        // Draw triangle-like FOV area
        Gizmos.DrawMesh(CreateTriangleMesh(leftBoundary, rightBoundary));
    }


    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        fov = GetComponent<FOV>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.color = fovDefaultColor;

    }

    void MakeMesh()
    {
        Color MeshColor = fovDefaultColor;
        if (fov.visible)
        {
            MeshColor = fovSpottedColor;
        }
        
        
        stepCount = Mathf.RoundToInt(fov.viewAngle * meshRes);
        float stepAngle = fov.viewAngle / stepCount;

        //view vertices is a list of vertices where each is the point where the nth raycast hit an object
        List<Vector3> viewVertices = new List<Vector3>();

        hit = new RaycastHit2D();

        for (int s = 0; s <= stepCount; s++)
        {
            float angle = fov.transform.eulerAngles.z - fov.viewAngle / 2 + stepAngle * s;
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
        Color[] colors = new Color[vertexCount];
        triangles = new int[(vertexCount - 2) * 3];

        //this represents the point at the observer
        vertices[0] = Vector3.zero;
        colors[0] = MeshColor;

        for(int i = 0; i < vertexCount - 1; i++)
        {

            vertices[i + 1] = transform.InverseTransformPoint(viewVertices[i]);
            colors[i + 1] = MeshColor;
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
        mesh.SetColors(colors);
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        

    }


    // Update is called once per frame
    void Update()
    {
        MakeMesh();
        if (fov.visible)
        {
            meshRenderer.material.color = fovSpottedColor;
        }
        else
        {
            meshRenderer.material.color = fovDefaultColor;
        }
    }
}
