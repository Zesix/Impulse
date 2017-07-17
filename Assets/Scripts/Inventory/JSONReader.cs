using System.Collections.Generic;
using UnityEngine;

public class JsonReader : MonoBehaviour
{
    [System.Serializable]
    public class ItemsData
    {
        public List<ItemStruct> Items;
    }

    [System.Serializable]
    public struct ItemStruct
    {
        public string Name;
        public int Range;
        public int Attack;
        public int Defense;
        public int Durability;
        public int Cost;
    }

    public static List<Dictionary<string, object>> Read(string file)
    {
        var list = new List<Dictionary<string, object>>();
        var data = Resources.Load(file) as TextAsset;

        if (data == null) return null;
        var parsedJason = JsonUtility.FromJson<ItemsData>(data.text);

        foreach (ItemStruct info in parsedJason.Items)
        {
            var entry = new Dictionary<string, object>
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


