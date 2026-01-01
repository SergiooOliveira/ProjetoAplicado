using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellUpgrade : MonoBehaviour
{
    [SerializeField] private Image img_slot;
    [SerializeField] private TMP_Text tb_stats;
    [SerializeField] private Button btn_upgrade;

    private SpellData spellData;
    private PlayerData playerData;
    private Image btn_upgradeImage;

    SpellUpgradeLevel sul;

    public void Initialize(Spell spell, PlayerData playerData)
    {
        spellData = spell.RuntimeSpellData;
        this.playerData = playerData;
        btn_upgradeImage = btn_upgrade.GetComponent<Image>();

        Debug.Log($"Spell level: {spellData.SpellLevel}");

        PopulateSlot();
    }

    private void PopulateSlot()
    {
        img_slot.sprite = spellData.SpellPrefab.GetComponent<SpriteRenderer>().sprite;
        img_slot.color = spellData.SpellPrefab.GetComponent<SpriteRenderer>().color;

        // if player has enough materials to upgrade, show in green, else red and disabled
        // if spell is max level, show in grey and change text to "Max Level"

        // Means, no upgrade available
        if (spellData.SpellUpgradeLevels.Count == 0)
        {
            tb_stats.text = "No upgrades for this spell";
            btn_upgrade.enabled = false;
            btn_upgradeImage.color = Color.gray;
            return;
        }

        if (spellData.SpellLevel >= spellData.SpellUpgradeLevels.Count)
        {
            tb_stats.text = "Max upgraded";
            btn_upgrade.enabled = false;
            btn_upgradeImage.color = Color.gray;
            return;
        }

        sul = spellData.SpellUpgradeLevels[spellData.SpellLevel];

        StringBuilder stats = new StringBuilder();

        if (sul.BonusCooldown != 0) stats.AppendLine($"{sul.BonusCooldown} Cooldown");
        if (sul.BonusManaCost != 0) stats.AppendLine($"{sul.BonusManaCost} Mana cost");
        if (sul.BonusCastTime != 0) stats.AppendLine($"{sul.BonusCastTime} Cast time");

        if (sul.BonusEffects.Count > 0)
        {
            foreach (SpellEffect s in sul.BonusEffects)
            {
                stats.AppendLine(s.AddEffectString());
            }
        }

        tb_stats.text = stats.ToString();

        if (CanUpgrade())
        {
            btn_upgrade.enabled = true;
            btn_upgradeImage.color = Color.green;
        }
        else
        {
            btn_upgrade.enabled = false;
            btn_upgradeImage.color = Color.red;
        }
    }

    private bool CanUpgrade()
    {
        // Gold check
        if (sul.CostGold > playerData.CharacterGold) return false;

        // Item check
        foreach (ItemCost ic in sul.UpgradeItemCost)
        {
            ItemEntry item = playerData.CharacterInventory.Find(i => i.item.RunTimeItemData.ItemName == ic.item.RunTimeItemData.ItemName);

            if (item.item == null || ic.quantity > item.quantity) return false;
        }

        return true;
    }

    public void BtnUpgrade()
    {
        // Deduct the cost
        playerData.AddGold(-sul.CostGold);

        foreach (ItemCost ic in sul.UpgradeItemCost)
        {
            ItemEntry item = playerData.CharacterInventory.Find(i => i.item.RunTimeItemData.ItemName == ic.item.RunTimeItemData.ItemName);

            playerData.RemoveItem(item, ic.quantity);
        }

        spellData.LevelUp(sul);
        PopulateSlot();
    }
}
