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
    }

    public void SetItem(ItemEntry i)
    {
        item = i;

        if (item.item == null)
        {
            Debug.Log($"Setting {item.item.RunTimeItemData.ItemName}");
            return;
        }

        tb_name.text = item.item.RunTimeItemData.ItemName;
        tb_amount.text = item.quantity.ToString();

        btn_buy.GetComponent<TMP_Text>().text = item.item.RunTimeItemData.ItemSellValue.ToString();
    }

    public void SetEquipment(EquipmentEntry e)
    {
        equipment = e;

        if (equipment.equipment == null) return;

        tb_name.text = equipment.equipment.RunTimeEquipmentData.ItemName;
        tb_amount.text = equipment.quantity.ToString();

        btn_buy.GetComponent<TMP_Text>().text = equipment.equipment.RunTimeEquipmentData.ItemSellValue.ToString();
    }


}
