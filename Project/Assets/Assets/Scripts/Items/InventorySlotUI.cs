using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    private Item itemData;
    private Image icon;

    public void SetSlot(Item item)
    {
        itemData = item;

        if (itemData ==  null)
        {
            icon.enabled = false;
        }
        else
        {
            icon.enabled = true;
            icon.sprite = itemData.ItemPrefab.GetComponent<SpriteRenderer>().sprite;
        }
    }
}