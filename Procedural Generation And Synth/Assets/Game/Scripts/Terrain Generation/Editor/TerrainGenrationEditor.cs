using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TerrainGeneration.EditorScripts
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(TerrainGeneration))]
    public class TerrainGenrationEditor : Editor
    {
        private TerrainGeneration _target;

        private void OnEnable()
        {
            _target = (TerrainGeneration)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Generate")) { _target.GenerateTerrain(); }
            if (GUILayout.Button("Apply Texture")) { _target.ApplyTexture(); }
        }
    }
}