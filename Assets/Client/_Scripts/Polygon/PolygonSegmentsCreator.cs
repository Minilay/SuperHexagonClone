using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

//TODO: Create abstract class 
public class PolygonSegmentsCreator : PolygonCreator
{
    [SerializeField] private float _width;
    [field: SerializeField] public bool RandomSegmentCount { get; set; }
    [SerializeField] private int _segmentCount;
    [field: SerializeField] public List<int> ActiveSegments { get;  set; }
    protected override void Awake()
    {
        if (RandomSegmentCount)
            _segmentCount = Random.Range(1, _vertexCount);

        base.Awake();
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        
        if (_segmentCount < 1)
            _segmentCount = 1;
            
        if (_segmentCount >= _vertexCount)
            _segmentCount = _vertexCount;
    
         
        if (_width >= Radius)
            _width = Radius;

        if (_width < 0)
            _width = 0;
    }


    protected override void CalculatePolygonMesh()
    {
        var verts = new Vector3[_vertexCount * 2];
        var uv = new Vector2[_vertexCount * 2];
        var tris = new int[_segmentCount * 6];
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

        if(ActiveSegments.isEmpty())
            ActiveSegments = new List<int>(Enumerable.Range(0, _vertexCount));
        
        ActiveSegments.RemoveRange(_segmentCount, ActiveSegments.Count - _segmentCount);
        ActiveSegments.Sort();
        for (var i = 0; i < _segmentCount; i++)
        {
            var segmentNumber = ActiveSegments[i];
            tris[i * 3] = segmentNumber;
            tris[i * 3 + 1] = (segmentNumber + 1) % _vertexCount;
            tris[i * 3 + 2] = segmentNumber + _vertexCount;

            tris[_segmentCount * 3 + i * 3] = (segmentNumber + 1) % _vertexCount;
            tris[_segmentCount * 3 + i * 3 + 1] = (segmentNumber + 1) % _vertexCount + _vertexCount;
            tris[_segmentCount * 3 + i * 3 + 2] = (segmentNumber + _vertexCount) % _vertexCount + _vertexCount;
        }
        _meshFilter.mesh = MakeMesh(verts, uv, tris, normals);
    }

    protected override IEnumerator SmoothVertexDecrease(float _smoothTransitionTime)
    {
        var startAngle = _angleStep;
        var newAngleStep = 360.0f / (_vertexCount - 1 ) ;

        var currentTime = .0f;
        while (currentTime <= _smoothTransitionTime)
        {
            currentTime += Time.deltaTime;
            CalculatePolygonMesh();
            _angleStep = Mathf.LerpAngle(startAngle, newAngleStep, currentTime / _smoothTransitionTime);
            yield return null;
        }
        _vertexCount--;

        ActiveSegments = new List<int>(Enumerable.Range(0, _vertexCount ));
        // CalculatePolygonMesh();
    }

    public PolygonSegmentsCreator ShuffleSegments()
    {
        ActiveSegments = new List<int>(Enumerable.Range(0, _vertexCount)).Shuffle();
        CalculatePolygonMesh();

        return this;
    }

    public void SetSegmentCount(int segmentCount)
    {
        if (segmentCount < 0) return;
        if (segmentCount > _vertexCount)
            segmentCount = _vertexCount;

        _segmentCount = segmentCount;
    }
}
