using TMPro;
using UnityEngine;

public class InventoryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform itemListPanel;
    [SerializeField] private GameObject slot;

    private Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    private void OnEnable()
    {
        DeleteList();
        SetAllSlots();
    }

    public void SetAllSlots()
    {
        foreach (InventoryItem entry in player.RunTimePlayerData.CharacterInventory)
        {
            GameObject newSLot = Instantiate(slot, itemListPanel);

            InventorySlotUI slotUI = newSLot.GetComponent<InventorySlotUI>();
            slotUI.SetSlot(entry);
        }
    }

    /// <summary>
    /// Call this method to delete all the items in the inventory
    /// </summary>
    public void DeleteList()
    {
        foreach (Transform i in itemListPanel)
        {
            Destroy(i.gameObject);
        }
    }

    public void ClickedElement(GameObject gameObject)
    {
        Debug.Log($"Clicked slot {gameObject.name}");
    }
}
