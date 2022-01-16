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
        [Range(0, 100)]
        [SerializeField] private float _innerRadiusFraction;

        private MeshFilter _meshFilter;

        private void OnValidate()
        {
            if (_vertexCount < 3)
                _vertexCount = 3;
        }

        public void PreCreate()
        {
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            _meshFilter = GetComponent<MeshFilter>();
        }
        public void CreateSolid()
        {
            var mesh = new Mesh();

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
        
            mesh.vertices = verts;
            mesh.triangles = tris;
            mesh.normals = normals;
            mesh.uv = uv;

            _meshFilter.mesh = mesh;        
        }

        public void CreateWithHole()
        {
            var mesh = new Mesh();
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
            
                verts[_vertexCount * 2 - i - 1] = new Vector3(x, y) * (_radius * _innerRadiusFraction / 100f);
                uv[_vertexCount * 2- i - 1] = new Vector3(x, y) * (_innerRadiusFraction / 100);
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

            foreach (var vert in tris)                  
            {
                Debug.Log(vert);
            }
            mesh.vertices = verts;
            mesh.triangles = tris;
            mesh.normals = normals;
            mesh.uv = uv;

            _meshFilter.mesh = mesh;
        }

        public void CreateSegmented()
        {
            
        }
    }
}