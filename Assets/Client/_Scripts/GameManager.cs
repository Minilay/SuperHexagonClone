using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Header("Obstacle Management")]
    [SerializeField] private ObstacleSpawner _obstacleSpawner;
    [SerializeField] private ObstacleManager _obstacleManager;
    
    [Header("Polygon creators")]
    [SerializeField] private PolygonCreator _centralPolygon;
    [SerializeField] private PolygonCreator _centralPolygonBorder;
    [SerializeField] private PolygonSegmentsCreator _backgroundPrimary;
    [SerializeField] private  PolygonSegmentsCreator _backgroundSecondary;

    [Header("Timings")]
    [SerializeField] private float _vertexChangeInterval;
    [SerializeField] private float _enclosedTime;
    [SerializeField] private float _slowMotionTime;
    
    [Range(3, 12)]
    [SerializeField] private int _minHexagonAmount;
    [Range(3, 12)]
    [SerializeField] private int _maxHexagonAmount;
    
    [field: SerializeField] private int VertexCount { get; set; }
    private List<PolygonCreator> _allPolygons;
    private void Awake()
    {
        _centralPolygon.SetVertexCount(VertexCount);
        _centralPolygonBorder.SetVertexCount(VertexCount);

        _allPolygons = new List<PolygonCreator> {_centralPolygon, _centralPolygonBorder, _backgroundPrimary, _backgroundSecondary };
        CreateAllPolygons();
        
        _obstacleSpawner.SetObstacleVertexCount(VertexCount);

        StartCoroutine(PolygonChanger());

    }

    private void OnValidate()
    {
        if (_minHexagonAmount >= _maxHexagonAmount)
            _maxHexagonAmount++;
    }

    private void CreateAllPolygons()
    {
        _allPolygons.ForEach( x=> x.Create());
    }

    private void SetBackground()
    {
        var segmentsList = Enumerable.Range(0, VertexCount).ToList();
        
        _backgroundPrimary.SetSegmentCount(VertexCount / 2);
        _backgroundPrimary.ActiveSegments =
            segmentsList.Where(x => x % 2 == 0).ToList();
        
        _backgroundSecondary.SetSegmentCount(VertexCount / 2);
        _backgroundSecondary.ActiveSegments = segmentsList.Where(x => x % 2 == 1).ToList();
    }

    private IEnumerator PolygonChanger()
    {
        while (true)
        {
            yield return new WaitForSeconds(_vertexChangeInterval);
            if (VertexCount == _minHexagonAmount)
            {
                StartCoroutine(IncreasePolygon());
            }
            else if (VertexCount == _maxHexagonAmount)
            {
                StartCoroutine(DecreasePolygon());
            }
            else if (Random.Range(0, 100) > 50)
            {
                StartCoroutine(IncreasePolygon());
            }
            else
            {
                StartCoroutine(DecreasePolygon());
            }
        }
    }
    //TODO: Refactor code, so that there won't be 5 calls from _obstacleSpawner
    private IEnumerator IncreasePolygon()
    { 
        VertexCount++;
        StopCoroutine(_obstacleSpawner.RegularCoroutine);
        StartCoroutine(_obstacleSpawner.PreIncrease());

        yield return new WaitForSeconds(_enclosedTime);
   
        _obstacleSpawner.Obstacles.ForEach(x => x.IncreaseVertex());
        
        _allPolygons.ForEach(x => x.IncreaseVertex());
        SetBackground();
        
        _obstacleSpawner.RegularCoroutine = StartCoroutine(_obstacleSpawner.SpawnObstacles());
        
             
        var shrinkSpeed = _obstacleManager.ShrinkSpeed;
        _obstacleManager.ShrinkSpeed /= 4;
        yield return new WaitForSeconds(_slowMotionTime);
        _obstacleManager.ShrinkSpeed = shrinkSpeed;

    }

    private IEnumerator DecreasePolygon()
    {
        VertexCount--;
        StopCoroutine(_obstacleSpawner.RegularCoroutine);
        StartCoroutine(_obstacleSpawner.PreDecrease());

        yield return new WaitForSeconds(_enclosedTime);
   
        _obstacleSpawner.Obstacles.ForEach(x => x.DecreaseVertex());
        
        _allPolygons.ForEach(x => x.DecreaseVertex());
        SetBackground();
        
        _obstacleSpawner.RegularCoroutine = StartCoroutine(_obstacleSpawner.SpawnObstacles());
        
        var shrinkSpeed = _obstacleManager.ShrinkSpeed;
        _obstacleManager.ShrinkSpeed /= 4;
        yield return new WaitForSeconds(_slowMotionTime);
        _obstacleManager.ShrinkSpeed = shrinkSpeed;
    }
}
