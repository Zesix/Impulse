using System;
using System.Collections.Generic;
using UnityEngine;

public class JsonReader : MonoBehaviour
{
    [System.Serializable]
    public class itemsData
    {
        public List<itemStruct> items;
    }

    [System.Serializable]
    public struct itemStruct
    {
        public string NAME;
        public int RANGE;
        public int ATTACK;
        public int DEFENSE;
        public int DURABILITY;
        public int COST;
    }

    public static List<Dictionary<string, object>> Read(string file)
    {
        var list = new List<Dictionary<string, object>>();
        TextAsset data = Resources.Load(file) as TextAsset;

        if (data == null) return null;
        itemsData parsedJason = JsonUtility.FromJson<itemsData>(data.text);

        foreach (itemStruct info in parsedJason.items)
        {
            Dictionary<string, object> entry = new Dictionary<string, object>
            {
                { "NAME", info.NAME },
                { "RANGE", info.RANGE },
                { "ATTACK", info.ATTACK },
                { "DEFENSE", info.DEFENSE },
                { "DURABILITY", info.DURABILITY },
                { "COST", info.COST }
            };
            list.Add(entry);
        }
        return list;
    }
}


