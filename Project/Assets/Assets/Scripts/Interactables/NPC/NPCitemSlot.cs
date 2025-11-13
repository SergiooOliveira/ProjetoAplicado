using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class NPCitemSlot : MonoBehaviour
{
    #region Variables
    [SerializeField] private TMP_Text tb_name;
    [SerializeField] private TMP_Text tb_amount;
    [SerializeField] private Button btn_buy;    

    private NPC npc;
    private NPCShopManager npcShopManager;
    private PlayerData playerData;

    private ItemEntry item;
    private EquipmentEntry equipment;

    private bool isBuy;

    private void Awake()
    {
        // Getting components
        npc = this.GetComponentInParent<NPC>();
        npcShopManager = this.GetComponentInParent<NPCShopManager>();
        playerData = npc.PlayerData;

        // See if there is any error
        if (npc == null) Debug.Log("NPC null");
        if (playerData == null) Debug.Log("Player null");

        if (this.transform.parent.name.Contains("Buy"))
        {
            isBuy = true;
        }
        else if (this.transform.parent.name.Contains("Sell"))
        {
            isBuy = false;
        }

    }
    #endregion

    #region Setters
    /// <summary>
    /// Call this method if its an item slot
    /// </summary>
    /// <param name="i">Item</param>
    public void SetItem(ItemEntry i)
    {
        item = i;

        if (item.item == null)
        {
            Debug.LogWarning($"{item.item.RunTimeItemData.ItemName} null");
            return;
        }

        UpdateUI();
    }

    /// <summary>
    /// Call this method if its an equipment slot
    /// </summary>
    /// <param name="e">Equipment</param>
    public void SetEquipment(EquipmentEntry e)
    {
        equipment = e;

        if (equipment.equipment == null)
        {
            Debug.LogWarning($"{equipment.equipment.RunTimeEquipmentData.ItemName} null");
            return;
        }

        UpdateUI();
    }
    #endregion

    /// <summary>
    /// This button is called when pressed
    /// </summary>
    public void BuyButton()
    {
        // If no player return. Shouldnt ever do this
        if (playerData == null)
        {
            Debug.Log("Player is null");
            return;
        }

        // Check if its an item or equipment
        if (item.item != null && item.quantity > 0)
        {
            ItemEntry buyingItem;
            int index;

            // Item exists
            if (isBuy)
            {                
                // Check if player has enough money
                if (playerData.CharacterGold >= item.item.RunTimeItemData.ItemSellValue)
                {
                    // Get index from NPC
                    index = npc.RuntimeData.NPCInventory.FindIndex(i => i.item.RunTimeItemData.ItemName == item.item.RunTimeItemData.ItemName);

                    // Get item from NPC inventory
                    buyingItem = npc.RuntimeData.NPCInventory[index];

                    // Player buys item
                    playerData.AddItem(buyingItem, 1);
                    playerData.AddGold(-buyingItem.item.RunTimeItemData.ItemSellValue);

                    // Remove quantity and reasign
                    buyingItem.RemoveQuantity(1);
                    npc.RuntimeData.NPCInventory[index] = buyingItem;

                    item = buyingItem;

                    if (item.quantity <= 0)
                    {
                        npc.RuntimeData.NPCInventory.RemoveAt(index);
                        Destroy(this.gameObject);
                    }
                }
            }
            else
            {
                item = playerData.RemoveItem(item, 1);
                playerData.AddGold(item.item.RunTimeItemData.ItemSellValue);

                if (item.quantity <= 0)
                    Destroy(this.gameObject);
            }
        }
        else if (equipment.equipment != null && equipment.quantity > 0)
        {
            EquipmentEntry buyingEquipment;
            int index;

            // Equipment exists
            if (isBuy)
            {
                if (playerData.CharacterGold >= equipment.equipment.RunTimeEquipmentData.ItemSellValue)
                {
                    // Get index of the item from the NpcEquipment
                    index = npc.RuntimeData.NPCEquipment.FindIndex(e => e.equipment.RunTimeEquipmentData.ItemName == equipment.equipment.RunTimeEquipmentData.ItemName);

                    // Get Equipment from list
                    buyingEquipment = npc.RuntimeData.NPCEquipment[index];

                    // Player add equipment and lose gold
                    playerData.AddEquip(buyingEquipment);
                    playerData.AddGold(-buyingEquipment.equipment.RunTimeEquipmentData.ItemSellValue);

                    buyingEquipment.RemoveQuantity(1);
                    npc.RuntimeData.NPCEquipment[index] = buyingEquipment;

                    equipment = buyingEquipment;

                    if (equipment.quantity <= 0)
                    {
                        npc.RuntimeData.NPCEquipment.RemoveAt(index);
                        Destroy(this.gameObject);
                    }
                }
            }
            else
            {
                equipment = playerData.RemoveEquip(equipment);
                playerData.AddGold(equipment.equipment.RunTimeEquipmentData.ItemSellValue);

                if (equipment.quantity <= 0)
                    Destroy(this.gameObject);
            }
        }
        else
        {
            if (item.item == null) Debug.Log("Item is null");
            if (equipment.equipment == null) Debug.Log("Equipment is null");
        }

        // Update amount
        UpdateUI();
    }

    private void UpdateUI()
    {
        npcShopManager.UpdateGoldUI(playerData);

        if (item.item != null)
        {
            tb_name.text = item.item.RunTimeItemData.ItemName;
            btn_buy.GetComponentInChildren<TMP_Text>().text = $"{(isBuy ? "Buy: " : "Sell: ")}" + item.item.RunTimeItemData.ItemSellValue.ToString();

            if (isBuy)
                tb_amount.text = npc.RuntimeData.NPCInventory.Find(i => i.item.RunTimeItemData.ItemName == item.item.RunTimeItemData.ItemName).quantity.ToString();
            else
                tb_amount.text = playerData.CharacterInventory.Find(i => i.item.RunTimeItemData.ItemName == item.item.RunTimeItemData.ItemName).quantity.ToString();
        }
        else if (equipment.equipment != null)
        {
            tb_name.text = equipment.equipment.RunTimeEquipmentData.ItemName;
            btn_buy.GetComponentInChildren<TMP_Text>().text = $"{(isBuy ? "Buy: " : "Sell: ")}" + equipment.equipment.RunTimeEquipmentData.ItemSellValue.ToString();

            if (isBuy)
            {                
                tb_amount.text = npc.RuntimeData.NPCEquipment.Find(e => e.equipment.RunTimeEquipmentData.ItemName == equipment.equipment.RunTimeEquipmentData.ItemName).quantity.ToString();
            }
            else
            {             
                tb_amount.text = playerData.CharacterEquipment.Find(e => e.equipment.RunTimeEquipmentData.ItemName == equipment.equipment.RunTimeEquipmentData.ItemName).quantity.ToString();
            }
        }
    }

}
