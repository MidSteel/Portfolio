using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    private MapGenerator _target;

    public void OnEnable()
    {
        _target = (MapGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate"))
        {
            _target.GenerateMap();
        }
    }
}
