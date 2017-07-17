using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(JSONItemExtractor))]
[CanEditMultipleObjects]

public class JSONItemExtractorEditor : Editor
{
    /// <summary>
    /// GUI display on inspection
    /// </summary>
    public override void OnInspectorGUI()
    {
        // Get component reference
        JSONItemExtractor myTarget = (JSONItemExtractor)target;

        // Display input parameters
        GUILayout.Label("Input", EditorStyles.boldLabel);
        myTarget.FileLocation = EditorGUILayout.TextField("JSON Folder", myTarget.FileLocation);
        myTarget.FileName = EditorGUILayout.TextField("JSON Name", myTarget.FileName);

        // Cosmetic space
        GUILayout.Space(5);

        // Display output
        GUILayout.Label("Output", EditorStyles.boldLabel);
        myTarget.OutputFolder = EditorGUILayout.TextField(new GUIContent("Output Folder", "This folder must exist in Assets/Resources/"), myTarget.OutputFolder);
        if (GUILayout.Button("Extract Items"))
        {
            myTarget.ExtractItemsFromJsonFile();
        }

        if (GUILayout.Button("Clear Items"))
        {
            myTarget.ClearAllData();
        }
        // Save the changes back to the object
        EditorUtility.SetDirty(target);
    }
}
