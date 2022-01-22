using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Client._Scripts.Polygon;
using UnityEngine;
using UnityEngine.Experimental.AI;


public class ObstacleCreator : PolygonCreator
{
    [SerializeField] private bool _randomSegmentCount;
    [SerializeField] private int _segmentCount;
    
    public List<int> activeSegments;

    protected new void Awake()
    {
        if (_randomSegmentCount)
            _segmentCount = Random.Range(1, _vertexCount);
        
        _meshFilter = GetComponent<MeshFilter>();
    }
    private void OnValidate()
    {
        Validation();
        
        if (_segmentCount < 1)
            _segmentCount = 1;
            
        if (_segmentCount >= _vertexCount)
            _segmentCount = _vertexCount - 1;
        
    }
    
    public ObstacleCreator CreateSegmented()
    {
        var verts = new Vector3[_vertexCount * 2];
        var uv = new Vector2[_vertexCount * 2];
        var tris = new int[_segmentCount * 6];
        var normals = new Vector3[_vertexCount * 2];
        
        var angleStep = 360.0f / _vertexCount;
    
        for (var i = 0; i < _vertexCount; i++)
        {
            var angle = i * angleStep + _phase;
            var coordinates = Utils.PolarToCartesian(angle);    
            
            verts[_vertexCount - i - 1] = coordinates * _radius;
            uv[_vertexCount - i - 1] = coordinates;
            normals[i] = Vector3.back;

            verts[_vertexCount * 2 - i - 1] = coordinates * (_radius - _width);
            uv[_vertexCount * 2- i - 1] = coordinates * (_width / _radius);
            normals[_vertexCount + i] = Vector3.back;
        }

        if(activeSegments.isEmpty())
            activeSegments = new List<int>(Enumerable.Range(0, _vertexCount));
        
        activeSegments.RemoveRange(_segmentCount, activeSegments.Count - _segmentCount);
        activeSegments.Sort();
        for (var i = 0; i < _segmentCount; i++)
        {
            var segmentNumber = activeSegments[i];
            tris[i * 3] = segmentNumber;
            tris[i * 3 + 1] = (segmentNumber + 1) % _vertexCount;
            tris[i * 3 + 2] = segmentNumber + _vertexCount;

            tris[_segmentCount * 3 + i * 3] = (segmentNumber + 1) % _vertexCount;
            tris[_segmentCount * 3 + i * 3 + 1] = (segmentNumber + 1) % _vertexCount + _vertexCount;
            tris[_segmentCount * 3 + i * 3 + 2] = (segmentNumber + _vertexCount) % _vertexCount + _vertexCount;
        }
        _meshFilter.mesh = MakeMesh(verts, uv, tris, normals);
        return this;
    }

    public ObstacleCreator ShuffleSegments()
    {
        activeSegments =  new List<int>(Enumerable.Range(0, _vertexCount)).Shuffle();
        CreateSegmented();

        return this;
    }
}
