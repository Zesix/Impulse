using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CSVItemExtractor))]
[CanEditMultipleObjects]

public class CSVItemExtractorEditor : Editor
{
    /// <summary>
    /// GUI display on inspection
    /// </summary>
    public override void OnInspectorGUI()
    {
        // Get component reference
        CSVItemExtractor myTarget = (CSVItemExtractor)target;

        // Display input parameters
        GUILayout.Label("Input", EditorStyles.boldLabel);
        myTarget.FileLocation = EditorGUILayout.TextField("CSV Folder", myTarget.FileLocation);
        myTarget.FileName = EditorGUILayout.TextField("CSV Name", myTarget.FileName);

        // Cosmetic space
        GUILayout.Space(5);

        // Display output
        GUILayout.Label("Output", EditorStyles.boldLabel);
        myTarget.OutputFolder = EditorGUILayout.TextField(new GUIContent( "Output Folder", "This folder must exist in Assets/Resources/"), myTarget.OutputFolder);
        if (GUILayout.Button("Extract Items"))
        {
            myTarget.ExtractItemsFromCSVFile();
        }

        if (GUILayout.Button("Clear Items"))
        {
            myTarget.ClearAllData();
        }
        // Save the changes back to the object
        EditorUtility.SetDirty(target);
    }
}
