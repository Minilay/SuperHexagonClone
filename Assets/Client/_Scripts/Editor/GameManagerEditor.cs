using System;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = (GameManager) target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        // GUIButton.Button((() =>
        // {
        //     _gameManager.DecreaseVertex();
        // }), "Decrease Vertex Count");
        // GUIButton.Button((() =>
        // {
        //     _gameManager.IncreaseVertex();
        // }), "Increase Vertex Count");
    }
}
