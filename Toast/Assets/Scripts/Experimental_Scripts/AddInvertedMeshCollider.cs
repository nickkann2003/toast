using UnityEngine;
using System.Linq;
using System.Collections;

/* https://forum.unity.com/threads/can-you-invert-a-sphere-or-box-collider.118733/ */

public class AddInvertedMeshCollider : MonoBehaviour
{
    public bool removeExistingColliders = true;

    public void CreateInvertedMeshCollider()
    {
        if (removeExistingColliders)
            RemoveExistingColliders();

        InvertMesh();

        gameObject.AddComponent<MeshCollider>();
    }

    private void RemoveExistingColliders()
    {
        Collider[] colliders = GetComponents<Collider>();
        for (int i = 0; i < colliders.Length; i++)
            DestroyImmediate(colliders[i]);
    }

    private void InvertMesh()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.triangles = mesh.triangles.Reverse().ToArray();
    }
}