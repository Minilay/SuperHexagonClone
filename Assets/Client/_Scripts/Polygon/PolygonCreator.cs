using System;
using System.Collections;
using System.Linq;
using UnityEngine;

    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class PolygonCreator : MonoBehaviour
    {
        [SerializeField] protected int _vertexCount;
        [field: SerializeField]public float Radius { get; set; } //Code Conjunction
        [Range(0, 120)]
        [SerializeField] protected float _phase;
        [SerializeField] protected MeshFilter _meshFilter;
        [SerializeField] protected float _angleStep;


        private float _transitionTime = .2f;
        protected virtual void Awake()
        {
            _meshFilter = GetComponent<MeshFilter>();
        }
        
        protected virtual void OnValidate()
        {
            if (_vertexCount < 3)
                _vertexCount = 3;

            if (Radius <= 0)
                Radius = 0.001f;
        }

        public void Create()
        {
            if(_angleStep == 0)
                _angleStep = 360.0f / _vertexCount;
            CalculatePolygonMesh();
        }

        protected virtual void CalculatePolygonMesh()
        {
            var verts = new Vector3[_vertexCount];
            var uv = new Vector2[_vertexCount];
            var tris = new int[3 * (_vertexCount - 2)];
            var normals = new Vector3[_vertexCount];

            for (var i = 0; i < _vertexCount; i++)
            {
                var x = Mathf.Cos((i * _angleStep + _phase) * Mathf.Deg2Rad);
                var y = Mathf.Sin((i * _angleStep + _phase) * Mathf.Deg2Rad);

                verts[_vertexCount - i - 1] = new Vector3(x, y) * Radius;
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

        protected Mesh MakeMesh(Vector3[] verts, Vector2[] uv, int[] tris, Vector3[] normals) 
            => new Mesh {vertices = verts, triangles = tris, normals = normals, uv = uv};

        public int GetVertexCount => _vertexCount;

        public void SetVertexCount(int vertexCount)
        {
            if (vertexCount < 3)
                return;
            _vertexCount = vertexCount;
        }
        public void DecreaseVertex() 
        {
            if(_vertexCount == 3)
                return;
            StartCoroutine(SmoothVertexDecrease(_transitionTime));
        }

        public void IncreaseVertex()
        {
            _vertexCount++;
            StartCoroutine(SmoothVertexIncrease(_transitionTime));
        }

        protected virtual IEnumerator SmoothVertexDecrease(float _smoothTransitionTime)
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
            // CalculatePolygonMesh();
        }
        
        private IEnumerator SmoothVertexIncrease(float _smoothTransitionTime)
        {
            var startAngle = _angleStep;
            var newAngleStep = 360.0f / _vertexCount  ;
            
            var currentTime = .0f;
            while (currentTime <= _smoothTransitionTime)
            {
                currentTime += Time.deltaTime;
                _angleStep = Mathf.LerpAngle(startAngle, newAngleStep, currentTime / _smoothTransitionTime);
                CalculatePolygonMesh();
                yield return null;
            }

        }
    }

