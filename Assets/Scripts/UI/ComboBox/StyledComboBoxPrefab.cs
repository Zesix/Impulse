using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StyledComboBoxPrefab : MonoBehaviour 
{
	public RectTransform    menuItem;	// where to put the menu button
	public RectTransform 	itemRoot;	// where to put the items list
	public CanvasGroup 		itemPanel;	// panel to toggle alpha on for open/close
}
