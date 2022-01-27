using System;
using NUnit.Framework.Constraints;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HollowPolygonCreator))]
public class HollowPolygonCreatorEditor: Editor
{
    private HollowPolygonCreator _polygonCreator;
    private void OnEnable()
    {
        _polygonCreator = (HollowPolygonCreator) target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUIButton.Button(() =>
        {
            _polygonCreator.Create();
        }, "Create Hollow Polygon");

    }
}