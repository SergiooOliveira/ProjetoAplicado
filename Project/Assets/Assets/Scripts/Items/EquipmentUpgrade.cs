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

    private EquipmentData equipmentData;
    private PlayerData playerData;

    EquipmentUpgradeLevel eul;

    private bool canUpgrade = false;

    public void Initialize(EquipmentData equipmentData, PlayerData playerData)
    {
        this.equipmentData = equipmentData;
        this.playerData = playerData;

        PopulateSlot();
    }

    private void PopulateSlot()
    {
        img_sprite.sprite = equipmentData.ItemPrefab.GetComponent<SpriteRenderer>().sprite;
        img_sprite.color = equipmentData.ItemPrefab.GetComponent<SpriteRenderer>().color;

        tb_name.text = equipmentData.ItemName;

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

    public void BtnUpgrade()
    {
        Debug.Log(eul.ToString());
        equipmentData.LevelUp(eul);
    }

    public void BtnClose()
    {
        Destroy(gameObject);
    }
}
