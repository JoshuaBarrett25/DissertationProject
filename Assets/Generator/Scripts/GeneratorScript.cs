using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


//Require that the component is on the gameobject this script is attached to
[RequireComponent(typeof(MeshFilter))]
public class GeneratorScript : MonoBehaviour
{

    //Debug variables for editor views
    public bool drawVerticesDebug;
    public bool updateInRealTime;

    //Any values set by user in inspector
    public Vector2 dimensions;

    Mesh object_mesh;
    Vector3[] object_vertices;
    int[] object_triangles;


    void Start()
    {
        object_mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = object_mesh;

        StartCoroutine(CreatePlaneMesh());
        UpdateMesh();
    }




    void Update()
    {
        if (updateInRealTime)
        {
            if (dimensions.x <= 1)
            {
                dimensions.x = 1;
            }

            if (dimensions.y <= 1)
            {
                dimensions.y = 1;
            }
            UpdateMesh();
        }
    }

    IEnumerator CreatePlaneMesh()
    {   
        object_vertices = new Vector3[((int)dimensions.x + 1) * ((int)dimensions.y + 1)];
        
        for (int i = 0, z = 0; z <= (int)dimensions.y; z++)
        {
            for (int x = 0; x <= (int)dimensions.x; x++)
            {
                object_vertices[i] = new Vector3(x, 0, z);
                i++;
            }
        }

        int vertices = 0;
        int triangles = 0;
        object_triangles = new int[(int)dimensions.x * (int)dimensions.x * 6];
        
        
        for (int y = 0; y < (int)dimensions.y; y++)
        {
            for (int x = 0; x < (int)dimensions.x; x++)
            {
                object_triangles[triangles + 0] = vertices + 0;
                object_triangles[triangles + 1] = vertices + (int)dimensions.x + 1;
                object_triangles[triangles + 2] = vertices + 1;
                object_triangles[triangles + 3] = vertices + 1;
                object_triangles[triangles + 4] = vertices + (int)dimensions.x + 1;
                object_triangles[triangles + 5] = vertices + (int)dimensions.x + 2;

                vertices++;
                triangles += 6;

                yield return new WaitForSeconds(.01F);
            }
            vertices++;
        }
    }


    void UpdateMesh()
    {
        object_mesh.Clear();
        object_mesh.vertices = object_vertices;
        object_mesh.triangles = object_triangles;
        object_mesh.RecalculateNormals();
    }


   
    private void OnDrawGizmos()
    {
        if (object_vertices == null)
        {
            return;
        }

        if (drawVerticesDebug)
        {
            for (int i = 0; i < object_vertices.Length; i++)
            {
                Gizmos.DrawSphere(object_vertices[i], .1f);
            }
        }
    }
}
