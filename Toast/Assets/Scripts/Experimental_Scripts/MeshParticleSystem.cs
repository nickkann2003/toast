using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshParticleSystem : MonoBehaviour
{
    private const int MAX_QUAD_AMOUNT = 15000;
    private Mesh mesh;

    private Vector3[] vertices;
    private int[] triangles;

    private int quadIndex;

    [SerializeField]
    private float size = .05f;

    // Singleton
    public static MeshParticleSystem instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        vertices = new Vector3[4 * MAX_QUAD_AMOUNT];
        triangles = new int[6 * MAX_QUAD_AMOUNT];

        //for (int i = 0; i < MAX_QUAD_AMOUNT; i++)
        //{
        //    CreateParticle(new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f)));
        //}
    }

    private void Update()
    {
        //CreateParticle(new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f)));

        //RemoveParticle(Random.Range(0, MAX_QUAD_AMOUNT));
        //for (int i = 0; i < quadIndex; i++)
        //{
        //    int vertIndex = quadIndex * 4;
        //    vertices[i]
        //}
    }

    public void CreateParticle(Vector3 position)
    {
        if (quadIndex >= MAX_QUAD_AMOUNT) return;

        int vertIndex = quadIndex * 4;
        vertices[vertIndex + 0] = position + new Vector3(-size/2, -size/2, 0);
        vertices[vertIndex + 1] = position + new Vector3(-size/2, size/2, 0);
        vertices[vertIndex + 2] = position + new Vector3(size/2, -size/2, 0);
        vertices[vertIndex + 3] = position + new Vector3(size/2, size/2, 0);

        int triIndex = quadIndex * 6;
        triangles[triIndex + 0] = vertIndex + 0;
        triangles[triIndex + 1] = vertIndex + 1;
        triangles[triIndex + 2] = vertIndex + 2;
        triangles[triIndex + 3] = vertIndex + 1;
        triangles[triIndex + 4] = vertIndex + 3;
        triangles[triIndex + 5] = vertIndex + 2;

        quadIndex++;

        UpdateMesh();
    }

    private void RemoveParticle(int index)
    {
        int vertIndex = index * 4;
        vertices[vertIndex + 0] = Vector3.zero;
        vertices[vertIndex + 0] = Vector3.zero;
        vertices[vertIndex + 0] = Vector3.zero;
        vertices[vertIndex + 0] = Vector3.zero;

        int triIndex = index * 6;
        triangles[triIndex + 0] = 0;
        triangles[triIndex + 1] = 0;
        triangles[triIndex + 2] = 0;
        triangles[triIndex + 3] = 0;
        triangles[triIndex + 4] = 0;
        triangles[triIndex + 5] = 0;

        UpdateMesh();
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }
}
