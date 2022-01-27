using UnityEngine;

[RequireComponent(typeof(ObstacleSpawner))]
public class ObstacleManager : MonoBehaviour
{
    [field: SerializeField] public float ShrinkSpeed { get; set; }
    private float _despawnDistance = 1;
    public PolygonSegmentsCreator ClosestObstacle { get; private set; }
    private ObstacleSpawner _obstacleSpawner;

    private void Awake()
    {
        _obstacleSpawner = GetComponent<ObstacleSpawner>();
    }

    private void FixedUpdate()
    {
        UpdateClosestObstacle();
        ShrinkObstacles();
    }

    private void UpdateClosestObstacle()
    {
        ClosestObstacle = _obstacleSpawner.GetFirstObstacle();
        if (!ClosestObstacle)
            return;
        
        if (!(ClosestObstacle.Radius <= _despawnDistance)) return;
        
        _obstacleSpawner.RemoveFirstObstacle();
        Destroy(ClosestObstacle.gameObject);
    }
    private void ShrinkObstacles()
    {
        var _obstacles = _obstacleSpawner.Obstacles;
        foreach (var obstacle in _obstacles)
        {
            obstacle.Radius -= ShrinkSpeed * Time.deltaTime;
            obstacle.Create();
        }
    }
}

