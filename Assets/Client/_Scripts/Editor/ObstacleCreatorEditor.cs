using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObstacleCreator))]
public class ObstacleCreatorEditor : Editor
{
    private ObstacleCreator _obstacleCreator;

    private void OnEnable()
    {
        _obstacleCreator = (ObstacleCreator) target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUIButton.Button(() =>
        {
            _obstacleCreator.CreateSegmented();
        }, "Create Segmented Polygon");
        
        GUIButton.Button(() =>
        {
            _obstacleCreator.ShuffleSegments();
        }, "Reshuffle Segments");
    }
}
