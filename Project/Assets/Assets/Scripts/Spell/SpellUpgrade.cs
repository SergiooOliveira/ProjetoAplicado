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
        Debug.Log($"Spell level: {spell.RuntimeSpellData.SpellLevel}");

        img_slot.sprite = spell.RuntimeSpellData.SpellPrefab.GetComponent<SpriteRenderer>().sprite;
        img_slot.color = spell.RuntimeSpellData.SpellPrefab.GetComponent<SpriteRenderer>().color;

        // if player has enough materials to upgrade, show in green, else red and disabled
        // if spell is max level, show in grey and change text to "Max Level"
    }
}
