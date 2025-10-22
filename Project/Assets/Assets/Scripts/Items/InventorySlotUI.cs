using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private TMP_Text tb_Name;
    [SerializeField] private TMP_Text tb_Amount;

    private Item itemData;
    
    /// <summary>
    /// Call this method to set the text values in each slot
    /// </summary>
    /// <param name="item"></param>
    public void SetSlot(Item item)
    {
        itemData = item;

        if (itemData == null) return;

        tb_Name.text = itemData.RunTimeItemData.ItemName;
        tb_Amount.text = itemData.RunTimeItemData.ItemQuantity.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        eventData.pointerEnter.gameObject.TryGetComponent<TMP_Text>(out TMP_Text hoverText);
        Debug.Log(hoverText.text);
    }

}