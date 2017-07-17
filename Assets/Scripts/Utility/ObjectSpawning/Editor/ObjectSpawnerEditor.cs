using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RandomObjectSpawner))]
public class ObjectSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var mySpawner = (RandomObjectSpawner)target;
        if (GUILayout.Button("Spawn"))
        {
            mySpawner.SpawnObjects();
        }
    }
}

