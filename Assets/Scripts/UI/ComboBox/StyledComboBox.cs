using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;



[RequireComponent(typeof(RectTransform))]
public class StyledComboBox : StyledItem 
{
	public delegate void SelectionChangedHandler(StyledItem item);
	public SelectionChangedHandler OnSelectionChanged;

	public StyledComboBoxPrefab 	containerPrefab;		// prefab for whole control
	public StyledItem 				itemPrefab;				// prefab for item in drop down
	public StyledItem 				itemMenuPrefab;		// prefab for item in menu

	public float ScrollSensitivity = 1f;

	[SerializeField]
	[HideInInspector]
	private StyledComboBoxPrefab 	root;
	
	[SerializeField]
	[HideInInspector]
	private List<StyledItem> items = new List<StyledItem>();

	[SerializeField]
	private int selectedIndex = 0;
	public int SelectedIndex
	{
		get 
		{
			return selectedIndex;
		}
		set
		{
			if (value >= 0 && value <= items.Count)
			{
				selectedIndex = value;
				CreateMenuButton(items[selectedIndex]);
			}

		}
	}


	public StyledItem SelectedItem
	{
		get
		{
			if (selectedIndex >= 0 && selectedIndex <= items.Count)
				return items[selectedIndex];
			return null;
		}
	}


	void Awake()
	{
		InitControl();
	}
	

	private void AddItem(object data)
	{
		if (itemPrefab != null)
		{
			Vector3[] corners = new Vector3[4];
			itemPrefab.GetComponent<RectTransform>().GetLocalCorners(corners);
			Vector3 pos = corners[0];
			float sizeY = pos.y - corners[2].y;
			pos.y = items.Count * sizeY - 5f;
			StyledItem styledItem = Instantiate(itemPrefab, pos, root.itemRoot.rotation) as StyledItem;
			RectTransform trans = styledItem.GetComponent<RectTransform>();
			styledItem.Populate(data);
			trans.SetParent (root.itemRoot.transform, false);

			trans.pivot = new Vector2(0,1);
			trans.anchorMin = new Vector2(0,1);
			trans.anchorMax = Vector2.one;
			trans.anchoredPosition = new Vector2(0.0f, pos.y);
			items.Add(styledItem);

			trans.offsetMin = new Vector2(0, pos.y + sizeY);
			trans.offsetMax = new Vector2(0, pos.y);
			float offsetSize = (items.Count + 1) * sizeY;
			if (-offsetSize > root.GetComponent<RectTransform> ().rect.height) 
			{
				scrollControl.vertical = true;
				scrollControl.verticalScrollbar.gameObject.SetActive (true);
			}
			root.itemRoot.offsetMin = new Vector2(root.itemRoot.offsetMin.x, offsetSize);

			Button b = styledItem.GetButton();
			int curIndex = items.Count - 1;
			if (b != null)
			{
				b.onClick.AddListener(delegate() { OnItemClicked(styledItem, curIndex); });
			}
		}
	}

	public void OnItemClicked(StyledItem item, int index)
	{
		SelectedIndex = index;

		TogglePanelState();	// close
		if (OnSelectionChanged != null)
		{
			OnSelectionChanged(item);
		}
	}

	public void ClearItems()
	{
		for (int i = items.Count - 1; i >= 0; --i)
			DestroyObject(items[i].gameObject);
	}

	public void AddItems(params object[] list)
	{
		ClearItems();

		for (int i = 0; i < list.Length; ++i)
		{
			AddItem(list[i]);
		}
		SelectedIndex = 0;
	}

	ScrollRect scrollControl;


	public void InitControl()
	{
		if (root != null)
			DestroyImmediate(root.gameObject);

		if (containerPrefab != null)
		{
			// create 
			RectTransform own = GetComponent<RectTransform>();
			root = Instantiate(containerPrefab, own.position, own.rotation) as StyledComboBoxPrefab;
			root.transform.SetParent(this.transform, false);
			scrollControl = root.itemPanel.GetComponent<ScrollRect> ();
			scrollControl.scrollSensitivity = ScrollSensitivity;
			RectTransform rt = root.GetComponent<RectTransform>();
			rt.pivot = new Vector2(0.5f, 0.5f);
			//root.anchoredPosition = Vector2.zero;
			rt.anchorMin = Vector2.zero;
			rt.anchorMax = Vector2.one;
			rt.offsetMax = Vector2.zero;
			rt.offsetMin = Vector2.zero;
			root.gameObject.hideFlags = HideFlags.HideInHierarchy; // should really be HideAndDontSave, but unity crashes
			root.itemPanel.alpha = 0.0f;

			// create menu item
			StyledItem toCreate = itemMenuPrefab;
			if (toCreate == null)
				toCreate = itemPrefab;
			CreateMenuButton(toCreate);
		}
	}

	private void CreateMenuButton(StyledItem toCreate)
	{
		if (root.menuItem.transform.childCount > 0)
		{
			for (int i = root.menuItem.transform.childCount - 1; i >= 0; --i)
				DestroyObject(root.menuItem.transform.GetChild(i).gameObject);
		}
		if (toCreate != null && root.menuItem != null)
		{
			StyledItem menuItem = Instantiate(toCreate) as StyledItem;
			menuItem.transform.SetParent(root.menuItem.transform,false);
			RectTransform mt = menuItem.GetComponent<RectTransform>();
			mt.pivot = new Vector2(0.5f, 0.5f);
			mt.anchorMin = Vector2.zero;
			mt.anchorMax = Vector2.one;
			mt.offsetMin = Vector2.zero;
			mt.offsetMax = Vector2.zero;
			root.gameObject.hideFlags = HideFlags.HideInHierarchy; // should really be HideAndDontSave, but unity crashes
			Button b = menuItem.GetButton();
			if (b != null)
			{
				b.onClick.AddListener(TogglePanelState);
			}
		}
	}
	
	public void TogglePanelState()
	{
		root.itemPanel.alpha = Mathf.Abs(root.itemPanel.alpha - 1.0f);
	}





}


