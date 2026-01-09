using TMPro;
using UnityEngine;

public class InventoryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform itemListPanel;
    [SerializeField] private GameObject slot;

    private Player player;
    private GameObject upgradeInstance;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    private void OnEnable()
    {
        UpdateList();
    }

    public void UpdateUpgradeInstance(GameObject upgradeInstance)
    {
        this.upgradeInstance = upgradeInstance;
    }

    private void OnDisable()
    {
        if (upgradeInstance != null)
        {
            Destroy(upgradeInstance);
            upgradeInstance = null;
        }
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

            InventorySlot slotUI = newSlot.GetComponent<InventorySlot>();
            slotUI.SetEquipmentSlot(entry);
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

    public void UpdateList()
    {
        DeleteList();
        SetAllSlots();
    }

    public void ClickedElement(GameObject gameObject)
    {
        Debug.Log($"Clicked slot {gameObject.name}");
    }
}
