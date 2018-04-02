using UnityEngine;
using UnityEditor;
using Debug = System.Diagnostics.Debug;

[CustomEditor (typeof(SquareTileMapGenerator))]
public class SquareTileMapGeneratorEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        var mapGenerator = target as SquareTileMapGenerator;
        if (DrawDefaultInspector())
        {
            Debug.Assert(mapGenerator != null, "mapGenerator != null");
            mapGenerator.GenerateMap();
        }

        if (GUILayout.Button("Load Map"))
        {
            Debug.Assert(mapGenerator != null, "mapGenerator != null");
            mapGenerator.LoadMap();
        }

        if (GUILayout.Button("Generate Map"))
        {
            Debug.Assert(mapGenerator != null, "mapGenerator != null");
            mapGenerator.GenerateMap();
        }

        if (GUILayout.Button("Save Map"))
        {
            Debug.Assert(mapGenerator != null, "mapGenerator != null");
            mapGenerator.SaveMap();
        }
    }

}
