using System.Linq;
using UnityEngine;

namespace Client._Scripts.Polygon
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class PolygonCreator : MonoBehaviour
    {
        [SerializeField] private int _vertexCount;
        [SerializeField] private float _radius;
        [Range(0, 120)]
        [SerializeField] private float _phase;

        [Header("For polygons with a hole")]
        [SerializeField] private float _innerRadius;
        
        [Header("For segmented polygon")]
        [SerializeField] private int _segmentCount;
        [SerializeField] private bool _generateRandomSegments;
        
        private MeshFilter _meshFilter;

        private void OnValidate()
        {
            Validation();
        }


        public void PreCreate()
        =>
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            _meshFilter = GetComponent<MeshFilter>();
        
        
        public void CreateSolid()
        {
            var verts = new Vector3[_vertexCount];
            var uv = new Vector2[_vertexCount];
            var tris = new int[3 * (_vertexCount - 2)];
            var normals = new Vector3[_vertexCount];

            var angleStep = 360.0f / _vertexCount;
            for (var i = 0; i < _vertexCount; i++)
            {
                var x = Mathf.Cos((i * angleStep + _phase) * Mathf.Deg2Rad);
                var y = Mathf.Sin((i * angleStep + _phase) * Mathf.Deg2Rad);

                verts[_vertexCount - i - 1] = new Vector3(x, y) * _radius;
                uv[_vertexCount - i - 1] = new Vector3(x, y);
                normals[i] = Vector3.back;
            }

            for (var i = 0; i < _vertexCount - 2; i++)
            {
                tris[i * 3] = i + 2;
                tris[i * 3 + 1] = 0;
                tris[i * 3 + 2] = i + 1;
            }
                  
            _meshFilter.mesh = MakeMesh(verts, uv, tris, normals);        
        }

        public void CreateWithHole()
        {
            var verts = new Vector3[_vertexCount * 2];
            var uv = new Vector2[_vertexCount * 2];
            var tris = new int[_vertexCount * 6];
            var normals = new Vector3[_vertexCount * 2];
            
            float x, y;
            var angleStep = 360.0f / _vertexCount;
        
            for (var i = 0; i < _vertexCount; i++)
            {
                x = Mathf.Cos((i * angleStep + _phase)  * Mathf.Deg2Rad);
                y = Mathf.Sin((i * angleStep + _phase) * Mathf.Deg2Rad);

                verts[_vertexCount - i - 1] = new Vector3(x, y) * _radius;
                uv[_vertexCount - i - 1] = new Vector3(x, y);
                normals[i] = Vector3.back;
            
                verts[_vertexCount * 2 - i - 1] = new Vector3(x, y) * (_radius - _innerRadius);
                uv[_vertexCount * 2- i - 1] = new Vector3(x, y) * (_innerRadius / _radius);
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

        public void CreateSegmented()
        {
            var verts = new Vector3[_vertexCount * 2];
            var uv = new Vector2[_vertexCount * 2];
            var tris = new int[_segmentCount * 6];
            var normals = new Vector3[_vertexCount * 2];
            
            float x, y;
            var angleStep = 360.0f / _vertexCount;
        
            for (var i = 0; i < _vertexCount; i++)
            {
                x = Mathf.Cos((i * angleStep + _phase)  * Mathf.Deg2Rad);
                y = Mathf.Sin((i * angleStep + _phase) * Mathf.Deg2Rad);

                verts[_vertexCount - i - 1] = new Vector3(x, y) * _radius;
                uv[_vertexCount - i - 1] = new Vector3(x, y);
                normals[i] = Vector3.back;
            
                verts[_vertexCount * 2 - i - 1] = new Vector3(x, y) * (_radius - _innerRadius );
                uv[_vertexCount * 2- i - 1] = new Vector3(x, y) * (_innerRadius / _radius);
                normals[_vertexCount + i] = Vector3.back;
            }

            var randomSegments = new int[_vertexCount];
            for (int i = 0; i < _vertexCount; i++)
            {
                randomSegments[i] = i;
            }

            if (_generateRandomSegments)
            {
                System.Random random = new System.Random();
                randomSegments = randomSegments.OrderBy(_ => random.Next()).ToArray();
            }

            for (int i = 0; i < _segmentCount; i++)
            {
                var segmentNumber = randomSegments[i];
                tris[i * 3] = segmentNumber;
                tris[i * 3 + 1] = (segmentNumber + 1) % _vertexCount;
                tris[i * 3 + 2] = segmentNumber + _vertexCount;

                tris[_segmentCount * 3 + i * 3] = (segmentNumber + 1) % _vertexCount;
                tris[_segmentCount * 3 + i * 3 + 1] = (segmentNumber + 1) % _vertexCount + _vertexCount;
                tris[_segmentCount * 3 + i * 3 + 2] = (segmentNumber + _vertexCount) % _vertexCount + _vertexCount;
            }
            _meshFilter.mesh = MakeMesh(verts, uv, tris, normals);
        }
        
        private void Validation()
        {
            if (_vertexCount < 3)
                _vertexCount = 3;

            if (_radius <= 0)
                _radius = 0.001f;
            
            if (_innerRadius >= _radius)
                _innerRadius = _radius;

            if (_innerRadius < 0)
                _innerRadius = 0;
            
            if (_segmentCount < 1)
                _segmentCount = 1;
            
            if (_segmentCount >= _vertexCount)
                _segmentCount = _vertexCount - 1;
            
        }
        private Mesh MakeMesh(Vector3[] verts, Vector2[] uv, int[] tris, Vector3[] normals) 
            => new Mesh {vertices = verts, triangles = tris, normals = normals, uv = uv};
    }
}
