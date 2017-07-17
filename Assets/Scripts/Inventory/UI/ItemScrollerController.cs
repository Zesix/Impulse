using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;

public class ItemScrollerController : MonoBehaviour
{
    // Config location
    public const string JsonItemExtractorPath = "Assets/Prefabs/Inventory/JSON Item Extractor.prefab";

    // UI display
    private List<ItemData> _data;
    public ScrollRect MyScroller;
    public ItemCellView ItemCellViewPrefab;

    public float CellSize = 50.0f;

    private void Start()
    {
        _data = new List<ItemData>();

        // Get item data extractor
        var itemExtractor = AssetDatabase.LoadAssetAtPath<JSONItemExtractor>(JsonItemExtractorPath);
        if (itemExtractor != null)
        {
            // Get items' prefabs
            var itemPrefabs = Resources.LoadAll(itemExtractor.OutputFolder);
            // Set item datas
            foreach (var itemObject in itemPrefabs)
            {
                var item = itemObject as ItemAssetData;
                if (item != null)
                    _data.Add(new ItemData(item.Name, item.Range, item.Attack, item.Defense, item.Durability, item.Cost));
            }
        }
        else
        {
            Debug.LogError("A JsonItemExtractor prefab wasn't found in " + JsonItemExtractorPath + ". Please fix this by adding this prefab there or changing the path in JsonItemExtractorPath in the ItemScrollerController class");
        }

        RefreshScroll();
    }

    private void RefreshScroll()
    {
        // Erase previous childrens
        for (var i = 0; i < MyScroller.content.childCount; i++)
            Destroy(MyScroller.content.GetChild(i).gameObject);

        // Update scroll list
        foreach (var item in _data)
        {
            var newItemCell = Instantiate(ItemCellViewPrefab);
            newItemCell.transform.SetParent(MyScroller.content);
            newItemCell.transform.localPosition = Vector3.zero;

            newItemCell.SetData(item);
        }
    }

    public int CurrentNumberElements()
    {
        return _data.Count;
    }
}
