
using System;
using Client._Scripts.Polygon;
using NUnit.Framework.Constraints;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PolygonCreator))]
public class PolygonCreatorEditor : Editor
{
    private PolygonCreator _polygonCreator;
    private void OnEnable()
    {
        _polygonCreator = (PolygonCreator) target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUIButton.Button(() =>
        {
            _polygonCreator.PreCreate();
            _polygonCreator.CreateSolid();
        }, "Create Solid Polygon");
        
        GUIButton.Button(() =>
        {
            _polygonCreator.PreCreate();
            _polygonCreator.CreateWithHole();
        }, "Create Polygon with a hole");
        
        GUIButton.Button(() =>
        {
            _polygonCreator.PreCreate();
            _polygonCreator.CreateSegmented();
        }, "Create segmented Polygon");
    }
}
