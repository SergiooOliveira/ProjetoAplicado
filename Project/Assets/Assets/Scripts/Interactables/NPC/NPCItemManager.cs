using TMPro;
using UnityEngine;

public class NPCItemManager : MonoBehaviour
{
    [SerializeField] private Transform itemList;
    [SerializeField] private GameObject itemBuySlot;    

    private NPC npc;
    private PlayerData playerData;

    private void Awake()
    {
        npc = GetComponentInParent<NPC>();
        playerData = npc.PlayerData;
    }

    public void OnEnable()
    {
        DeleteList();

        if (this.name == "BuyMenu")
        {
            SetAllBuySlots();
        }
        else if (this.name == "SellMenu")
        {
            SetAllSellSlots();
        }
    }

    #region Setters
    /// <summary>
    /// Call this method to set all the buy slots
    /// </summary>
    private void SetAllBuySlots()
    {
        if (npc == null)
        {
            Debug.Log($"NPC {npc.RuntimeData.NPCName} is null");
            return;
        }

        foreach (ItemEntry entry in npc.RuntimeData.NPCInventory)
        {
            if (!entry.item.RunTimeItemData.IsItemSellable) continue;
            if (entry.quantity <= 0) continue;

            //Debug.Log($"Setting buy slot: {entry.item.RunTimeItemData.ItemName}");

            GameObject newSlot = Instantiate(itemBuySlot, itemList);

            NPCitemSlot slotUI = newSlot.GetComponent<NPCitemSlot>();
            
            slotUI.SetItem(entry);
        }

        foreach (EquipmentEntry entry in npc.RuntimeData.NPCEquipment)
        {
            if (!entry.equipment.RunTimeEquipmentData.IsItemSellable) continue;
            if (entry.quantity <= 0) continue;

            //Debug.Log($"Setting buy slot: {entry.equipment.RunTimeEquipmentData.ItemName}");

            GameObject newSlot = Instantiate(itemBuySlot, itemList);

            NPCitemSlot slotUI = newSlot.GetComponent<NPCitemSlot>();

            slotUI.SetEquipment(entry);
        }
    }

    /// <summary>
    /// Call this method to set all the sell slots
    /// </summary>
    private void SetAllSellSlots()
    {
        if (npc == null)
        {
            Debug.Log($"Npc {this.name} is null");
            return;
        }

        foreach (ItemEntry entry in playerData.CharacterInventory)
        {
            if (!entry.item.RunTimeItemData.IsItemSellable) continue;
            if (entry.quantity <= 0) continue;

            GameObject newSlot = Instantiate(itemBuySlot, itemList);

            NPCitemSlot slotUI = newSlot.GetComponent<NPCitemSlot>();

            slotUI.SetItem(entry);
        }

        foreach (EquipmentEntry entry in playerData.CharacterEquipment)
        {
            //Debug.Log($"<Color=red>{entry.equipment.RunTimeEquipmentData.ItemName} is {(entry.isEquipped ? "equipped" : "not equipped")}</Color>");
            if (!entry.equipment.RunTimeEquipmentData.IsItemSellable) continue;
            if (entry.quantity <= 0) continue;
            if (entry.isEquipped) continue;

            GameObject newSlot = Instantiate(itemBuySlot, itemList);

            NPCitemSlot slotUI = newSlot.GetComponent<NPCitemSlot>();

            slotUI.SetEquipment(entry);
        }
    }
    #endregion

    private void DeleteList()
    {
        foreach (Transform i in itemList)
        {
            if (i.name == "Title") continue;
            Destroy(i.gameObject);
        }
    }
}
