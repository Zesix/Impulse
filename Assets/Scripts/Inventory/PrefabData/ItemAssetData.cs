using UnityEngine;

[System.Serializable]
public class ItemAssetData : ScriptableObject
{
    // Item parameters
    public string Name;
    public int Range;
    public int Attack;
    public int Defense;
    public int Durability;
    public int Cost;

    public ItemAssetData(string name, int range, int attack, int defense, int durability, int cost)
    {
        Name = name;
        Range = range;
        Attack = attack;
        Defense = defense;
        Durability = durability;
        Cost = cost;
    }
}
