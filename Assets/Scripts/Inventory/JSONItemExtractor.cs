using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class JSONItemExtractor : MonoBehaviour {
    // Resource file location
    public string FileLocation = "Text";
    public string FileName = "Items";

    // Output file location
    [Tooltip("This folder must exist in Assets/Resources/")]
    public string OutputFolder = "Prefabs/Items/";

    public void ExtractItemsFromJsonFile()
    {
        List<Dictionary<string, object>> data = JSONReader.Read(this.FileLocation + "/" + this.FileName);

        // Error check
        if (data == null)
        {
            Debug.LogError(this.FileLocation + "/ Folder wasn't found");
            return;
        }

        // Delete all previouly created assets
        this.ClearAllData();

        // Extract the data per read item
        for (var i = 0; i < data.Count; i++)
        {
            this.CreateAssetsAndPrefabs(data, i);
        }

        // Update item data message
        Debug.Log("Item data updated!");
    }

    private void CreateAssetsAndPrefabs(List<Dictionary<string, object>> data, int i)
    {
        // Create .asset file
        ItemAssetData itemAsset = ScriptableObject.CreateInstance<ItemAssetData>();
        itemAsset.Name = (string)data[i]["NAME"];
        itemAsset.Range = (int)data[i]["RANGE"];
        itemAsset.Attack = (int)data[i]["ATTACK"];
        itemAsset.Defense = (int)data[i]["DEFENSE"];
        itemAsset.Durability = (int)data[i]["DURABILITY"];
        itemAsset.Cost = (int)data[i]["COST"];

        AssetDatabase.CreateAsset(itemAsset, "Assets/Resources/" + this.OutputFolder + itemAsset.Name + ".asset");
        AssetDatabase.SaveAssets();

        // Create .prefab files
        var gameobject = new GameObject();
        gameobject.transform.name = itemAsset.Name;
        ItemPrefabData itemPrefab = gameobject.AddComponent<ItemPrefabData>();
        itemPrefab.Initialize(itemAsset.Name, itemAsset.Range, itemAsset.Attack, itemAsset.Defense,
            itemAsset.Durability, itemAsset.Cost);

#pragma warning disable 618
        Object prefab = EditorUtility.CreateEmptyPrefab("Assets/Resources/" + this.OutputFolder + itemAsset.Name + ".prefab");
        EditorUtility.ReplacePrefab(gameobject, prefab, ReplacePrefabOptions.ConnectToPrefab);
#pragma warning restore 618
        DestroyImmediate(gameobject);
    }

    public void ClearAllData()
    {
        var previousAssets = Resources.LoadAll(this.OutputFolder);
        foreach (var asset in previousAssets)
        {
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(asset));
        }
    }
}
