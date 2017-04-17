using UnityEngine;
using UnityEditor;

class MyAllPostprocessor : AssetPostprocessor
{
    // Config location
    public const string CsvItemExtractorPath = "Assets/Resources/InventoryDemo/Prefabs/CSV Item Extractor.prefab";
    public const string JsonItemExtractorPath = "Assets/Resources/InventoryDemo/Prefabs/JSON Item Extractor.prefab";

    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (var str in importedAssets)
        {
            // Check if the file was an .CSV item
            if (str.Contains(".csv"))
            {
                CSVItemExtractor itemExtractor = AssetDatabase.LoadAssetAtPath<CSVItemExtractor>(CsvItemExtractorPath);
                if (itemExtractor != null)
                {
                    // Update Items' Data
                    Debug.Log("CSV File Found, updating prefabs");
                    itemExtractor.ExtractItemsFromCSVFile();
                }
                else
                {
                    Debug.LogError("A CSVItemExtractor prefab wasn't found in " + CsvItemExtractorPath +
                                   ". Please fix this by adding this prefab there or changing the path in CsvItemExtractorPath in the MyAllPostprocessor class");
                }
            }
            // Otherwise check if it was a json file
            else if (str.Contains(".json"))
            {
                JSONItemExtractor itemExtractor = AssetDatabase.LoadAssetAtPath<JSONItemExtractor>(JsonItemExtractorPath);
                if (itemExtractor != null)
                {
                    // Update Items' Data
                    Debug.Log("JSON File Found, updating prefabs");
                    itemExtractor.ExtractItemsFromJsonFile();
                }
                else
                {
                    Debug.LogError("A JSONItemExtractor prefab wasn't found in " + JsonItemExtractorPath +
                                   ". Please fix this by adding this prefab there or changing the path in JsonItemExtractorPath in the MyAllPostprocessor class");
                }
            }
        }

        /* Delete asset case
        foreach (var str in deletedAssets)
        {
            // Check if the file was an .CSV item
            if (str.Contains(".csv"))
            {
                CSVItemExtractor itemExtractor = AssetDatabase.LoadAssetAtPath<CSVItemExtractor>(CsvItemExtractorPath);
                if (itemExtractor != null)
                {
                    // Update Items' Data
                    itemExtractor.ExtractItemsFromCSVFile();

                    // Update item data
                    Debug.Log("Item data updated");
                }
                else
                {
                    Debug.LogError("A CSVItemExtractor prefab wasn't found in " + CsvItemExtractorPath +
                                   ". Please fix this by adding this prefab there or changing the path in CsvItemExtractorPath in the MyAllPostprocessor class");
                }
            }
        }
        */
    }
}