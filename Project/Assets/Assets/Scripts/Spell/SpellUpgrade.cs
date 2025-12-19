using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellUpgrade : MonoBehaviour
{
    [SerializeField] private Image img_slot;
    [SerializeField] private TMP_Text tb_stats;
    [SerializeField] private Button btn_upgrade;

    public void Initialize(Spell spell)
    {
        SpellData spellData = spell.RuntimeSpellData;

        Debug.Log($"Spell level: {spellData.SpellLevel}");

        img_slot.sprite = spellData.SpellPrefab.GetComponent<SpriteRenderer>().sprite;
        img_slot.color = spellData.SpellPrefab.GetComponent<SpriteRenderer>().color;

        // if player has enough materials to upgrade, show in green, else red and disabled
        // if spell is max level, show in grey and change text to "Max Level"

        SpellUpgradeLevel sul = spellData.SpellUpgradeLevels[spellData.SpellLevel];

        if (sul == null)
        {
            tb_stats.text = "Max upgraded";
            btn_upgrade.enabled = false;
            return;
        }

        StringBuilder stats = new StringBuilder();

        if (sul.BonusCooldown != 0) stats.AppendLine($"- {sul.BonusCooldown} Cooldown\n");
        if (sul.BonusManaCost != 0) stats.AppendLine($"- {sul.BonusManaCost} Mana cost\n");
        if (sul.BonusCastTime != 0) stats.AppendLine($"- {sul.BonusCastTime} Cast time\n");

        if (sul.BonusEffects.Count > 0)
        {
            foreach (SpellEffect s in sul.BonusEffects)
            {
                stats.AppendLine(s.AddEffectString());
            }
        }
    }
}
