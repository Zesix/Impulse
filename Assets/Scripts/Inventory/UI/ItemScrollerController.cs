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

    void Start()
    {
        this._data = new List<ItemData>();

        // Get item data extractor
        JSONItemExtractor itemExtractor = AssetDatabase.LoadAssetAtPath<JSONItemExtractor>(JsonItemExtractorPath);
        if (itemExtractor != null)
        {
            // Get items' prefabs
            var itemPrefabs = Resources.LoadAll(itemExtractor.OutputFolder);
            // Set item datas
            foreach (var itemObject in itemPrefabs)
            {
                var item = itemObject as ItemAssetData;
                if (item != null)
                    this._data.Add(new ItemData(item.Name, item.Range, item.Attack, item.Defense, item.Durability, item.Cost));
            }
        }
        else
        {
            Debug.LogError("A JsonItemExtractor prefab wasn't found in " + JsonItemExtractorPath + ". Please fix this by adding this prefab there or changing the path in JsonItemExtractorPath in the ItemScrollerController class");
        }

        this.RefreshScroll();
    }

    private void RefreshScroll()
    {
        // Erase previous childrens
        for (int i = 0; i < this.MyScroller.content.childCount; i++)
            Destroy(this.MyScroller.content.GetChild(i).gameObject);

        // Update scroll list
        foreach (var item in this._data)
        {
            var newItemCell = Instantiate(ItemCellViewPrefab) as ItemCellView;
            newItemCell.transform.SetParent(this.MyScroller.content);
            newItemCell.transform.localPosition = Vector3.zero;

            newItemCell.SetData(item);
        }
    }

    public int CurrentNumberElements()
    {
        return this._data.Count;
    }
}
