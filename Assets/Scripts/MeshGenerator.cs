using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    
    public int xSize = 20;
    public int zSize = 20;   

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        StartCoroutine(CreateShape());
       // UpdateMesh();
    }

    void Update()
    {
        UpdateMesh();
    }

    IEnumerator CreateShape()
    {
        /*vertices = new Vector3[]
        {
            new Vector3 (0,0,0),
            new Vector3 (0,0,1),
            new Vector3 (1,0,0),
            new Vector3 (1,0,1)
        };

        triangles = new int[]
        {
            0, 1, 2,
            3, 2, 1
        };*/

        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for(int i = 0, z = 0; z <= zSize; z++)
        {
            for(int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * .3f, z * .3f) * 2f;
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        triangles = new int[vertices.Length * 6];
        
        for(int z = 0, t = 0; z < zSize; z++) // Manages Z-axis of the triangle
        {
            for(int x = 0; x < xSize; x++) // Manages X-axis of the triangle
            {
                triangles[t + 0] = (z * (zSize + 1)) + x;
                triangles[t + 1] = (z * (zSize + 1)) + x + xSize + 1;
                triangles[t + 2] = (z * (zSize + 1)) + x + 1;
                triangles[t + 3] = (z * (zSize + 1)) + x + 1; 
                triangles[t + 4] = (z * (zSize + 1)) + x + xSize + 1;
                triangles[t + 5] = (z * (zSize + 1)) + x + zSize + 2;
                t += 6; // Add 6 to make sure not to overwrite the implemented triangles
                yield return new WaitForSeconds(.001f);
            }
           
        }

        /*
        Brackeys' code

        int vert = 0;
        int tris = 0;

        for(int z = 0; z < zSize; z++) 
        {
            for(int x = 0; x < xSize; x++) 
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1; 
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;
                
                vert++;
                tris += 6; 
                
                yield return new WaitForSeconds(.1f);
            }
            vert++;
        }
        */
        
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    void OnDrawGizmos() // Debugging Gizmos
    {
        if(vertices == null)
            return;

        for(int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], .1f);
        }
    }
}
