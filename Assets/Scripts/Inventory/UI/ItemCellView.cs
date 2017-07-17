using UnityEngine;
using UnityEngine.UI;

public class ItemCellView : MonoBehaviour
{
    // Item visual representation
    public Text Name;
    public Text Range;
    public Text Attack;
    public Text Defense;
    public Text Durability;
    public Text Cost;

    public void SetData(ItemData data)
    {
        gameObject.name = data.Name;

        Name.text = data.Name;
        Range.text = data.Range.ToString();
        Attack.text = data.Attack.ToString();
        Defense.text = data.Defense.ToString();
        Durability.text = data.Durability.ToString();
        Cost.text = data.Cost.ToString();
    }
}
