using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionDetector : MonoBehaviour
{
    [SerializeField] private ObstacleManager _obstacleManager;
    [SerializeField] private float _collisionDistance;
    [SerializeField] private float _collisionWidths;
    
    private int _currentSegment;
    private float _currentAngle;
       
    private PolygonSegmentsCreator _closestObstacle;
    private void FixedUpdate()
    {
        _closestObstacle = _obstacleManager.ClosestObstacle;
        
        if(!_closestObstacle) return;
        
        var closestObstacleDistance = _closestObstacle.Radius;
        //TODO: Make it update only when vertex count changes; 
        var angleStep = 360.0f / _closestObstacle.GetVertexCount;

        
        _currentAngle = (transform.eulerAngles.y + 270 - angleStep) % 360;
        _currentSegment = (int) (_currentAngle / angleStep);

        if (closestObstacleDistance <= _collisionDistance && 
            (_collisionDistance - closestObstacleDistance < _collisionWidths)&& 
            _closestObstacle.ActiveSegments.Contains(_currentSegment))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
}

   
    
}
