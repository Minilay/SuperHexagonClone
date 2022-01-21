using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Client._Scripts.Polygon;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] private float shrinkSpeed;
    [SerializeField] private float _spawnInterval;
    
    [SerializeField] private ObstacleCreator _obstaclePrefab;

    [SerializeField]private List<ObstacleCreator> _obstacles;
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
            if (obstacle._radius <= 0)
            {
                _obstacles.Remove(obstacle);
                Destroy(obstacle.gameObject);
            }
            obstacle._radius -= shrinkSpeed * Time.deltaTime;
            obstacle.CreateSegmented();
        }
    }
    
    private IEnumerator SpawnObstacles()
    {
        while (true)
        {
            _obstacles.Add(Instantiate(_obstaclePrefab, transform).ShuffleSegments());
            yield return new WaitForSeconds(_spawnInterval);
        }
    }
}
