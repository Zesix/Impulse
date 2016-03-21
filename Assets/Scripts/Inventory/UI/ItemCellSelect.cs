using UnityEngine;
using System.Collections;

public class ItemCellSelect : MonoBehaviour
{

    // Notification to post.
    public const string ItemCellSelectNotification = "ItemCell.SelectNotification";

    public void OnItemCellSelect()
    {
        this.PostNotification(ItemCellSelectNotification);
        Debug.Log("Notification posted");
    }
}
