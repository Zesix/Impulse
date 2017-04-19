using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class JsonReader : MonoBehaviour
{
    public static List<Dictionary<string, object>> Read(string file)
    {
        var list = new List<Dictionary<string, object>>();
        TextAsset data = Resources.Load(file) as TextAsset;

        if (data == null) return null;
        ItemData[] parsedJason = JsonConvert.DeserializeObject<ItemData[]>(data.text);
        foreach (ItemData info in parsedJason)
        {
            Dictionary<string, object> entry = new Dictionary<string, object>
            {
                { "NAME", info.Name },
                { "RANGE", info.Range },
                { "ATTACK", info.Attack },
                { "DEFENSE", info.Defense },
                { "DURABILITY", info.Durability },
                { "COST", info.Cost }
            };
            list.Add(entry);
        }
        return list;
    }
}


