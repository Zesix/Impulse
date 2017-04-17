using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class JSONReader : MonoBehaviour
{

    public static List<Dictionary<string, object>> Read(string file)
    {
        var list = new List<Dictionary<string, object>>();
        TextAsset data = Resources.Load(file) as TextAsset;

        if (data == null) return null;
        JSONNode parsedJason = SimpleJSON.JSON.Parse(data.text);
        foreach (JSONNode element in parsedJason.Children)
        {
            Dictionary<string, object> entry = new Dictionary<string, object>();
            entry.Add("NAME",(string)element["NAME"]);
            entry.Add("RANGE", (int)element["RANGE"]);
            entry.Add("ATTACK", (int)element["ATTACK"]);
            entry.Add("DEFENSE", (int)element["DEFENSE"]);
            entry.Add("DURABILITY", (int)element["DURABILITY"]);
            entry.Add("COST", (int)element["COST"]);
            list.Add(entry);
        }

        return list;
    }
}


