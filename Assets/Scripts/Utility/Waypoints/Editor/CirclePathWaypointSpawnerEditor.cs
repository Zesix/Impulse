using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CirclePathWaypointSpawner))]
public class CirclePathWaypointSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var mySpawner = (CirclePathWaypointSpawner)target;
        if (GUILayout.Button("Spawn"))
        {
            mySpawner.SpawnPath();
        }
    }
}

