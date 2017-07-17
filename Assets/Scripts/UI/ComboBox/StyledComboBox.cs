using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class StyledComboBox : StyledItem 
{
	public delegate void SelectionChangedHandler(StyledItem item);
	public SelectionChangedHandler OnSelectionChanged;

	public StyledComboBoxPrefab 	ContainerPrefab;		// prefab for whole control
	public StyledItem 				ItemPrefab;				// prefab for item in drop down
	public StyledItem 				ItemMenuPrefab;		// prefab for item in menu

	public float ScrollSensitivity = 1f;

	[SerializeField]
	[HideInInspector]
	private StyledComboBoxPrefab 	_root;
	
	[SerializeField]
	[HideInInspector]
	private List<StyledItem> _items = new List<StyledItem>();

	[SerializeField]
	private int _selectedIndex;
	public int SelectedIndex
	{
		get 
		{
			return _selectedIndex;
		}
		set
		{
			if (value >= 0 && value <= _items.Count)
			{
				_selectedIndex = value;
				CreateMenuButton(_items[_selectedIndex]);
			}

		}
	}


	public StyledItem SelectedItem
	{
		get
		{
			if (_selectedIndex >= 0 && _selectedIndex <= _items.Count)
				return _items[_selectedIndex];
			return null;
		}
	}


	private void Awake()
	{
		InitControl();
	}
	

	private void AddItem(object data)
	{
		if (ItemPrefab != null)
		{
			var corners = new Vector3[4];
			ItemPrefab.GetComponent<RectTransform>().GetLocalCorners(corners);
			var pos = corners[0];
			var sizeY = pos.y - corners[2].y;
			pos.y = _items.Count * sizeY - 5f;
			var styledItem = Instantiate(ItemPrefab, pos, _root.ItemRoot.rotation);
			var trans = styledItem.GetComponent<RectTransform>();
			styledItem.Populate(data);
			trans.SetParent (_root.ItemRoot.transform, false);

			trans.pivot = new Vector2(0,1);
			trans.anchorMin = new Vector2(0,1);
			trans.anchorMax = Vector2.one;
			trans.anchoredPosition = new Vector2(0.0f, pos.y);
			_items.Add(styledItem);

			trans.offsetMin = new Vector2(0, pos.y + sizeY);
			trans.offsetMax = new Vector2(0, pos.y);
			var offsetSize = (_items.Count + 1) * sizeY;
			if (-offsetSize > _root.GetComponent<RectTransform> ().rect.height) 
			{
				_scrollControl.vertical = true;
				_scrollControl.verticalScrollbar.gameObject.SetActive (true);
			}
			_root.ItemRoot.offsetMin = new Vector2(_root.ItemRoot.offsetMin.x, offsetSize);

			var b = styledItem.GetButton();
			var curIndex = _items.Count - 1;
			if (b != null)
			{
				b.onClick.AddListener(delegate { OnItemClicked(styledItem, curIndex); });
			}
		}
	}

	public void OnItemClicked(StyledItem item, int index)
	{
		SelectedIndex = index;

		TogglePanelState();	// close
		OnSelectionChanged?.Invoke(item);
	}

	public void ClearItems()
	{
		for (var i = _items.Count - 1; i >= 0; --i)
			DestroyObject(_items[i].gameObject);
	}

	public void AddItems(params object[] list)
	{
		ClearItems();

		foreach (var t in list)
		{
			AddItem(t);
		}
		SelectedIndex = 0;
	}

	private ScrollRect _scrollControl;


	public void InitControl()
	{
		if (_root != null)
			DestroyImmediate(_root.gameObject);

		if (ContainerPrefab != null)
		{
			// create 
			var own = GetComponent<RectTransform>();
			_root = Instantiate(ContainerPrefab, own.position, own.rotation);
			_root.transform.SetParent(transform, false);
			_scrollControl = _root.ItemPanel.GetComponent<ScrollRect> ();
			_scrollControl.scrollSensitivity = ScrollSensitivity;
			var rt = _root.GetComponent<RectTransform>();
			rt.pivot = new Vector2(0.5f, 0.5f);
			//root.anchoredPosition = Vector2.zero;
			rt.anchorMin = Vector2.zero;
			rt.anchorMax = Vector2.one;
			rt.offsetMax = Vector2.zero;
			rt.offsetMin = Vector2.zero;
			_root.gameObject.hideFlags = HideFlags.HideInHierarchy; // should really be HideAndDontSave, but unity crashes
			_root.ItemPanel.alpha = 0.0f;

			// create menu item
			var toCreate = ItemMenuPrefab;
			if (toCreate == null)
				toCreate = ItemPrefab;
			CreateMenuButton(toCreate);
		}
	}

	private void CreateMenuButton(StyledItem toCreate)
	{
		if (_root.MenuItem.transform.childCount > 0)
		{
			for (var i = _root.MenuItem.transform.childCount - 1; i >= 0; --i)
				DestroyObject(_root.MenuItem.transform.GetChild(i).gameObject);
		}
		if (toCreate != null && _root.MenuItem != null)
		{
			var menuItem = Instantiate(toCreate);
			menuItem.transform.SetParent(_root.MenuItem.transform,false);
			var mt = menuItem.GetComponent<RectTransform>();
			mt.pivot = new Vector2(0.5f, 0.5f);
			mt.anchorMin = Vector2.zero;
			mt.anchorMax = Vector2.one;
			mt.offsetMin = Vector2.zero;
			mt.offsetMax = Vector2.zero;
			_root.gameObject.hideFlags = HideFlags.HideInHierarchy; // should really be HideAndDontSave, but unity crashes
			var b = menuItem.GetButton();
			if (b != null)
			{
				b.onClick.AddListener(TogglePanelState);
			}
		}
	}
	
	public void TogglePanelState()
	{
		_root.ItemPanel.alpha = Mathf.Abs(_root.ItemPanel.alpha - 1.0f);
	}





}


