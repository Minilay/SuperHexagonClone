
using System;
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
            _polygonCreator.Create();
        }, "Create Solid Polygon");

    }
}
