using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HollowPolygonCreator : PolygonCreator
{
    [SerializeField] private float _width;
    protected override void OnValidate()
    {
        base.OnValidate();

        if (_width >= Radius)
            _width = Radius;

        if (_width < 0)
            _width = 0;
    }

    protected override void CalculatePolygonMesh()
    {
        var verts = new Vector3[_vertexCount * 2];
        var uv = new Vector2[_vertexCount * 2];
        var tris = new int[_vertexCount * 6];
        var normals = new Vector3[_vertexCount * 2];
            
        
        for (var i = 0; i < _vertexCount; i++)
        {
            var angle = i * _angleStep + _phase;
            var coordinates = Utils.PolarToCartesian(angle);    
            
            verts[_vertexCount - i - 1] = coordinates * Radius;
            uv[_vertexCount - i - 1] = coordinates;
            normals[i] = Vector3.back;

            verts[_vertexCount * 2 - i - 1] = coordinates * (Radius - _width);
            uv[_vertexCount * 2- i - 1] = coordinates * (_width / Radius);
            normals[_vertexCount + i] = Vector3.back;
        }

        for (var i = 0; i < _vertexCount; i++)
        {
            tris[i * 3] = i;
            tris[i * 3 + 1] = (i + 1) % _vertexCount;
            tris[i * 3 + 2] = i + _vertexCount;

            tris[_vertexCount * 3 + i * 3] = i;
            tris[_vertexCount * 3 + i * 3 + 1] = i + _vertexCount;
            tris[_vertexCount * 3 + i * 3 + 2] = (i + _vertexCount - 1) % _vertexCount + _vertexCount;
        }

        _meshFilter.mesh = MakeMesh(verts, uv, tris, normals);        
    }
}
