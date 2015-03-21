using UnityEngine;
using System.Collections;
[RequireComponent (typeof(StyledComboBox))]
public class ComboBoxItemList : MonoBehaviour 
{
	public string[] List;

	void Start()
	{
		GetComponent<StyledComboBox> ().AddItems (List);
		Destroy (this);
	}
}
