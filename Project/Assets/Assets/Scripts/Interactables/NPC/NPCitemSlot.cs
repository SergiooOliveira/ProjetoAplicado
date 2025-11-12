using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCitemSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text tb_name;
    [SerializeField] private TMP_Text tb_amount;
    [SerializeField] private Button btn_buy;

    private NPC npc;
    private PlayerData playerData;

    ItemEntry item;
    EquipmentEntry equipment;

    private void Awake()
    {
        npc = this.GetComponentInParent<NPC>();
        playerData = npc.PlayerData;

        if (npc == null) Debug.Log("NPC null");
        if (playerData == null) Debug.Log("Player null");
    }

    public void SetItem(ItemEntry i)
    {
        item = i;

        Debug.Log($"Setting {item.item.RunTimeItemData.ItemName}");

        if (item.item == null)
        {
            Debug.LogWarning($"{item.item.RunTimeItemData.ItemName} null");
            return;
        }

        tb_name.text = item.item.RunTimeItemData.ItemName;
        //tb_amount.text = item.quantity.ToString();
        tb_amount.text = npc.RuntimeData.NPCInventory.Find(i => i.item.RunTimeItemData.ItemName == item.item.RunTimeItemData.ItemName).quantity.ToString();

        btn_buy.GetComponentInChildren<TMP_Text>().text = item.item.RunTimeItemData.ItemSellValue.ToString();
    }

    public void SetEquipment(EquipmentEntry e)
    {
        equipment = e;

        if (equipment.equipment == null) return;

        tb_name.text = equipment.equipment.RunTimeEquipmentData.ItemName;
        tb_amount.text = equipment.quantity.ToString();

        btn_buy.GetComponent<TMP_Text>().text = equipment.equipment.RunTimeEquipmentData.ItemSellValue.ToString();
    }

    public void BuyButton()
    {
        // If no player return. Shouldnt ever do this
        if (playerData == null) return;

        // If its an item
        if (item.item != null)
        {
            // Check if player has gold and if there is any item to buy
            if (playerData.CharacterGold >= item.item.RunTimeItemData.ItemSellValue)
            {
                int index = npc.RuntimeData.NPCInventory.FindIndex(i => i.item.RunTimeItemData.ItemName == item.item.RunTimeItemData.ItemName);

                if (index >= 0)
                {
                    // Item exists and has an amount
                    ItemEntry buyingItem = npc.RuntimeData.NPCInventory[index];

                    if (buyingItem.quantity > 0)
                    {
                        // Add item to player
                        playerData.AddItem(item, 1);

                        // Remove 1 item from NPC inventory
                        buyingItem.RemoveQuantity(1);
                        npc.RuntimeData.NPCInventory[index] = buyingItem;

                        UpdateUI();
                    }
                    else
                    {
                        Debug.Log("Not enough Items left to buy");
                    }
                }
                else
                {
                    Debug.Log("Item doesn't exist? HOW?");
                }
            }
            else
            {
                // Do something to warn Player. Maybe flash a red light on the button (?)
                Debug.Log("No Mula");
            }
        }// Then its an equipment
        else if (equipment.equipment != null)
        {
            playerData.AddEquip(equipment);
        }
    }

    private void UpdateUI()
    {
        tb_amount.text = npc.RuntimeData.NPCInventory.Find(i => i.item.RunTimeItemData.ItemName == item.item.RunTimeItemData.ItemName).quantity.ToString();
    }

}
