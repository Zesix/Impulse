using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class JSONReader : MonoBehaviour
{
    public static List<Dictionary<string, object>> Read(string file)
    {
        var list = new List<Dictionary<string, object>>();
        TextAsset data = Resources.Load(file) as TextAsset;

        if (data == null) return null;
        ItemData[] parsedJason = JsonConvert.DeserializeObject<ItemData[]>(data.text);
        foreach (ItemData info in parsedJason)
        {
            Dictionary<string, object> entry = new Dictionary<string, object>();
            entry.Add("NAME", (string)info.Name);
            entry.Add("RANGE", (int)info.Range);
            entry.Add("ATTACK", (int)info.Attack);
            entry.Add("DEFENSE", (int)info.Defense);
            entry.Add("DURABILITY", (int)info.Durability);
            entry.Add("COST", (int)info.Cost);
            list.Add(entry);
        }
        return list;
    }
}


