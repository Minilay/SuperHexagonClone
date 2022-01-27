using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PolygonSegmentsCreator))]
public class PolygonSegmentsCreatorEditor : Editor
{
    private PolygonSegmentsCreator _obstacleMeshCreator;

    private void OnEnable()
    {
        _obstacleMeshCreator = (PolygonSegmentsCreator) target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUIButton.Button(() =>
        {
            _obstacleMeshCreator.Create();
        }, "Create Segmented Polygon");
        
        GUIButton.Button(() =>
        {
            _obstacleMeshCreator.ShuffleSegments();
        }, "Reshuffle Segments");
    }
}
