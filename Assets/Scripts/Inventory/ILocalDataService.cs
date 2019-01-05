using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILocalDataService  {

    void CreateLocalDataFile(object content, string filename, string path = "");

    T ReadLocalDataFile<T>(string path);

    void UpdateLocalDataFile(object content, string path);

    TextAsset ReadLocalRawlDataFile(string path);

    void DeleteLocalDataFile(string path);

}
