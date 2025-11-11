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

    private void OnDisable()
    {
        // TODO: Get all the tooltips that might exist and destroy them
    }

    public void SetAllSlots()
    {
        foreach (ItemEntry entry in player.RunTimePlayerData.CharacterInventory)
        {
            GameObject newSLot = Instantiate(slot, itemListPanel);

            InventorySlot slotUI = newSLot.GetComponent<InventorySlot>();
            slotUI.SetItemSlot(entry);
        }

        foreach (EquipmentEntry entry in player.RunTimePlayerData.CharacterEquipment)
        {
            GameObject newSlot = Instantiate(slot, itemListPanel);

            InventorySlot sslotUI = newSlot.GetComponent<InventorySlot>();
            sslotUI.SetEquipmentSlot(entry);
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
