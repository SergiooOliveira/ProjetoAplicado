using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private TMP_Text tb_Name;
    [SerializeField] private TMP_Text tb_Amount;

    private InventoryEntry entry;
    
    /// <summary>
    /// Call this method to set the text values in each slot
    /// </summary>
    /// <param name="i"></param>
    public void SetSlot(InventoryEntry e)
    {
        entry = e;

        if (entry.item == null) return;

        tb_Name.text = entry.item.RunTimeItemData.ItemName;
        tb_Amount.text = entry.quantity.ToString();
    }

    /// <summary>
    /// This method is called when the mouse is hover an element
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameObject slot = eventData.pointerEnter.gameObject;
        
        Transform name = slot.transform.Find("Name");

        TMP_Text tb_name = slot.GetComponent<TMP_Text>();

        Debug.Log($"Hovering over {tb_name.text} slot");
    }

}