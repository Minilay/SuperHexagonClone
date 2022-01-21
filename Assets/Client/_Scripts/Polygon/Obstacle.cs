using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObstacleCreator))]
public class Obstacle : MonoBehaviour
{
    private ObstacleCreator _obstacleCreator;

    private void Awake()
    {
        _obstacleCreator = GetComponent<ObstacleCreator>();
    }

    //TODO: Is it legal to render polygon in One update and set mesh to collider in another update?
    private void FixedUpdate()
    {
    }
}
