using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Client._Scripts.Polygon;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] private float shrinkSpeed;
    [SerializeField] private float _spawnInterval;
    
    [SerializeField] private ObstacleCreator _obstaclePrefab;

    [SerializeField]private List<ObstacleCreator> _obstacles;

    //TODO: Should it be here? I mean, Obstacle manager has no practical reason of having this field inside (its only useful for CollisionDetector class), but it makes coding easier, so I am leaving it for a while
    [NonSerialized]public ObstacleCreator _closestObstacle;

    private void Start()
    {
        _obstacles = new List<ObstacleCreator>();
        StartCoroutine(SpawnObstacles());
    }

    private void FixedUpdate()
    {
        for(var i = 0; i < _obstacles.Count; i++)
        {
            var obstacle = _obstacles[i];
            if (obstacle._radius <= 2)
            {
                _obstacles.Remove(obstacle);
                Destroy(obstacle.gameObject);
                _closestObstacle = _obstacles.First();
            }
            obstacle._radius -= shrinkSpeed * Time.deltaTime;
            obstacle.CreateSegmented();
        }
    }
    
    private IEnumerator SpawnObstacles()
    {
        while (true)
        {
            var obstacle = Instantiate(_obstaclePrefab, transform).ShuffleSegments();
            _obstacles.Add(obstacle);
            //TODO: Is There a faster way to initialize _closestObstacle only for the first time? 
            _closestObstacle ??= obstacle;
            yield return new WaitForSeconds(_spawnInterval);
        }
    }
}
