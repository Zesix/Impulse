using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(StyledComboBox))]
public class StyledComboBoxEditor : Editor 
{
	public override void OnInspectorGUI()
	{
		// we have to slam the control so it fits in it's desired size due to a bug in unity which blows
		// away the initial transform data.
		StyledComboBox comboBox = target as StyledComboBox;
		EditorGUI.BeginChangeCheck();
		DrawDefaultInspector();
		if (EditorGUI.EndChangeCheck())
		{
			comboBox.InitControl();
		}
	}
}

public class StyledComboBoxMenuItem
{
	[MenuItem("GameObject/UI/ComboBox")]
	public static void CreateComboBox()
	{
		GameObject go = new GameObject("ComboBox");
		go.AddComponent<RectTransform>();
		if (Selection.objects.Length > 0)
		{
			GameObject selected = Selection.objects[0] as GameObject;
			if (selected)
				go.transform.parent = selected.transform;
		}
		
		go.AddComponent<StyledComboBox>();
	}
}