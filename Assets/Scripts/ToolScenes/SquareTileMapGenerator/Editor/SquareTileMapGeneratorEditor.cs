using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof(SquareTileMapGenerator))]
public class SquareTileMapGeneratorEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        SquareTileMapGenerator mapGenerator = target as SquareTileMapGenerator;
        if (DrawDefaultInspector())
        {
            mapGenerator.GenerateMap();

            if (GUILayout.Button("Generate Map"))
            {
                mapGenerator.GenerateMap();
            }
        }
    }

}
