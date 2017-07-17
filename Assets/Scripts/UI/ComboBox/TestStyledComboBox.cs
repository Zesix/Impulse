using UnityEngine;

public class TestStyledComboBox : MonoBehaviour 
{
	public StyledComboBox ComboBox;

	private void Start () 
	{   
		ComboBox.AddItems("Test1", "Test2", "Test3", "Unity", "Needs", "A", "Better", "Encapsulation", "System", "Than", "Prefabs");
	}
}
