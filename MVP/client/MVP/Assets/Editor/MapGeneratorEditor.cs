using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        MapGenerator mapGenerator = (MapGenerator) target;
        if(GUILayout.Button("Build Map"))
            mapGenerator.GenerateMap();
        if(GUILayout.Button("Destroy Map"))
            mapGenerator.DeleteMap();
    }
}
