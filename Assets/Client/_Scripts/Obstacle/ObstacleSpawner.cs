using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private float _spawnInterval;
    [SerializeField] private PolygonSegmentsCreator obstacleMeshPrefab;
    private int _obstaclesVertexCount;
    public List<PolygonSegmentsCreator> Obstacles { get; private set; }
    public Coroutine RegularCoroutine { get; set; }
    private void Awake()
    {
        Obstacles = new List<PolygonSegmentsCreator>();
        RegularCoroutine = StartCoroutine(SpawnObstacles());
    }
    
    //TODO: Read about async/await
    public IEnumerator SpawnObstacles()
    {
        yield return new WaitForSeconds(_spawnInterval);

        while (true)
        {
            obstacleMeshPrefab.SetVertexCount(_obstaclesVertexCount);
            obstacleMeshPrefab.RandomSegmentCount = true;

            var obstacle = Instantiate(obstacleMeshPrefab, transform).ShuffleSegments();
            Obstacles.Add(obstacle);
            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    public IEnumerator PreIncrease()
    {
        yield return new WaitForSeconds(_spawnInterval);
        
        const int obstacleCount = 3;
        for (var i = 0; i < obstacleCount; i++)
        {
            obstacleMeshPrefab.SetVertexCount(_obstaclesVertexCount);
            obstacleMeshPrefab.SetSegmentCount(_obstaclesVertexCount);
            obstacleMeshPrefab.RandomSegmentCount = false;
            var obstacle = Instantiate(obstacleMeshPrefab, transform).ShuffleSegments();
            Obstacles.Add(obstacle);
            yield return new WaitForSeconds(_spawnInterval / 4);
        }
        _obstaclesVertexCount++;
    }

    public IEnumerator PreDecrease()
    {
        yield return new WaitForSeconds(_spawnInterval);
        obstacleMeshPrefab.RandomSegmentCount = false;
        obstacleMeshPrefab.SetSegmentCount(_obstaclesVertexCount - 2);

        var obstacle = Instantiate(obstacleMeshPrefab, transform);

        var activeSegments = new List<int>(Enumerable.Range(0, _obstaclesVertexCount));
        activeSegments.Remove(0);
        activeSegments.Remove(_obstaclesVertexCount - 1);
        obstacle.ActiveSegments = activeSegments;

        obstacle.Create();
        Obstacles.Add(obstacle);
        
        _obstaclesVertexCount--;

    }
    public PolygonSegmentsCreator GetFirstObstacle() => Obstacles.FirstOrDefault();
    public void RemoveFirstObstacle() => Obstacles.RemoveAt(0);
    

    public void SetObstacleVertexCount(int vertexCount)
    {
        if (vertexCount < 3) return;
        _obstaclesVertexCount = vertexCount;
    }
        
}
    
