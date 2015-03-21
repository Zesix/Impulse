using UnityEngine;
using System.Collections;

public class TestStyledComboBox : MonoBehaviour 
{
	public StyledComboBox comboBox;

	void Start () 
	{   
		// just text
		comboBox.AddItems("Test1", "Test2", "Test3", "Unity", "Needs", "A", "Better", "Encapsulation", "System", "Than", "Prefabs");

	}
}
