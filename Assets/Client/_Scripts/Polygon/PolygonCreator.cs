using System;
using System.Linq;
using UnityEngine;

namespace Client._Scripts.Polygon
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class PolygonCreator : MonoBehaviour
    {
        [SerializeField] protected int _vertexCount;
        
        //TODO: Is there way to prevent public, if I need both get and set value from outer class? 
        [SerializeField] public float _radius;
        [Range(0, 120)]
        [SerializeField] protected float _phase;

        //TODO: Create HoledPolygons CLass and inherit this class
        [Header("For polygons with a hole")]
        [SerializeField] protected float _width;

        protected MeshFilter _meshFilter;

        protected void Awake()
        {
            _meshFilter = GetComponent<MeshFilter>();
        }

        private void OnValidate()
        {
            Validation();
        }
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
            
            var angleStep = 360.0f / _vertexCount;
        
            for (var i = 0; i < _vertexCount; i++)
            {
                var angle = i * angleStep + _phase;
                
                verts[_vertexCount - i - 1] = PolarToCartesian(angle, _radius);
                uv[_vertexCount - i - 1] = PolarToCartesian(angle);
                normals[i] = Vector3.back;
            
                verts[_vertexCount * 2 - i - 1] = PolarToCartesian(angle, _radius - _width);
                uv[_vertexCount * 2- i - 1] = PolarToCartesian(angle, _width / _radius);
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

        protected void Validation()
        {
            if (_vertexCount < 3)
                _vertexCount = 3;

            if (_radius <= 0)
                _radius = 0.001f;
            
            if (_width >= _radius)
                _width = _radius;

            if (_width < 0)
                _width = 0;
        }

        protected Vector3 PolarToCartesian(float angle, float radius = 1) =>
            new Vector3(
                Mathf.Cos(angle * Mathf.Deg2Rad) * radius,
                Mathf.Sin(angle * Mathf.Deg2Rad) * radius
                );
        
        protected Mesh MakeMesh(Vector3[] verts, Vector2[] uv, int[] tris, Vector3[] normals) 
            => new Mesh {vertices = verts, triangles = tris, normals = normals, uv = uv};

        public Mesh GetMesh() => _meshFilter.mesh;
    }
}
