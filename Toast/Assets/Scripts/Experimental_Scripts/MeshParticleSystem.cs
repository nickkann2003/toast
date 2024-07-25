using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MeshParticleSystem : MonoBehaviour
{
    private const int MAX_QUAD_AMOUNT = 15000;
    private Mesh mesh;

    // set in the editor using pixel values
    [System.Serializable]
    public struct ParticleUVPixels
    {
        public Vector2Int uv00Pixels;
        public Vector2Int uv11Pixels;
    }
    // holds normalized texture UV coordinates
    private struct UVCoords
    {
        public Vector2 uv00;
        public Vector2 uv11;
    }

    [SerializeField]
    private ParticleUVPixels[] particleUVPixelsArray;
    private UVCoords[] uvCoordsArray;

    private Vector3[] vertices;
    //private Vector2[] uv;
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

        vertices = new Vector3[8 * MAX_QUAD_AMOUNT];
        //uv = new Vector2[8 * MAX_QUAD_AMOUNT];
        triangles = new int[6 * MAX_QUAD_AMOUNT];

        //for (int i = 0; i < MAX_QUAD_AMOUNT; i++)
        //{
        //    CreateParticle(new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f)));
        //}

        // set up internal UV normalized array
        Material material = GetComponent<MeshRenderer>().material;
        Texture mainTexture = material.mainTexture;
        int textureWidth = mainTexture.width;
        int textureHeight = mainTexture.height;

        //List<UVCoords> uvCoordsList = new List<UVCoords>();
        //foreach (ParticleUVPixels particleUVPixels in particleUVPixelsArray)
        //{
        //    UVCoords uvCoords = new UVCoords
        //    {
        //        uv00 = new Vector2((float)particleUVPixels.uv00Pixels.x / textureWidth, (float)particleUVPixels.uv00Pixels.x / textureHeight),
        //        uv11 = new Vector2((float)particleUVPixels.uv11Pixels.x / textureWidth, (float)particleUVPixels.uv11Pixels.x / textureHeight)
        //    };
        //    uvCoordsList.Add(uvCoords);
        //}
        //uvCoordsArray = uvCoordsList.ToArray();
    }

    //private void Update()
    //{
    //    for (int i = 0; i < quadIndex; i++)
    //    {
    //        Vector3 eulerAngles = Camera.main.transform.eulerAngles;
    //        Quaternion rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z);
    //        Matrix4x4 m = Matrix4x4.Rotate(rotation);

    //        int vertIndex = i * 4;
    //        vertices[vertIndex + 0] = m.MultiplyPoint3x4(vertices[vertIndex + 0]);
    //        vertices[vertIndex + 1] = m.MultiplyPoint3x4(vertices[vertIndex + 1]);
    //        vertices[vertIndex + 2] = m.MultiplyPoint3x4(vertices[vertIndex + 2]);
    //        vertices[vertIndex + 3] = m.MultiplyPoint3x4(vertices[vertIndex + 3]);
    //    }

    //    UpdateMesh();
    //}

    public void CreateParticle(Vector3 position)
    {
        if (quadIndex >= MAX_QUAD_AMOUNT) return;

        int vertIndex = quadIndex * 8;
        vertices[vertIndex + 0] = position + new Vector3(-size/2, -size/2, 0);
        vertices[vertIndex + 1] = position + new Vector3(-size/2, size/2, 0);
        vertices[vertIndex + 2] = position + new Vector3(size/2, -size/2, 0);
        vertices[vertIndex + 3] = position + new Vector3(size/2, size/2, 0);

        int triIndex = quadIndex * 36;
        triangles[triIndex + 0] = vertIndex + 0;
        triangles[triIndex + 1] = vertIndex + 1;
        triangles[triIndex + 2] = vertIndex + 2;
        triangles[triIndex + 3] = vertIndex + 1;
        triangles[triIndex + 4] = vertIndex + 3;
        triangles[triIndex + 5] = vertIndex + 2;

        quadIndex++;

        UpdateMesh();
    }

    public void CreateParticle(Vector3 position, Vector3 eulerAngles, float sizeMod = 1)
    {
        if (quadIndex >= MAX_QUAD_AMOUNT) return;

        float modifiedSize = size * sizeMod;

        int vertIndex = quadIndex * 4;
        vertices[vertIndex + 0] = new Vector3(-modifiedSize / 2, -modifiedSize / 2, 0);
        vertices[vertIndex + 1] = new Vector3(-modifiedSize / 2, modifiedSize / 2, 0);
        vertices[vertIndex + 2] = new Vector3(modifiedSize / 2, -modifiedSize / 2, 0);
        vertices[vertIndex + 3] = new Vector3(modifiedSize / 2, modifiedSize / 2, 0);

        if (eulerAngles.y != 0)
        {
            eulerAngles.x = 0;
            eulerAngles.z = 0;

            Quaternion rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z);
            Matrix4x4 m = Matrix4x4.Rotate(rotation);

            vertices[vertIndex + 0] = m.MultiplyPoint3x4(vertices[vertIndex + 0]);
            vertices[vertIndex + 1] = m.MultiplyPoint3x4(vertices[vertIndex + 1]);
            vertices[vertIndex + 2] = m.MultiplyPoint3x4(vertices[vertIndex + 2]);
            vertices[vertIndex + 3] = m.MultiplyPoint3x4(vertices[vertIndex + 3]);
        }

        vertices[vertIndex + 0] += position;
        vertices[vertIndex + 1] += position;
        vertices[vertIndex + 2] += position;
        vertices[vertIndex + 3] += position;

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

    public void CreateCube(Vector3 position, float toastiness = 0, float sizeMod = 1)
    {
        if (quadIndex >= MAX_QUAD_AMOUNT) return;
        if (sizeMod >= 2)
        {
            sizeMod = 10;
        }

        float modifiedSize = size * sizeMod;

        int vertIndex = quadIndex * 8;
        vertices[vertIndex + 0] = position + new Vector3(-modifiedSize / 2, -modifiedSize / 2, -modifiedSize / 2);
        vertices[vertIndex + 1] = position + new Vector3(modifiedSize / 2, -modifiedSize / 2, -modifiedSize / 2);
        vertices[vertIndex + 2] = position + new Vector3(modifiedSize / 2, modifiedSize / 2, -modifiedSize / 2);
        vertices[vertIndex + 3] = position + new Vector3(-modifiedSize / 2, modifiedSize / 2, -modifiedSize / 2);
        vertices[vertIndex + 4] = position + new Vector3(-modifiedSize / 2, modifiedSize / 2, modifiedSize / 2);
        vertices[vertIndex + 5] = position + new Vector3(modifiedSize / 2, modifiedSize / 2, modifiedSize / 2);
        vertices[vertIndex + 6] = position + new Vector3(modifiedSize / 2, -modifiedSize / 2, modifiedSize / 2);
        vertices[vertIndex + 7] = position + new Vector3(-modifiedSize / 2, -modifiedSize / 2, modifiedSize / 2);

        int triIndex = quadIndex * 36;
        triangles[triIndex + 0] = vertIndex + 0;
        triangles[triIndex + 1] = vertIndex + 2;
        triangles[triIndex + 2] = vertIndex + 1;
        triangles[triIndex + 3] = vertIndex + 0;
        triangles[triIndex + 4] = vertIndex + 3;
        triangles[triIndex + 5] = vertIndex + 2;
        triangles[triIndex + 6] = vertIndex + 2;
        triangles[triIndex + 7] = vertIndex + 3;
        triangles[triIndex + 8] = vertIndex + 4;
        triangles[triIndex + 9] = vertIndex + 2;
        triangles[triIndex + 10] = vertIndex + 4;
        triangles[triIndex + 11] = vertIndex + 5;
        triangles[triIndex + 12] = vertIndex + 1;
        triangles[triIndex + 13] = vertIndex + 2;
        triangles[triIndex + 14] = vertIndex + 5;
        triangles[triIndex + 15] = vertIndex + 1;
        triangles[triIndex + 16] = vertIndex + 5;
        triangles[triIndex + 17] = vertIndex + 6;
        triangles[triIndex + 18] = vertIndex + 0;
        triangles[triIndex + 19] = vertIndex + 7;
        triangles[triIndex + 20] = vertIndex + 4;
        triangles[triIndex + 21] = vertIndex + 0;
        triangles[triIndex + 22] = vertIndex + 4;
        triangles[triIndex + 23] = vertIndex + 3;
        triangles[triIndex + 24] = vertIndex + 5;
        triangles[triIndex + 25] = vertIndex + 4;
        triangles[triIndex + 26] = vertIndex + 7;
        triangles[triIndex + 27] = vertIndex + 5;
        triangles[triIndex + 28] = vertIndex + 7;
        triangles[triIndex + 29] = vertIndex + 6;
        triangles[triIndex + 30] = vertIndex + 0;
        triangles[triIndex + 31] = vertIndex + 6;
        triangles[triIndex + 32] = vertIndex + 7;
        triangles[triIndex + 33] = vertIndex + 0;
        triangles[triIndex + 34] = vertIndex + 1;
        triangles[triIndex + 35] = vertIndex + 6;

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
        //mesh.uv = uv;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }
}
