using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentUpgrade : MonoBehaviour
{
    [SerializeField] private Image img_sprite;
    [SerializeField] private TMP_Text tb_name;
    [SerializeField] private TMP_Text tb_stats;
    [SerializeField] private Button btn_upgrade;

    [Header("Upgrade Costs")]
    [SerializeField] private Transform costTransform;
    [SerializeField] private GameObject costPrefab;

    private EquipmentData equipmentData;
    private EquipmentEntry equipmentEntry;
    private PlayerData playerData;
    private StatManagerUI statManagerUI;
    private InventoryManagerUI inventoryManagerUI;

    EquipmentUpgradeLevel eul;

    public void Initialize(EquipmentData equipmentData, PlayerData playerData, StatManagerUI statManagerUI, InventoryManagerUI inventoryManagerUI)
    {
        this.equipmentData = equipmentData;
        this.playerData = playerData;
        this.equipmentEntry = playerData.CharacterEquipedEquipment.Find(e => e.equipment.RunTimeEquipmentData.ItemName == equipmentData.ItemName);
        this.statManagerUI = statManagerUI;
        this.inventoryManagerUI = inventoryManagerUI;

        PopulateSlot();
    }

    private void PopulateSlot()
    {
        img_sprite.sprite = equipmentData.ItemPrefab.GetComponent<SpriteRenderer>().sprite;
        img_sprite.color = equipmentData.ItemPrefab.GetComponent<SpriteRenderer>().color;
        tb_name.text = equipmentData.ItemName;
        ClearItems();

        if (equipmentData.UpgradeLevels.Count == 0)
        {
            tb_stats.text = "No upgrades for this equipment";
            btn_upgrade.enabled = false;
            btn_upgrade.GetComponent<Image>().color = Color.gray;
            return;
        }

        if (equipmentData.CurrentLevel >= equipmentData.UpgradeLevels.Count)
        {
            tb_stats.text = "Max upgraded";
            btn_upgrade.enabled = false;
            btn_upgrade.GetComponent<Image>().color = Color.gray;
            return;
        }

        #region Stats
        eul = equipmentData.UpgradeLevels[equipmentData.CurrentLevel];

        StringBuilder stats = new StringBuilder();

        if (eul.BonusHp != 0) stats.AppendLine($"+{eul.BonusHp} HP");
        if (eul.BonusAttackDamage != 0) stats.AppendLine($"+{eul.BonusAttackDamage} Attack Damage");
        if (eul.BonusAttackSpeed != 0) stats.AppendLine($"+{eul.BonusAttackSpeed} Attack Speed");
        if (eul.BonusDefense != 0) stats.AppendLine($"+{eul.BonusDefense} Defense");
        if (eul.BonusMana != 0) stats.AppendLine($"+{eul.BonusMana} Mana");
        if (eul.BonusMovementSpeed != 0) stats.AppendLine($"+{eul.BonusMovementSpeed} Movement Speed");
        
        foreach (Resistance res in eul.BonusResistances)
        {
            stats.AppendLine($"+{res.Amount} % {res.SpellAfinity} Resistance");
        }

        foreach (Resistance res in eul.BonusDamageAffinity)
        {
            stats.AppendLine($"+{res.Amount} % {res.SpellAfinity} Damage");
        }

        tb_stats.text = stats.ToString().TrimEnd();

        if (CanUpgrade())
        {
            btn_upgrade.enabled = true;
            btn_upgrade.GetComponent<Image>().color = Color.green;
        }
        else
        {
            btn_upgrade.enabled = false;
            btn_upgrade.GetComponent<Image>().color = Color.red;
        }
        #endregion

        if (eul.CostGold > 0)
        {
            GameObject costGO = Instantiate(costPrefab, costTransform);
            ItemCostUI icUI = costGO.GetComponent<ItemCostUI>();
            icUI.Initialize(eul.CostGold);
        }

        foreach (ItemCost ic in eul.CostItem)
        {
            GameObject costGO = Instantiate(costPrefab, costTransform);
            ItemCostUI icUI = costGO.GetComponent<ItemCostUI>();
            icUI.Initialize(ic);
        }
    }

    private bool CanUpgrade()
    {
        if (eul.CostGold > playerData.CharacterGold) return false;

        foreach (ItemCost ic in eul.CostItem)
        {
            ItemEntry item = playerData.CharacterInventory.Find(i => i.item.RunTimeItemData.ItemName == ic.item.RunTimeItemData.ItemName);
            if (item.item == null || ic.quantity > item.quantity) return false;
        }

        return true;
    }

    private void ClearItems()
    {
        foreach (Transform t in costTransform)
        {
            GameObject.Destroy(t.gameObject);
        }
    }

    private void CostDeduct()
    {
        playerData.AddGold(-eul.CostGold);

        foreach (ItemCost ic in eul.CostItem)
        {
            playerData.RemoveItem(ic.item, ic.quantity);
        }
    }

    public void BtnUpgrade()
    {
        CostDeduct();
        playerData.UnequipEquipment(equipmentEntry);
        equipmentData.LevelUp(eul);
        playerData.EquipEquipment(equipmentEntry);
        PopulateSlot();
        statManagerUI.UpdateUI();
        inventoryManagerUI.UpdateList();
    }

    public void BtnClose()
    {
        Destroy(gameObject);
    }
}
