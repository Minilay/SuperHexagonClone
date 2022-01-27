using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ObstacleSpawner _obstacleSpawner;
    [SerializeField] private ObstacleManager _obstacleManager;
    
    
    [SerializeField] private PolygonCreator _centralPolygon;
    [SerializeField] private PolygonCreator _centralPolygonBorder;
    [SerializeField] private PolygonSegmentsCreator _backgroundPrimary;
    [SerializeField] private  PolygonSegmentsCreator _backgroundSecondary;

    [SerializeField] private float _transitionTime;

    [SerializeField] private float _vertexChangeInterval;

    [SerializeField] private float _enclosedTime;
    [SerializeField] private float _slowMotionTime;
    
    [field: SerializeField] public int VertexCount { get; private set; }

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

        var toDecrease = false;
        while (true)
        {
            yield return new WaitForSeconds(_vertexChangeInterval);
            StartCoroutine(toDecrease ? DecreasePolygon() : IncreasePolygon());

            toDecrease = !toDecrease;

        }
    }
    //TODO: Refactor code, so that there won't be 5 calls from _obstacleSpawner
    private IEnumerator IncreasePolygon()
    { 
        VertexCount++;
        StopCoroutine(_obstacleSpawner.RegularCoroutine);
        StartCoroutine(_obstacleSpawner.SpawnEnclosedObstacle());

        yield return new WaitForSeconds(_enclosedTime);
   
        _obstacleSpawner.Obstacles.ForEach(x => x.IncreaseVertex(_transitionTime));
        
        _allPolygons.ForEach(x => x.IncreaseVertex(_transitionTime));
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
        _allPolygons.ForEach(x => x.DecreaseVertex(_transitionTime));
        SetBackground();
        _obstacleSpawner.DecreasePolygon();
        yield return null;
    }
}
