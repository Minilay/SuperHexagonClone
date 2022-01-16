using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class PolygonItem : MonoBehaviour
{
    //TODO: field refactor
    [Header("Polygon Settings")]
    [Range(3, 18)]
    public int vertexAmount = 3;
    [Range(1.0f, 10.0f)]
    public float radius = 1;

    [Range(0, 120)]
    public float phase = 0;
    public bool withAHole = false;
    [Range(1, 100f)]
    public float innerRadiusFraction = 80f;
    public GameObject parent;
    public Material material;

    [Header("Collider Settings")]
    public bool hasCollider = false;
    private GameObject _polygon;
    private GameObject _go;
    private MeshFilter _meshFilter;
    private PolygonCollider2D _coll;


    private void OnValidate() //It works when script has compiled. Scripts compiles when fields are changed
    {
        _go = _polygon != null ? _polygon : gameObject;
        if (parent != null)
            _go.transform.SetParent(parent.transform);

        MeshRenderer meshRenderer = (_go.GetComponent<MeshRenderer>() == null) ? _go.AddComponent<MeshRenderer>() : _go.GetComponent<MeshRenderer>();
        
        meshRenderer.sharedMaterial = (material == null) ? new Material(Shader.Find("Standard")) : material;
        
        _meshFilter = _go.GetComponent<MeshFilter>() == null ? _go.AddComponent<MeshFilter>() : _go.GetComponent<MeshFilter>();
        
        if (hasCollider)
            _coll = _go.GetComponent<PolygonCollider2D>() == null ? _go.AddComponent<PolygonCollider2D>() : _go.GetComponent<PolygonCollider2D>();

        
        if (!withAHole)
        {
            SolidPolyGen();
        }
        else
        {
            HoledPolyGen();
        }

        if (hasCollider)
        {
            AddCollider();
        }
    }
    

    private void SolidPolyGen()
    {
        
        Mesh mesh = new Mesh();
        Vector3[] verts = new Vector3[vertexAmount];
        Vector2[] uv = new Vector2[vertexAmount];
        int[] tris = new int[3 * (vertexAmount - 2)];
        Vector3[] normals = new Vector3[vertexAmount];
        
        
        float x, y;
        float angleStep = 360.0f / vertexAmount;
        for (int i = 0; i < vertexAmount; i++)
        {
            x = Mathf.Cos(((i * angleStep + phase) * Mathf.Deg2Rad));
            y = Mathf.Sin(((i * angleStep + phase) * Mathf.Deg2Rad));

            verts[vertexAmount - i - 1] = new Vector3(x, y) * radius;
            uv[vertexAmount - i - 1] = new Vector3(x, y);
            normals[i] = -Vector3.forward;
        }

        for (int i = 0; i < vertexAmount - 2; i++)
        {
            tris[i * 3] = i + 2;
            tris[i * 3 + 1] = 0;
            tris[i * 3 + 2] = i + 1;
        }
        
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.normals = normals;
        mesh.uv = uv;

        _meshFilter.mesh = mesh;
    }     

    private void HoledPolyGen()
    {
         
        var mesh = new Mesh();
        var verts = new Vector3[vertexAmount * 2];
        var uv = new Vector2[vertexAmount * 2];
        var tris = new int[vertexAmount * 6];
        var normals = new Vector3[vertexAmount * 2];
        
        
        float x, y;
        var angleStep = 360.0f / vertexAmount;
        
        for (var i = 0; i < vertexAmount; i++)
        {
            x = Mathf.Cos((i * angleStep + phase)  * Mathf.Deg2Rad);
            y = Mathf.Sin((i * angleStep + phase) * Mathf.Deg2Rad);

            verts[vertexAmount - i - 1] = new Vector3(x, y) * radius;
            uv[vertexAmount - i - 1] = new Vector3(x, y);
            normals[i] = -Vector3.forward;
            
            verts[vertexAmount * 2 - i - 1] = new Vector3(x, y) * (radius * innerRadiusFraction / 100f);
            uv[vertexAmount * 2- i - 1] = new Vector3(x, y) * (innerRadiusFraction / 100);
            normals[vertexAmount + i] = -Vector3.forward;
        }

        for (var i = 0; i < vertexAmount; i++)
        {
            tris[i * 3] = i;
            tris[i * 3 + 1] = (i + 1) % vertexAmount;
            tris[i * 3 + 2] = i + vertexAmount;

            tris[vertexAmount * 3 + i * 3] = i;
            tris[vertexAmount * 3 + i * 3 + 1] = i + vertexAmount;
            tris[vertexAmount * 3 + i * 3 + 2] = (i + vertexAmount - 1) % vertexAmount + vertexAmount;

        }
        
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.normals = normals;
        mesh.uv = uv;

        _meshFilter.mesh = mesh;
    }

    private void AddCollider()
    {

        Vector2[] verts = new Vector2[vertexAmount];
        float x, y;
        float angleStep = 360.0f / vertexAmount;
        for (int i = 0; i < vertexAmount; i++)
        {
            x = Mathf.Cos(((i * angleStep + phase) * Mathf.Deg2Rad));
            y = Mathf.Sin(((i * angleStep + phase) * Mathf.Deg2Rad));

            verts[vertexAmount - i - 1] = new Vector2(x, y) * radius;
        }

        _coll.points = verts;
    }
}
