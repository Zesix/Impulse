using System.Collections;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif   
using UnityEngine;

/// <summary>
/// Use this class to read and parse json files
/// </summary>
public class JsonService : Singleton<JsonService>, ILocalDataService
{
    private const string _resourcesPath = "/Resources/";

    /// <summary>
    /// Use this to create a new json file
    /// </summary>
    public void CreateLocalDataFile(object content,string filename, string path = "")
    {
        string json = JsonUtility.ToJson(content);
        File.WriteAllText(Application.dataPath + _resourcesPath + path,json);
    }

    /// <summary>
    /// Use this to delete target json
    /// NOTE: Resources items can only be deleted during Editor time, not in a runtime build
    /// </summary>
    public void DeleteLocalDataFile(string pathToDelete)
    {
#if UNITY_EDITOR
        if(AssetDatabase.DeleteAsset(_resourcesPath + pathToDelete))
        {
            Debug.Log(pathToDelete + " deleted succesfully!");
        }
        else
        {
            Debug.LogError(pathToDelete + " couldn't be found!");
        }
#else
        Debug.LogError("Can't delete resources during a runtime build!");
#endif   
    }

    /// <summary>
    /// Use this to deserialize target json and load file at path
    /// </summary>
    public T ReadLocalDataFile<T>(string path)
    {
       return  JsonUtility.FromJson<T>(path);
    }

    /// <summary>
    /// Use this to get raw deseralization target json and load file at path
    /// </summary>
    public TextAsset ReadLocalRawlDataFile(string path)
    {
        TextAsset SourceFile = (TextAsset)Resources.Load(path, typeof(TextAsset));
        return SourceFile;
    }

    /// <summary>
    /// Use this to overwrite an existing json file
    /// </summary>
    public void UpdateLocalDataFile(object content, string path)
    {
        TextAsset targetJson = ReadLocalRawlDataFile(path);

        if(targetJson != null)
        {
            CreateLocalDataFile(content, targetJson.name, path);
        }
        else
        {
            Debug.LogError("Couldn't find json :" + path);
        }

    }
}
