using UnityEngine;

[RequireComponent (typeof(StyledComboBox))]
public class ComboBoxItemList : MonoBehaviour 
{
	public object[] List;

	private void Start()
	{
		GetComponent<StyledComboBox> ().AddItems (List);
		Destroy (this);
	}
}
