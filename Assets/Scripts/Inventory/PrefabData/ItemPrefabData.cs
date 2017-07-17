using UnityEngine;

public class ItemPrefabData : MonoBehaviour {
    // Item parameters
    public string Name;
    public int Range;
    public int Attack;
    public int Defense;
    public int Durability;
    public int Cost;

    public void Initialize(string itemName, int range, int attack, int defense, int durability, int cost)
    {
        Name = itemName;
        Range = range;
        Attack = attack;
        Defense = defense;
        Durability = durability;
        Cost = cost;
    }
}
