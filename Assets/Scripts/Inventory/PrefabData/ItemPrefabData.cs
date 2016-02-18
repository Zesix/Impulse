using UnityEngine;
using System.Collections;

public class ItemPrefabData : MonoBehaviour {
    // Item parameters
    public string Name;
    public int Range;
    public int Attack;
    public int Defense;
    public int Durability;
    public int Cost;

    public void Initialize(string name, int range, int attack, int defense, int durability, int cost)
    {
        this.Name = name;
        this.Range = range;
        this.Attack = attack;
        this.Defense = defense;
        this.Durability = durability;
        this.Cost = cost;
    }
}
